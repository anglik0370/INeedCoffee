using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;

    public float disableTime;
    public float speed;

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
