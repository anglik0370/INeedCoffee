using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUpgradeCard : MonoBehaviour
{
    [SerializeField]
    private Button btn;

    [SerializeField]
    private Text nameTxt;
    [SerializeField]
    private Image upgradeImg;
    [SerializeField]
    private Text explanationTxt;

    [SerializeField]
    private BulletUpgradeType type;

    public bool canPush = false;

    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            if (!canPush) return;

            switch (type)
            {
                case BulletUpgradeType.DAMAGE:
                    BulletManager.Instance.UpgradeDamage();
                    break;
                case BulletUpgradeType.SPEED:
                    BulletManager.Instance.UpgradeSpeed();
                    break;
                case BulletUpgradeType.KNOCKBACK:
                    BulletManager.Instance.UpgradePushPower();
                    break;
                default:
                    return;
            }

            GameManager.Instance.OccurPause(false);
        });
    }

    public void Init(BulletUpgradeSO so)
    {
        canPush = false;

        type = so.type;

        nameTxt.text = so.nameStr;
        
        if(upgradeImg.sprite == null)
        {
            upgradeImg.color = new Color(upgradeImg.color.r, upgradeImg.color.g, upgradeImg.color.b, 0);
        }
        else
        {
            upgradeImg.sprite = so.sprite;
        }

        explanationTxt.text = so.explanationStr;
    }
}
