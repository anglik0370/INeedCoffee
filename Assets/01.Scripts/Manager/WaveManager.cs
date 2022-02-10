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

        isGameStart = false;
    }

    private void Start() 
    {
        GameManager.Instance.SubGameStart(() => 
        {
            isGameStart = true;

            if(co != null)
            {
                StopCoroutine(co);
            }

            co = StartCoroutine(EnemySpawnRoutine());
        });

        GameManager.Instance.SubGameOver(() =>
        {
            isGameStart = false;
        });

        EnemyManager.Instance.SubUpgradeSpawnDelay(spawnDelay => 
        {
            this.spawnDelay = spawnDelay;
            ws = new WaitForSeconds(this.spawnDelay);
        });

        spawnDelay = EnemyManager.ORIGIN_ENEMY_SPAWNDELAY;
        ws = new WaitForSeconds(spawnDelay);
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
        float randomAngle = Random.Range(0, 359) * Mathf.Deg2Rad;
        Vector2 randomPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * spawnRangeRadius;

        enemy.SetPosition((Vector3)randomPos + playerPos);
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
