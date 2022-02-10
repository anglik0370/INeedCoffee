using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Player player;

    [Header("프리팹")]
    public Bullet bulletPrefab;
    public MuzzleFlash muzzleFlashPrefab;
    public Catridge CatridgePreafab;
    public CatridgeDropSoundEffect catridgeDropSoundEffectPrefab;
    public PlayerHitSoundEffect playerHitSoundEffectPrefab;

    [Header("핸들러")]
    public LifeUIHandler lifeUIHandler;
    public KillCountUI killCountUI;

    [Header("풀매니저")]
    public Transform poolManagerTrm;

    [Header("라이프")]
    public float maxLife;
    public float life;

    [Header("킬카운트")]
    public int killCount;
    
    //이벤트
    public Action GameStart = () => {};
    public Action GameOver = () => {};

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<Bullet>(bulletPrefab.gameObject, poolManagerTrm, 15);
        PoolManager.CreatePool<MuzzleFlash>(muzzleFlashPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<Catridge>(CatridgePreafab.gameObject, poolManagerTrm, 13);
        PoolManager.CreatePool<CatridgeDropSoundEffect>(catridgeDropSoundEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<PlayerHitSoundEffect>(playerHitSoundEffectPrefab.gameObject, poolManagerTrm, 3);
    }

    private void Start() 
    {
        life = maxLife;

        killCount = 0;
        killCountUI.UpdateCountText(killCount);
    }

    public static Player GetPlayer()
    {
        return Instance.player;
    }

    public void PlayerDamaged()
    {
        if(life - 1 == 0)
        {
            GameOver?.Invoke();
        }

        life--;
        lifeUIHandler.RemoveLife();
    }

    public void EnemyDead()
    {
        killCount++;
        killCountUI.UpdateCountText(killCount);
    }
}
