using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletUpgradeCards : MonoBehaviour
{
    private CanvasGroup cvs;

    [SerializeField]
    private List<BulletUpgradeCard> cardList = new List<BulletUpgradeCard>();
    [SerializeField]
    private List<BulletUpgradeSO> upgradeSOList = new List<BulletUpgradeSO>();

    private const float TWEEN_DURATION = 0.5f;
    private Sequence seq;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.Instance.SubPause(pause =>
        {
            if(!pause)
            {
                if (seq != null)
                {
                    seq.Kill();
                }

                seq = DOTween.Sequence();

                seq.Append(cvs.DOFade(0f, TWEEN_DURATION));
                seq.AppendCallback(() =>
                {
                    cvs.interactable = false;
                    cvs.blocksRaycasts = false;
                });
            }
        });

        cvs.alpha = 0f;
        cvs.interactable = true;
        cvs.blocksRaycasts = false;
    }

    public void StartUpgrade()
    {
        GameManager.Instance.OccurPause(true);

        for(int i = 0; i < cardList.Count; i++)
        {
            cardList[i].Init(upgradeSOList[i]);
        }

        if(seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        seq.Append(cvs.DOFade(1f, TWEEN_DURATION));
        seq.AppendCallback(() =>
        {
            cvs.interactable = true;
            cvs.blocksRaycasts = true;

            cardList.ForEach(x => x.canPush = true);
        });
    }
}
