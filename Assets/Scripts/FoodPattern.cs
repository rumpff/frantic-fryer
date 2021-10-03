using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern", menuName = "FoodPattern")]
public class FoodPattern : ScriptableObject
{
    [Header("!!! Don't forget items must be in order !!!")]
    public FoodDingeje[] FoodList;
}

[Serializable]
public struct FoodDingeje
{
    public float Timing;
    public FoodItem Food;
}
