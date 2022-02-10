using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class PausePanel : MonoBehaviour
{
    private CanvasGroup cvs;

    [SerializeField]
    private Button continueBtn;
    [SerializeField]
    private Button backToMainBtn;
    [SerializeField]
    private Button pauseBtn;

    private const float TWEEN_DURATION = 0.5f;
    private Tween tween;

    private void Awake() 
    {
        cvs = GetComponent<CanvasGroup>();

        pauseBtn.onClick.AddListener(() => 
        {
            if(tween != null)
            {
                tween.Complete();
                tween.Kill();
            }

            pauseBtn.interactable = false;

            tween = cvs.DOFade(1f, TWEEN_DURATION);
            cvs.interactable = true;
            cvs.blocksRaycasts = true;
        });

        continueBtn.onClick.AddListener(() => 
        {
            if(tween != null)
            {
                tween.Complete();
                tween.Kill();
            }

            pauseBtn.interactable = true;

            tween = cvs.DOFade(0f, TWEEN_DURATION);
            cvs.interactable = false;
            cvs.blocksRaycasts = false;
        });

        backToMainBtn.onClick.AddListener(() => 
        {
            if(tween != null)
            {
                tween.Complete();
                tween.Kill();
            }

            pauseBtn.interactable = true;

            cvs.alpha = 0f;
            cvs.interactable = false;
            cvs.blocksRaycasts = false;
        });

        cvs.alpha = 0f;
        cvs.interactable = false;
        cvs.blocksRaycasts = false;
    }

    public void SubPauseOnClick(Action Callback)
    {
        pauseBtn.onClick.AddListener(() => Callback?.Invoke());
    }

    public void SubContunueOnClick(Action Callback)
    {
        continueBtn.onClick.AddListener(() => Callback?.Invoke());
    }

    public void SubBackToMainOnClick(Action Callback)
    {
        backToMainBtn.onClick.AddListener(() => Callback?.Invoke());
    }
}
