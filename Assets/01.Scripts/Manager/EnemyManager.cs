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
    public EnemyDeadSoundEffect deadSoundEffectPrefab;

    [Header("풀매니저")]
    public Transform poolManagerTrm;

    private Player player;

    private Queue<Action> deadSoundQueue = new Queue<Action>();
    private Queue<Action> deadEffectQueue = new Queue<Action>();
    private Queue<Action> playerAttackQueue = new Queue<Action>();

    public const float UPGRADE_DELAY = 10;

    public const float ORIGIN_ENEMY_HEALTH = 10;
    public const float ORIGIN_ENEMY_MOVESPEED = 2;
    public const float ORIGIN_ENEMY_SPAWNDELAY = 5;

    private const float ENEMY_HEALTH_INCREMENT = 1;
    private const float ENEMY_MOVESPEED_INCREMENT = 0.2f;
    private const float ENEMY_SPAWNDELAY_INCREMENT = -0.3f;

    private const float ENEMY_SPAWNDELAY_MIN = 0.5f;

    public float EnemyHealth => enemyHealth;
    public float EnemyMoveSpeed => enemyMoveSpeed;

    [SerializeField]
    private float enemyHealth;
    [SerializeField]
    private float enemyMoveSpeed;
    [SerializeField]
    private float enemySpawnDelay;

    private Action<float, float> EnemyHealthUpgraded = (a, b) => {};
    private Action<float> EnemyMoveSpeedUpgraded = a => {};
    private Action<float> EnemySpawnDelayUpgraded = a => {};

    private List<Action> upgradeList = new List<Action>();

    private bool isGameStart = false;
    private bool isPause = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<Enemy>(enemyPrefab.gameObject, poolManagerTrm, 30);
        PoolManager.CreatePool<EnemyHitEffect>(hitEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<EnemyDeadEffect>(deadEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<EnemyDeadSoundEffect>(deadSoundEffectPrefab.gameObject, poolManagerTrm, 30);

        upgradeList.Add(UpgradeHealth);
        upgradeList.Add(UpgradeMoveSpeed);
        upgradeList.Add(UpgradeSpawnDelay);

        enemyHealth = ORIGIN_ENEMY_HEALTH;
        enemyMoveSpeed = ORIGIN_ENEMY_MOVESPEED;
        enemySpawnDelay = ORIGIN_ENEMY_SPAWNDELAY;

        isGameStart = false;
        isPause = false;
    }

    private void Start() 
    {
        player = GameManager.GetPlayer();

        GameManager.Instance.SubGameStart(() => 
        {
            isGameStart = true;
        });

        GameManager.Instance.SubGameOver(() =>
        {
            isGameStart = false;

            EnemyDeadSoundEffect soundEffect = PoolManager.GetItem<EnemyDeadSoundEffect>();
            soundEffect.Play();

            foreach(Enemy enemy in PoolManager.GetItemList<Enemy>())
            {
                if(!enemy.gameObject.activeSelf) continue;
                
                EnemyDeadEffect deadEffect = PoolManager.GetItem<EnemyDeadEffect>();
                deadEffect.SetPosition(enemy.transform.position);

                enemy.gameObject.SetActive(false);
            }

            deadSoundQueue.Clear();
            deadEffectQueue.Clear();
            playerAttackQueue.Clear();

            enemyHealth = ORIGIN_ENEMY_HEALTH;
            enemyMoveSpeed = ORIGIN_ENEMY_MOVESPEED;
            enemySpawnDelay = ORIGIN_ENEMY_SPAWNDELAY;
        });

        GameManager.Instance.SubBackToMain(() => 
        {
            isGameStart = false;

            foreach(Enemy enemy in PoolManager.GetItemList<Enemy>())
            {
                if(!enemy.gameObject.activeSelf) continue;

                enemy.gameObject.SetActive(false);
            }

            deadSoundQueue.Clear();
            deadEffectQueue.Clear();
            playerAttackQueue.Clear();

            enemyHealth = ORIGIN_ENEMY_HEALTH;
            enemyMoveSpeed = ORIGIN_ENEMY_MOVESPEED;
            enemySpawnDelay = ORIGIN_ENEMY_SPAWNDELAY;
        });

        GameManager.Instance.SubPause(isPause => 
        {
            this.isPause = isPause;
        });
    }

    private void Update() 
    {
        if(!isGameStart || isPause) return;

        if(deadSoundQueue.Count > 0)
        {
            deadSoundQueue.Dequeue()?.Invoke();
            deadSoundQueue.Clear();
        }

        if(playerAttackQueue.Count > 0)
        {
            playerAttackQueue.Dequeue()?.Invoke();
            playerAttackQueue.Clear();
        }

        while(deadEffectQueue.Count > 0)
        {
            deadEffectQueue.Dequeue()?.Invoke();
        }
    }

    private void UpgradeHealth()
    {
        enemyHealth += ENEMY_HEALTH_INCREMENT;

        GameManager.Aleart("적의 체력이 늘어났습니다!");

        EnemyHealthUpgraded(enemyHealth, ENEMY_HEALTH_INCREMENT);
    }

    private void UpgradeMoveSpeed()
    {
        enemyMoveSpeed += ENEMY_MOVESPEED_INCREMENT;

        GameManager.Aleart("적의 이동속도가 증가했습니다!");

        EnemyMoveSpeedUpgraded(enemyMoveSpeed);
    }

    private void UpgradeSpawnDelay()
    {
        if(enemySpawnDelay - ENEMY_SPAWNDELAY_INCREMENT < ENEMY_SPAWNDELAY_MIN)
        {
            enemySpawnDelay = ENEMY_SPAWNDELAY_MIN;
            GameManager.Aleart("적의 생성속도가 최고치입니다!");

            upgradeList.Remove(UpgradeSpawnDelay);
        }
        else
        {
            enemySpawnDelay += ENEMY_SPAWNDELAY_INCREMENT;
            GameManager.Aleart("적의 생성속도가 빨라졌습니다!");
        }

        EnemySpawnDelayUpgraded(enemySpawnDelay);
    }

    public void RandomEnemyUpgrade()
    {
        int rand = UnityEngine.Random.Range(0, upgradeList.Count);

        upgradeList[rand]?.Invoke();
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

        deadEffectQueue.Enqueue(() => 
        {
            EnemyDeadEffect deadEffect = PoolManager.GetItem<EnemyDeadEffect>();
            deadEffect.SetPosition(enemyPos);
        });

        deadSoundQueue.Enqueue(() =>
        {
            EnemyDeadSoundEffect soundEffect = PoolManager.GetItem<EnemyDeadSoundEffect>();
            soundEffect.Play();
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
