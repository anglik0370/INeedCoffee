using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeUI : MonoBehaviour
{
    public Image fillImg;

    private const float FADE_DURATION = 1f;

    public bool isEmpty {get; private set;}

    public void Empty()
    {
        isEmpty = true;
        fillImg.DOFade(0f, FADE_DURATION);
    }

    public void Fill()
    {
        fillImg.color = new Color(1, 1, 1, 1);
        isEmpty = false;
    }
}
