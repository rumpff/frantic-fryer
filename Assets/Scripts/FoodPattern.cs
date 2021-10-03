using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern", menuName = "FoodPattern")]
public class FoodPattern : ScriptableObject
{
    [Header("!!! Don't forget items must be in order !!!")]
    public FoodDingeje[] FoodList;
    public AudioClip BGMClip;
}

[Serializable]
public struct FoodDingeje
{
    public float Timing;
    public FoodItem Food;
}
