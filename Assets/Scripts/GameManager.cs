using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public const int patternLength = 16;

    [SerializeField] private FoodPattern[] _foodPatterns;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private Stove[] _stoves;
    [SerializeField] private Transform _foodSpawnPos;
    [SerializeField] private List<FoodInstance> _activeFoods;

    public event Action<float> FoodUpdateEvent;

    private int _currentPattern;
    private float _musicSpeed;
    private int _hearts;

    private bool _gameIsNotOver = true;
    private bool RELOADTHESCENE = false;

    void Start()
    {
        Instance = this;
        _currentPattern = -1;
        _hearts = 3;
        
        _gameIsNotOver = true;

        SetMusicSpeed(0.85f);
        NextPattern();
    }

    void Update()
    {
        ReadInput();;

        if (!_gameIsNotOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }


            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }

    public IEnumerator PatternUpdate(FoodPattern pattern)
    {
        FoodDingeje[] foodList = pattern.FoodList;

        int patternId = _musicManager.LoopAmount;
        int spawnCount = 0;

        _musicManager.PlayClip(pattern.BGMClip);

        while (patternId == _musicManager.LoopAmount && _gameIsNotOver)
        {
            float patternTime = _musicManager.ElapsedBeats - patternLength * patternId;

            if (spawnCount < foodList.Length)
            {
                if (patternTime >= foodList[spawnCount].Timing)
                {
                    SpawnFoodItem(foodList[spawnCount]);
                    spawnCount++;
                }
            }

            FoodUpdateEvent?.Invoke(patternTime);
            yield return new WaitForEndOfFrame();
        }
    }


    public void LoseHeart()
    {
        _hearts--;
        HeartContainer.Instance.LoseAHeart();

        if (_hearts <= 0)
        {
            // Oei
            _gameIsNotOver = false;
            GameOverContainer.Instance.gameObject.SetActive(true);


            _musicManager._audioSource.DOPitch(0, 0.5f);
        }
    }

    public void NextPattern()
    {
        if (_currentPattern + 1 == _foodPatterns.Length)
        {
            // Loop and increase speed
            _currentPattern = 0;
            AddMusicSpeed(0.15f);

            _musicManager.LoopAmount = 0;
            _musicManager.ElapsedTime = 0;
        }
        else
        {
            // Continue
            _currentPattern++;
        }

        StartCoroutine(PatternUpdate(_foodPatterns[_currentPattern]));
    }

    public void SpawnFoodItem(FoodDingeje dingetje)
    {
        // Pick random stove
        Stove stove = null;

        List<Stove> stovePool = _stoves.ToList();

        while (stove == null)
        {
            int r = UnityEngine.Random.Range(0, stovePool.Count);
            if (!stovePool[r].IsOccupied)
            {
                stove = stovePool[r];
                break;
            }
            else
            {
                stovePool.RemoveAt(r);
            }
        }

        // Spawn food
        var g = Instantiate(dingetje.Food.Prefab);
        var instance = g.GetComponent<FoodInstance>();

        float realHitTime = (_musicManager.LoopAmount * patternLength + dingetje.Timing + dingetje.Food.ThrowTime
             + dingetje.Food.FryTime) / _musicManager.ClipBPM * 60.0f;

        instance.InitFood(dingetje, realHitTime, stove, _foodSpawnPos.position);

        _activeFoods.Add(instance);
        FoodUpdateEvent += instance.UpdateFood;
    }

    public void EradicateFoodObject(FoodInstance instance)
    {
        _activeFoods.Remove(instance);
        instance.Stove.Empty();
        FoodUpdateEvent -= instance.UpdateFood;
        Destroy(instance.gameObject);
    }

    private void ReadInput()
    {
        if (_activeFoods.Count == 0)
            return;

        float patternTime = _musicManager.ElapsedBeats - patternLength * _musicManager.LoopAmount;

        if (Input.anyKeyDown)
        {
            // Find food closest to finishing
            int savedId = -1;
            float savedDistance = Int32.MaxValue;

            for (int i = 0; i < _activeFoods.Count; i++)
            {
                FoodInstance food = _activeFoods[i];
                float d = food.RealHitTime;
                if (d < savedDistance)
                {
                    savedId = i;
                    savedDistance = d;
                }
            }

            _activeFoods[savedId].Hit(_musicManager.ElapsedTime);
        }
    }

    public void SetMusicSpeed(float speed)
    {
        _musicSpeed = speed;
        _musicManager._audioSource.pitch = speed;
    }

    public void AddMusicSpeed(float speed)
    {
        SetMusicSpeed(_musicSpeed + speed);
    }
}
