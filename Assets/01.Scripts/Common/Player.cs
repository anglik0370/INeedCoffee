using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    public Gun gun;

    public float limitDistance;
    public float speed;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector3 dir)
    {
        if((transform.position + dir * speed * Time.deltaTime).x > limitDistance 
            || (transform.position + dir * speed * Time.deltaTime).x < -limitDistance)
        {
            dir.x = 0;
        }
        if((transform.position + dir * speed * Time.deltaTime).y > limitDistance
            || (transform.position + dir * speed * Time.deltaTime).y < -limitDistance)
        {
            dir.y = 0;
        }

        if(dir != Vector3.zero)
        {
            if(dir.x > 0)
            {
                //0일때 정방향, 1일때 역방향
                anim.SetFloat("IsReverse", 1f);
                gun.SetAnimIsReverse(1f);
            }
            else
            {
                anim.SetFloat("IsReverse", 0f);
                gun.SetAnimIsReverse(0f);
            }

            anim.SetBool("IsMove", true);
            gun.SetAnimIsMove(true);
        }
        else
        {
            anim.SetBool("IsMove", false);
            gun.SetAnimIsMove(false);
        }

        transform.position += dir * speed * Time.deltaTime;
    }

    public void Shot(Vector3 shotDir)
    {
        gun.Shot(shotDir);
    }

    public void OnDamage()
    {
        GameManager.Instance.PlayerDamaged();

        PlayerHitSoundEffect soundEffect = PoolManager.GetItem<PlayerHitSoundEffect>();
        soundEffect.Play();
    }
}
