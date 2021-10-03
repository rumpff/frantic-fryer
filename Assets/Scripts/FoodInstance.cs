using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using JetBrains.Annotations;
using UnityEngine;

public class FoodInstance : MonoBehaviour
{
    public const float MaxJump = 4;
    public const float TimingOvershoot = 0.2f;
    public const float EarlyMiss = -0.35f;

    private IFoodState currentState;
    public FoodDingeje Dingetje { get; private set; }

    public float RealHitTime;
    public Vector3 SpawnPos;
    public Stove Stove;

    public void InitFood(FoodDingeje food, float realHitTime, Stove stove, Vector3 spawnPos)
    {
        Dingetje = food;
        RealHitTime = realHitTime;
        SpawnPos = spawnPos;
        Stove = stove;

        stove.IsOccupied = true;
        transform.position = spawnPos;

        currentState = new ThrowState();
        currentState.StateEnter(this);
    }

    public void UpdateFood(float patternTime)
    {
        currentState.StateUpdate(patternTime);
    }

    public void SetState(IFoodState newState)
    {
        currentState = newState;
        currentState.StateEnter(this);
    }

    public void ChangeState(IFoodState newState)
    {
        currentState.StateExit();
        currentState = newState;
        currentState.StateEnter(this);
    }

    public void Hit(float time)
    {
        float d = time - RealHitTime;

        if (d < EarlyMiss)
        {
            // Too early, cancel
            return;
        }

        if (Mathf.Abs(d) <= TimingOvershoot)
        {
            // Hit
            Debug.Log("hit!");
        }
        else
        {
            // Miss
            Debug.Log("miss!");
        }

        GameManager.Instance.EradicateFoodObject(this);
    }

    public interface IFoodState
    {
        void StateEnter(FoodInstance foodInstance);
        void StateUpdate(float patternTime);
        void StateExit();
    }

    private class ThrowState : IFoodState
    {
        private FoodInstance Instance;
        void IFoodState.StateEnter(FoodInstance foodInstance)
        {
            Instance = foodInstance;
        }

        void IFoodState.StateUpdate(float patternTime)
        {
            float t = (patternTime - Instance.Dingetje.Timing) / Instance.Dingetje.Food.ThrowTime;

            Vector3 position = new Vector3()
            {
                y = Instance.Stove.transform.position.y + Instance.Dingetje.Food.ThrowCurve.Evaluate(t) * MaxJump,
                x = Mathf.Lerp(Instance.SpawnPos.x, Instance.Stove.transform.position.x, t),
                z = Mathf.Lerp(Instance.SpawnPos.z, Instance.Stove.transform.position.z, t)
            };

            Instance.transform.position = position;

            if (t >= 1)
                Instance.ChangeState(new FryState());
        }

        void IFoodState.StateExit()
        {

        }
    }

    private class FryState : IFoodState
    {
        private FoodInstance Instance;
        private Stove Stove;

        void IFoodState.StateEnter(FoodInstance foodInstance)
        {
            Instance = foodInstance;
            Stove = foodInstance.Stove;

            Instance.transform.position = Stove.transform.position;
            Stove.IsFrying = true;
        }

        void IFoodState.StateUpdate(float patternTime)
        {
            float t = (patternTime - Instance.Dingetje.Timing - Instance.Dingetje.Food.ThrowTime) / Instance.Dingetje.Food.FryTime;
            Stove.SetProgressbar(Instance.Dingetje.Food.FryCurve.Evaluate(t));

            if (MusicManager.Instance.ElapsedTime > Instance.RealHitTime + FoodInstance.TimingOvershoot)
            {
                Instance.Hit(MusicManager.Instance.ElapsedTime);
            }
        }

        void IFoodState.StateExit()
        {
            Stove.IsFrying = false;
        }
    }
}
