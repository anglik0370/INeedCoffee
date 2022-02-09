using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sr;

    Player player;
    HealthBar healthBar;

    Sequence seq;
    private const float TWEEN_DURATION = 0.5f;

    private Coroutine co;
    private const float COROUTINE_DURATION = 0.2f;
    private WaitForSeconds ws;

    public float maxHp;
    public float curHp;

    public float speed;
    public Vector3 dir;

    public float attackSpeed;
    private float attackTimer;
    public float attackDistance;

    private void Awake() 
    {
        healthBar = GetComponentInChildren<HealthBar>();
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        
        ws = new WaitForSeconds(COROUTINE_DURATION);
        seq = DOTween.Sequence();
    }

    private void Start() 
    {
        player = GameManager.GetPlayer();
        curHp = maxHp;
    }

    private void Update() 
    {
        dir = (player.transform.position - transform.position).normalized;

        if(dir.x < 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        transform.position += dir * speed * Time.deltaTime;

        if(Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {
            if(attackTimer <= 0)
            {
                Attack();
                attackTimer += attackSpeed;
            }
        }

        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private void OnDisable() 
    {
        if(co != null)
        {
            seq.Kill();
            StopCoroutine(co);
        }

        curHp = maxHp;
    }

    public void Attack()
    {
        player.OnDamage();

        EnemyAttackEffect attackEffect = PoolManager.GetItem<EnemyAttackEffect>();
        attackEffect.SetPosition(transform.position);
        attackEffect.SetRotation((player.transform.position - transform.position).normalized);
    }

    public void OnDamage(float damage, Vector3 push)
    {
        //밀리는 효과
        Vector2 dir = new Vector2(push.x, push.y);

        rigid.AddForce(dir, ForceMode2D.Impulse);

        if(seq != null)
        {
            seq.Kill();
            seq = DOTween.Sequence();
        }

        seq.Append(DOTween.To(() => rigid.velocity, x => rigid.velocity = x, Vector2.zero, TWEEN_DURATION));

        //색이 바뀌는 효과
        sr.material.SetInt("_IsMask", 1);

        if(co != null)
        {
            StopCoroutine(co);
        }

        if(gameObject.activeSelf)
        {
            StartCoroutine(SetOriginColor());
        }

        seq.Join(healthBar.transform.DOShakePosition(TWEEN_DURATION, 0.1f, 50));

        //실제로 대미지 받는 부분
        if(curHp - damage <= 0)
        {
            curHp = 0;
            healthBar.UpdateHealthBar(maxHp, curHp);
            Die();
        }

        curHp -= damage;

        healthBar.UpdateHealthBar(maxHp, curHp);
    }

    private void Die()
    {
        EnemyDeadEffect deadEffect = PoolManager.GetItem<EnemyDeadEffect>();
        deadEffect.SetPosition(transform.position);

        sr.material.SetInt("_IsMask", 0);

        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private IEnumerator SetOriginColor()
    {
        yield return ws;
        sr.material.SetInt("_IsMask", 0);
    }
}
