using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AleartUI : MonoBehaviour
{
    private Text alertText;
    private RectTransform rect;

    [SerializeField]
    private Vector2 originPos;
    [SerializeField]
    private Vector2 upPos;

    private const float TWEEN_DURATION = 2f;
    private Sequence seq;

    private void Awake() 
    {
        rect = GetComponent<RectTransform>();
        alertText = GetComponent<Text>();

        rect.anchoredPosition = originPos;
        alertText.color = new Color(alertText.color.r, alertText.color.g, alertText.color.b, 0);
    }

    public void Aleart(string msg)
    {
        if(seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        alertText.text = msg;
        alertText.color = new Color(alertText.color.r, alertText.color.g, alertText.color.b, 1);
        rect.anchoredPosition = originPos;

        seq.Append(DOTween.To(() => rect.anchoredPosition, x => rect.anchoredPosition = x, upPos, TWEEN_DURATION));
        seq.Join(alertText.DOFade(0, TWEEN_DURATION));
    }
}
