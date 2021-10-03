using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class FoodInstance : MonoBehaviour
{
    private const float MaxJump = 4;
    private const float TimingOvershoot = 0.15f;

    private IFoodState currentState;
    public FoodItem Item { get; private set; }

    public float StartPos;
    public Vector3 SpawnPos;
    public Stove Stove;

    public void InitFood(FoodItem item, float startPos, Stove stove, Vector3 spawnPos)
    {
        Item = item;
        StartPos = startPos;
        SpawnPos = spawnPos;
        Stove = stove;

        //stove.IsOccupied = true;
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

    public void OnFlip()
    {

    }

    public interface IFoodState
    {
        void StateEnter(FoodInstance foodInstance);
        void StateUpdate(float patternTime);
        void StateExit();
        void Flip();
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
            float t = (patternTime - Instance.StartPos) / Instance.Item.ThrowTime;

            Vector3 position = new Vector3()
            {
                y = Instance.Item.ThrowCurve.Evaluate(t) * MaxJump,
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

        void IFoodState.Flip()
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
            // Set stove to frying
        }

        void IFoodState.StateUpdate(float patternTime)
        {
            float t = (patternTime - Instance.StartPos - Instance.Item.ThrowTime) / Instance.Item.FryTime;
        }

        void IFoodState.StateExit()
        {

        }

        void IFoodState.Flip()
        {

        }
    }
}
