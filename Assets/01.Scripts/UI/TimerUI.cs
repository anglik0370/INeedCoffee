using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerUI : MonoBehaviour
{
    private Image timerImg;
    private Text timerText;

    private const float ADD_ROTATE_VALUE = 180;
    private const float TWEEN_DURAITION = 1f;

    private Sequence seq;

    private void Awake() 
    {
        timerImg = GetComponentInChildren<Image>();
        timerText = GetComponentInChildren<Text>();
    }

    public void SetTimerText(float elapsedTime)
    {
        elapsedTime = Mathf.Round(elapsedTime);

        float sec = elapsedTime % 60;
        float min = 0;

        while(min != 0)
        {
            min -= 60;
            min++;
        }

        timerText.text = $"{min.ToString("00")}:{sec.ToString("00")}";
    }

    public void RollTimerImg()
    {   
        if(seq != null)
        {
            seq.Kill();
        }
        
        seq = DOTween.Sequence();

        seq.Append(timerImg.transform.DORotate(new Vector3(0, 0, ADD_ROTATE_VALUE), TWEEN_DURAITION));
        seq.AppendInterval(TWEEN_DURAITION);
        seq.Append(timerImg.transform.DOLocalRotate(new Vector3(0, 0, ADD_ROTATE_VALUE), TWEEN_DURAITION * 2));
        seq.AppendCallback(() => timerImg.transform.rotation = Quaternion.Euler(Vector3.zero));
    }
}
