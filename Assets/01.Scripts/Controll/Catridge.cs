using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catridge : MonoBehaviour
{

    public float disableTime;

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

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private IEnumerator DisableRoutine()
    {
        yield return ws;

        gameObject.SetActive(false);
    }
}
