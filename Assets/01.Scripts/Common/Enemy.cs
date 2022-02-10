using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer sr;

    HealthBar healthBar;

    Sequence seq;
    private const float TWEEN_DURATION = 0.5f;

    private Coroutine co;
    private const float COROUTINE_DURATION = 0.2f;
    private WaitForSeconds ws;

    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float curHp;

    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 dir;

    [SerializeField]
    private float attackDistance;

    public Queue<Action> damageQueue = new Queue<Action>();

    private bool isPause = false;

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
        EnemyManager.Instance.SubUpgradeHealth((health, increment) => 
        {
            maxHp = health;
            curHp += increment;
            curHp = Mathf.Clamp(curHp, 0, maxHp);
        });

        EnemyManager.Instance.SubUpgradeMoveSpeed(moveSpeed => 
        {
            this.speed = moveSpeed;
        });

        GameManager.Instance.SubPause(isPause => 
        {
            this.isPause = isPause;
        });

        isPause = false;
    }

    private void Update() 
    {
        if(isPause) return;

        dir = EnemyManager.Instance.GetTowardPlayerDir(transform.position);

        if(dir.x < 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        transform.position += dir * speed * Time.deltaTime;

        if(EnemyManager.Instance.GetPlayerDist(transform.position) <= attackDistance)
        {
            EnemyManager.Instance.AddAttackReq(transform.position);
            gameObject.SetActive(false);
        }

        if(damageQueue.Count > 0)
        {
            damageQueue.Dequeue()?.Invoke();
        }
    }

    private void OnEnable() 
    {
        maxHp = EnemyManager.Instance.EnemyHealth;
        speed = EnemyManager.Instance.EnemyMoveSpeed;

        if(maxHp == 0)
        {
            maxHp = EnemyManager.ORIGIN_ENEMY_HEALTH;
        }

        if(speed == 0)
        {
            speed = EnemyManager.ORIGIN_ENEMY_MOVESPEED;
        }

        curHp = maxHp;
        healthBar.UpdateHealthBar(maxHp, curHp);
        
        healthBar.ResetTransform();
        if(co != null)
        {
            seq.Kill();
            StopCoroutine(co);
        }

        sr.material.SetInt("_IsMask", 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackDistance);
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

        seq.Join(healthBar.transform.DOShakePosition(TWEEN_DURATION, 0.2f, 30));

        //실제로 대미지 받는 부분
        if(curHp - damage <= 0)
        {
            Die();
        }

        curHp -= damage;

        healthBar.UpdateHealthBar(maxHp, curHp);
    }

    public void Die()
    {
        curHp = 0;
        healthBar.UpdateHealthBar(maxHp, curHp);

        damageQueue.Clear();

        EnemyManager.Instance.AddDeadReq(transform.position);

        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void UpdateHealthBar()
    {
        healthBar.UpdateHealthBar(maxHp, curHp);
    }

    private IEnumerator SetOriginColor()
    {
        yield return ws;
        sr.material.SetInt("_IsMask", 0);
    }
}
