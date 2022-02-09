using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Player player;

    [Header("프리팹")]
    public Bullet bulletPrefab;
    public MuzzleFlash muzzleFlashPrefab;
    public Catridge CatridgePreafab;
    public EnemyHitEffect hitEffectPrefab;
    public EnemyDeadEffect deadEffectPrefab;
    public EnemyAttackEffect enemyAttackEffectPrefab;

    [Header("핸들러")]
    public LifeUIHandler lifeUIHandler;

    [Header("풀매니저")]
    public Transform poolManagerTrm;

    [Header("라이프")]
    public float maxLife;
    public float life;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<Bullet>(bulletPrefab.gameObject, poolManagerTrm, 15);
        PoolManager.CreatePool<MuzzleFlash>(muzzleFlashPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<Catridge>(CatridgePreafab.gameObject, poolManagerTrm, 13);
        PoolManager.CreatePool<EnemyHitEffect>(hitEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<EnemyDeadEffect>(deadEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<EnemyAttackEffect>(enemyAttackEffectPrefab.gameObject, poolManagerTrm, 10);
    }

    private void Start() 
    {
        life = maxLife;
    }

    public static Player GetPlayer()
    {
        return Instance.player;
    }

    public void GetDamage()
    {
        if(life - 1 == 0)
        {
            //게임오버
        }

        life--;
        lifeUIHandler.RemoveLife();
    }
}
