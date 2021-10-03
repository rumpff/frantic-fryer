using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    public static HeartContainer Instance;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite deadSprite;
    private Stack<Image> heartQueue;

    private void Start()
    {
        Instance = this;

        heartQueue = new Stack<Image>();
        for (int i = 0; i < hearts.Length; i++)
        {
            heartQueue.Push(hearts[i]);
        }
    }

    public void LoseAHeart()
    {
        if (heartQueue.Count == 0)
            return;

        Image heart = heartQueue.Pop();
        heart.sprite = deadSprite;

        Sequence dieSequence = DOTween.Sequence();

        dieSequence.Append(heart.rectTransform.DOAnchorPosY(-115, 1));
        dieSequence.Join(heart.DOFade(0, 1));

        dieSequence.Play();
    }
}
