using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [Header("ÇÁ¸®ÆÕ")]
    public Bullet bulletPrefab;

    public const float ORIGIN_BULLET_DAMAGE = 2;
    public const float ORIGIN_BULLET_SPEED = 10;
    public const float ORIGIN_BULLET_PUSHPOWER = 2;

    private const float BULLET_DAMAGE_INCREMENT = 0.2f;
    private const float BULLET_SPEED_INCREMENT = 1f;
    private const float BULLET_PUSHPOWER_INCREMENT = 0.2f;

    [SerializeField]
    private float bulletDamage;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float bulletPushPower;

    public float BulletDamage => bulletDamage;
    public float BulletSpeed => bulletSpeed;
    public float BulletPushPower => bulletPushPower;

    private Action<float> BulletDamageUpgraded = a => { };
    private Action<float> BulletSpeedUpgraded = a => { };
    private Action<float> BulletPushPowerUpgraded = a => { };

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<Bullet>(bulletPrefab.gameObject, transform, 15);

        bulletDamage = ORIGIN_BULLET_DAMAGE;
        bulletSpeed = ORIGIN_BULLET_SPEED;
        bulletPushPower = ORIGIN_BULLET_PUSHPOWER;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UpgradePushPower();
        }
    }

    private void Start()
    {
        GameManager.Instance.SubGameOver(() =>
        {
            PoolManager.DisableAll<Bullet>();

            bulletDamage = ORIGIN_BULLET_DAMAGE;
            bulletSpeed = ORIGIN_BULLET_SPEED;
            bulletPushPower = ORIGIN_BULLET_PUSHPOWER;
        });

        GameManager.Instance.SubBackToMain(() =>
        {
            PoolManager.DisableAll<Bullet>();

            bulletDamage = ORIGIN_BULLET_DAMAGE;
            bulletSpeed = ORIGIN_BULLET_SPEED;
            bulletPushPower = ORIGIN_BULLET_PUSHPOWER;
        });
    }

    public void SubDamageUpgraded(Action<float> Callback)
    {
        BulletDamageUpgraded += Callback;
    }

    public void SubSpeedUpgraded(Action<float> Callback)
    {
        BulletSpeedUpgraded += Callback;
    }

    public void SubPushPowerUpgraded(Action<float> Callback)
    {
        BulletPushPowerUpgraded += Callback;
    }

    public void UpgradeDamage()
    {
        bulletDamage += BULLET_DAMAGE_INCREMENT;
        BulletDamageUpgraded?.Invoke(bulletDamage);
    }

    public void UpgradeSpeed()
    {
        bulletSpeed += BULLET_SPEED_INCREMENT;
        BulletSpeedUpgraded?.Invoke(bulletSpeed);
    }

    public void UpgradePushPower()
    {
        bulletPushPower += BULLET_PUSHPOWER_INCREMENT;
        BulletPushPowerUpgraded?.Invoke(bulletPushPower);
    }
}
