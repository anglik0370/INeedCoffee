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
    }

    private void Start() 
    {
        isGameStart = true;

        player = GameManager.GetPlayer();

        GameManager.Instance.GameOver += () =>
        {
            EnemyDeadSoundEffect soundEffect = PoolManager.GetItem<EnemyDeadSoundEffect>();
            soundEffect.Play();

            foreach(Enemy enemy in PoolManager.GetItemList<Enemy>())
            {
                if(!enemy.gameObject.activeSelf) continue;

                enemy.Die();
            }

            isGameStart = false;
        };
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
