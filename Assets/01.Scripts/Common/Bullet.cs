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

    private bool isPaused = false;

    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();

        ws = new WaitForSeconds(disableTime);
    }

    private void Start()
    {
        GameManager.Instance.SubPause(pause =>
        {
            isPaused = pause;
        });

        BulletManager.Instance.SubDamageUpgraded(damage =>
        {
            this.damage = damage;
        });

        BulletManager.Instance.SubSpeedUpgraded(speed =>
        {
            this.speed = speed;
        });

        BulletManager.Instance.SubPushPowerUpgraded(pushPower =>
        {
            this.pushPower = pushPower;
        });

        damage = BulletManager.ORIGIN_BULLET_DAMAGE;
        speed = BulletManager.ORIGIN_BULLET_SPEED;
        pushPower = BulletManager.ORIGIN_BULLET_PUSHPOWER;
    }

    private void OnEnable() 
    {
        damage = BulletManager.Instance.BulletDamage;
        speed = BulletManager.Instance.BulletSpeed;
        pushPower = BulletManager.Instance.BulletPushPower;

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

            Vector2 pushDir = rigid.velocity.normalized * pushPower;

            enemy.damageQueue.Enqueue(() =>
            {
                enemy.OnDamage(damage);
            });

            enemy.knockbackQueue.Enqueue(() =>
            {
                enemy.Knockback(pushDir);
            });

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
