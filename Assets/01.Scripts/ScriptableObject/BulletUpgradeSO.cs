using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletUpgradeType
{
    DAMAGE,
    AMOUNT,
    SPEED,
    PENETRATE,
    KNOCKBACK,
}

[CreateAssetMenu(menuName = "ScriptableObject/BulletUpgradeSO", fileName = "new BulletUpgradeSO")]
public class BulletUpgradeSO : ScriptableObject
{
    public BulletUpgradeType type;
    public string nameStr;
    public Sprite sprite;
    public string explanationStr;
}
