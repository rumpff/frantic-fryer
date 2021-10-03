using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "FoodItem")]
public class FoodItem : ScriptableObject
{
    public GameObject Prefab;
    [Space(5)]

    public float ThrowTime = 1;
    public float FryTime = 1;
    public AnimationCurve ThrowCurve;
    public AnimationCurve FryCurve;
}
