using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour
{
    private CanvasGroup cvs;

    [SerializeField]
    private Text killCountText;
    [SerializeField]
    private Text lifeTimeText;

    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Button reStartBtn;
    [SerializeField]
    private Button backToMainBtn;
    [SerializeField]
    private Button pauseBtn;

    [SerializeField]
    private bool canTouchBtn = false;

    private const float TWEEN_DURATION = 1f;
    private Sequence seq;

    private void Awake() 
    {
        cvs = GetComponent<CanvasGroup>();

        Close();
    }

    private void Start() 
    {
        reStartBtn.onClick.AddListener(() =>
        {
            if(!canTouchBtn) return;

            GameManager.Instance.GameStart();
            pauseBtn.interactable = true;
            Close();
        });

        backToMainBtn.onClick.AddListener(() => 
        {
            if(!canTouchBtn) return;

            GameManager.Instance.ChangeCvs(false);
            pauseBtn.interactable = true;
            Close();
        });
    }

    public void Open(int killCount, int lifeTime, int highScore, int score)
    {
        if(seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        cvs.blocksRaycasts = true;
        cvs.interactable = true;

        pauseBtn.interactable = false;

        canTouchBtn = false;

        killCountText.text = string.Empty;
        lifeTimeText.text = string.Empty;
        highScoreText.text = string.Empty;
        scoreText.text = string.Empty;

        int tmp0 = 0;
        int tmp1 = 0;
        int tmp2 = 0;

        int min = 0;
        int sec = 0;

        int lifeTimeCopy = lifeTime;
        

        while(lifeTimeCopy > 60)
        {
            lifeTimeCopy -= 60;
            min++;
        }

        seq.Append(cvs.DOFade(1f, TWEEN_DURATION));
        seq.Append(DOTween.To(() => tmp0, x => 
        {
            tmp0 = x;
            killCountText.text = $"x{tmp0}";
        }, killCount, TWEEN_DURATION));

        seq.Append(DOTween.To(() => tmp1, x => 
        {
            tmp1 = x;
        }, min, TWEEN_DURATION));
        seq.Join(DOTween.To(() => sec, x =>
        {
            sec = x;
            lifeTimeText.text = $"{tmp1}:{sec.ToString("00")}";
        },lifeTime % 60, TWEEN_DURATION));

        seq.Append(DOTween.To(() => tmp2, x =>
        {
            tmp2 = x;
            scoreText.text = $"점수 : {tmp2}";
        }, score, TWEEN_DURATION));
        seq.AppendCallback(() => 
        {
            highScoreText.text = $"최고점수 : {highScore}";

            canTouchBtn = true;
        });
    }

    public void Close()
    {
        cvs.alpha = 0f;
        cvs.interactable = false;
        cvs.blocksRaycasts = false;

        canTouchBtn = false;
    }
}
