using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHitEffectUI : MonoBehaviour
{
    RectTransform rect;
    Image img;

    public Vector3 originScale;
    public Vector3 bigScale;
    public Vector3 smaleScale;

    private const float TWEEN_DURATION = 0.2f;
    private Sequence seq;

    private void Awake() 
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        img.color = new Color(1, 1, 1, 0);
    }

    public void Play()
    {
        if(seq != null)
        {
            rect.localScale = originScale;
            seq.Kill();
        }

        seq = DOTween.Sequence();

        img.color = new Color(1, 1, 1, 1);

        seq.Append(DOTween.To(() => rect.localScale, x => rect.localScale = x, bigScale, TWEEN_DURATION * 2));
        seq.Append(DOTween.To(() => rect.localScale, x => rect.localScale = x, smaleScale, TWEEN_DURATION));
        seq.Join(img.DOFade(0f, TWEEN_DURATION));
    }
}
