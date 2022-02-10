using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance {get; private set;}

    [Header("Enemy관련 프리팹")]
    public Enemy enemyPrefab;
    public EnemyHitEffect hitEffectPrefab;
    public EnemyDeadEffect deadEffectPrefab;
    public EnemyAttackEffect attackEffectPrefab;
    public EnemyDeadSoundEffect deadSoundEffectPrefab;

    [Header("풀매니저")]
    public Transform poolManagerTrm;

    private Player player;

    private Queue<Action> deadSoundQueue = new Queue<Action>();
    private Queue<Action> deadEffectQueue = new Queue<Action>();
    private Queue<Action> playerAttackQueue = new Queue<Action>();
    private Queue<Action> attackEffectQueue = new Queue<Action>();

    private const float ORIGIN_ENEMY_HEALTH = 10;
    private const float ORIGIN_ENEMY_MOVESPEED = 2;
    private const float ORIGIN_ENEMY_SPAWNDELAY = 3;

    private const float ENEMY_HEALTH_INCREMENT = 2;
    private const float ENEMY_MOVESPEED_INCREMENT = 0.1f;
    private const float ENEMY_SPAWNDELAY_INCREMENT = -0.15f;

    private float enemyHealth;
    private float enemyMoveSpeed;
    private float enemySpawnDelay;

    private Action<float, float> EnemyHealthUpgraded = (a, b) => {};
    private Action<float> EnemyMoveSpeedUpgraded = a => {};
    private Action<float> EnemySpawnDelayUpgraded = a => {};


    private bool isGameStart = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<Enemy>(enemyPrefab.gameObject, poolManagerTrm, 30);
        PoolManager.CreatePool<EnemyHitEffect>(hitEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<EnemyDeadEffect>(deadEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<EnemyAttackEffect>(attackEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<EnemyDeadSoundEffect>(deadSoundEffectPrefab.gameObject, poolManagerTrm, 30);

        enemyHealth = ORIGIN_ENEMY_HEALTH;
        enemyMoveSpeed = ORIGIN_ENEMY_MOVESPEED;
        enemySpawnDelay = ORIGIN_ENEMY_SPAWNDELAY;
    }

    private void Start() 
    {
        isGameStart = true;

        player = GameManager.GetPlayer();

        GameManager.Instance.SubGameOver(() =>
        {
            EnemyDeadSoundEffect soundEffect = PoolManager.GetItem<EnemyDeadSoundEffect>();
            soundEffect.Play();

            foreach(Enemy enemy in PoolManager.GetItemList<Enemy>())
            {
                if(!enemy.gameObject.activeSelf) continue;

                enemy.Die();
            }

            enemyHealth = ORIGIN_ENEMY_HEALTH;
            enemyMoveSpeed = ORIGIN_ENEMY_MOVESPEED;
            enemySpawnDelay = ORIGIN_ENEMY_SPAWNDELAY;

            isGameStart = false;
        });
    }

    private void Update() 
    {
        if(!isGameStart) return;

        if(deadSoundQueue.Count > 0)
        {
            deadSoundQueue.Dequeue()?.Invoke();
            deadSoundQueue.Clear();
        }

        while(deadEffectQueue.Count > 0)
        {
            deadEffectQueue.Dequeue()?.Invoke();
        }

        if(playerAttackQueue.Count > 0)
        {
            playerAttackQueue.Dequeue()?.Invoke();
            playerAttackQueue.Clear();
        }

        while(attackEffectQueue.Count > 0)
        {
            attackEffectQueue.Dequeue()?.Invoke();
        }
    }

    public void UpgradeHealth()
    {
        enemyHealth += ENEMY_HEALTH_INCREMENT;

        EnemyHealthUpgraded(enemyHealth, ENEMY_HEALTH_INCREMENT);
    }

    public void UpgradeMoveSpeed()
    {
        enemyMoveSpeed += ENEMY_MOVESPEED_INCREMENT;

        EnemyMoveSpeedUpgraded(enemyMoveSpeed);
    }

    public void UpgradeSpawnDelay()
    {
        enemySpawnDelay += ENEMY_SPAWNDELAY_INCREMENT;

        EnemySpawnDelayUpgraded(enemySpawnDelay);
    }

    public void SubUpgradeHealth(Action<float, float> CallBack)
    {
        EnemyHealthUpgraded += CallBack;
    }

    public void SubUpgradeMoveSpeed(Action<float> CallBack)
    {
        EnemyMoveSpeedUpgraded += CallBack;
    }

    public void SubUpgradeSpawnDelay(Action<float> CallBack)
    {
        EnemySpawnDelayUpgraded += CallBack;
    }

    public void AddDeadReq(Vector3 enemyPos)
    {
        deadEffectQueue.Enqueue(() => 
        {
            GameManager.Instance.EnemyDead();

            EnemyDeadEffect deadEffect = PoolManager.GetItem<EnemyDeadEffect>();
            deadEffect.SetPosition(enemyPos);
        });

        deadSoundQueue.Enqueue(() =>
        {
            EnemyDeadSoundEffect soundEffect = PoolManager.GetItem<EnemyDeadSoundEffect>();
            soundEffect.Play();
        });
    }

    public void AddAttackReq(Vector3 enemyPos)
    {
        playerAttackQueue.Enqueue(() => player.OnDamage());

        attackEffectQueue.Enqueue(() => 
        {
            EnemyAttackEffect attackEffect = PoolManager.GetItem<EnemyAttackEffect>();
            attackEffect.SetPosition(enemyPos);
            attackEffect.SetRotation((player.transform.position - enemyPos).normalized);
        });
    }

    public Vector3 GetTowardPlayerDir(Vector3 pos)
    {
        return (player.transform.position - pos).normalized;
    }

    public float GetPlayerDist(Vector3 pos)
    {
        return Vector3.Distance(player.transform.position, pos);
    }
}
