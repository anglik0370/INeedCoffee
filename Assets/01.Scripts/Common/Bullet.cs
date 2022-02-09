using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;

    [Header("총알 피해량")]
    public float damage;

    [Header("총알 속도")]
    public float speed;
    [Header("밀어내는 힘")]
    public float pushPower;

    [Header("비활성화 시간")]
    public float disableTime;

    private const string ENEMY_TAG = "ENEMY";

    private WaitForSeconds ws;
    private Coroutine co;

    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();

        ws = new WaitForSeconds(disableTime);
    }

    private void OnEnable() 
    {
        co = StartCoroutine(DisableRoutine());
    }
    private void OnDisable() 
    {
        StopCoroutine(co);
        rigid.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag(ENEMY_TAG))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.OnDamage(damage, rigid.velocity.normalized * pushPower);

            EnemyHitEffect hitEffect = PoolManager.GetItem<EnemyHitEffect>();
            hitEffect.SetPosition(transform.position);

            gameObject.SetActive(false);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Shot(Vector3 dir)
    {
        rigid.velocity = dir * speed;
    }

    private IEnumerator DisableRoutine()
    {
        yield return ws;

        gameObject.SetActive(false);
    }
}
