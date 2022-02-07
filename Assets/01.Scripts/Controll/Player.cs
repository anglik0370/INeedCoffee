using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    public Gun gun;
    public float speed;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector3 dir)
    {
        if(dir != Vector3.zero)
        {
            if(dir.x > 0)
            {
                //0일때 정방향, 1일때 역방향
                anim.SetFloat("IsReverse", 1f);
            }
            else
            {
                anim.SetFloat("IsReverse", 0f);
            }

            anim.SetBool("IsMove", true);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }

        transform.position += dir * speed * Time.deltaTime;
    }

    public void Shot(Vector3 shotDir)
    {
        if(!gun.isReloaded) return;

        anim.SetTrigger("Shot");
        gun.Shot();
    }

    public void PlayReloadSound()
    {
        gun.PlayReloadSound();
    }

    public void Reload()
    {
        gun.Reload();
    }
}
