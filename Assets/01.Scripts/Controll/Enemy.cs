using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public float maxHp;
    public float curHp;

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
        curHp = maxHp;
    }

    private void OnDisable() 
    {
        if(co != null)
        {
            seq.Kill();
            StopCoroutine(co);
            sr.material.SetInt("_IsMask", 0);
        }
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
        gameObject.SetActive(false);
    }

    private IEnumerator SetOriginColor()
    {
        yield return ws;
        sr.material.SetInt("_IsMask", 0);
    }
}
