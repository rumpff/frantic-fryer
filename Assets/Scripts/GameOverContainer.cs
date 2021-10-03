using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverContainer : MonoBehaviour
{
    public static GameOverContainer Instance;

    private void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
}
