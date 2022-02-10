using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance {get; private set;}

    public Player player;
    public float spawnRangeRadius;

    public float spawnDelay;
    private WaitForSeconds ws;
    private Coroutine co;

    private bool isGameStart = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        ws = new WaitForSeconds(spawnDelay);
    }

    private void Start() 
    {
        isGameStart = true;

        GameManager.Instance.SubGameOver(() =>
        {
            isGameStart = false;
        });

        EnemyManager.Instance.SubUpgradeSpawnDelay(spawnDelay => 
        {
            this.spawnDelay = spawnDelay;
            ws = new WaitForSeconds(this.spawnDelay);
        });

        co = StartCoroutine(EnemySpawnRoutine());
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, spawnRangeRadius);
        Gizmos.color = Color.white;
    }

    private void SpawnEnemyRandomPos(Vector3 playerPos)
    {
        Enemy enemy = PoolManager.GetItem<Enemy>();
        Vector2 randomPos = new Vector2(Mathf.Cos(Random.Range(-180, 180) * Mathf.Deg2Rad) * spawnRangeRadius + playerPos.x, 
                                        Mathf.Sin(Random.Range(-180, 180) * Mathf.Deg2Rad) * spawnRangeRadius + playerPos.y);

        enemy.SetPosition(randomPos);
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while(isGameStart)
        {
            SpawnEnemyRandomPos(player.transform.position);
            yield return ws;
        }
    }
}
