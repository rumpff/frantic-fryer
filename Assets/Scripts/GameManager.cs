using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    private const int patternLength = 16;

    [SerializeField] private FoodPattern _testPattern;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private Stove[] _stoves;
    [SerializeField] private Transform _foodSpawnPos;

    public event Action<float> FoodUpdateEvent;

    void Start()
    {
        StartCoroutine(PatternUpdate(_testPattern));
    }

    void Update()
    {
        
    }

    public IEnumerator PatternUpdate(FoodPattern pattern)
    {
        FoodDingeje[] foodList = pattern.FoodList;

        int patternId = _musicManager.LoopAmount;
        int spawnCount = 0;

        while (patternId == _musicManager.LoopAmount)
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

        instance.InitFood(dingetje.Food, dingetje.Timing, stove, _foodSpawnPos.position);
        FoodUpdateEvent += instance.UpdateFood;
    }
}
