using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletUpgradeType
{
    DAMAGE,
    AMOUNT,
    SPEED,
    PENETRATE,
}

[CreateAssetMenu(menuName = "ScriptableObject/BulletUpgradeSO", fileName = "new BulletUpgradeSO")]
public class BulletUpgradeSO : ScriptableObject
{
    public BulletUpgradeType type;
    public string nameStr;
    public string explanationStr;
}
