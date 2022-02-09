using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Catridge : MonoBehaviour
{
    public float disableTime;

    [Header("탄피 Tween 관련 변수들")]
    public Vector3 originRot;
    public Vector3 popPos;
    public Vector3 popRot;
    public Vector3 dropPos;

    private const float POP_DURATION = 0.2f;
    private const float DROP_DURATION = 0.3f;

    private Sequence seq;
    private WaitForSeconds ws;
    private Coroutine co;

    private void Awake() 
    {
        ws = new WaitForSeconds(disableTime);
    }

    private void OnEnable() 
    {
        co = StartCoroutine(DisableRoutine());
    }
    private void OnDisable() 
    {
        StopCoroutine(co);
    }

    public void Ejection(bool isFlip)
    {
        seq = DOTween.Sequence();

        if(!isFlip)
        {
            transform.rotation = Quaternion.Euler(originRot);

            //위로 튀는부분
            seq.Append(transform.DOMove(transform.position + popPos, POP_DURATION));
            seq.Join(transform.DORotate(transform.rotation * popRot, POP_DURATION));

            seq.Append(transform.DOMove(transform.position + dropPos, DROP_DURATION));
            seq.Join(transform.DORotate(transform.rotation * new Vector3(0, 0, Random.Range(-180, 180)), DROP_DURATION));
        }
        else
        {
            transform.rotation = Quaternion.Euler(-originRot);

            //위로 튀는부분
            seq.Append(transform.DOMove(transform.position + new Vector3(-popPos.x, popPos.y, popPos.z), POP_DURATION));
            seq.Join(transform.DORotate(transform.rotation * popRot, DROP_DURATION));

            seq.Append(transform.DOMove(transform.position + new Vector3(-dropPos.x, dropPos.y, dropPos.z), POP_DURATION));
            seq.Join(transform.DORotate(transform.rotation * new Vector3(0, 0, Random.Range(-180, 180)), DROP_DURATION));
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private IEnumerator DisableRoutine()
    {
        yield return ws;

        seq.Kill();
        gameObject.SetActive(false);
    }
}
