using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadEffect : MonoBehaviour
{
    SpriteRenderer sr;

    public Sprite[] sprites;

    private Coroutine co;
    private const float SPRITE_CHANGE_CYCLE = 0.1f;
    private WaitForSeconds ws;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
        ws = new WaitForSeconds(SPRITE_CHANGE_CYCLE);
    }

    private void OnEnable() 
    {
        co = StartCoroutine(ParticleRoutine());
    }

    private void OnDisable() 
    {
        StopCoroutine(co);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180)));
    }

    private IEnumerator ParticleRoutine()
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            sr.sprite = sprites[i];
            yield return ws;
        }

        gameObject.SetActive(false);
    }
}
