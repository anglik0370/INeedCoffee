using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{  
    Animator anim;

    public Transform leftMuzzleTrm;
    public Transform rightMuzzleTrm;

    public Transform leftEjectionTrm;
    public Transform rightEjectionTrm;

    public AudioSource shotAudio;
    public AudioSource reloadAudio;
    public AudioSource cantShotAudio;

    public float spreadAngle;

    public bool isReloaded = true;
    public bool isFlip = false;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    public void SetAnimIsMove(bool isMove)
    {
        anim.SetBool("IsMove", isMove);
    }

    public void SetAnimIsReverse(float isReverse)
    {
        anim.SetFloat("IsReverse", isReverse);
        
        if(isReverse == 0f)
        {
            isFlip = false;
        }
        else if(isReverse == 1f)
        {
            isFlip = true;
        }
    }
    
    public bool Shot(Vector3 shotDir)
    {
        if(isReloaded)
        {
            shotAudio.Play();
            anim.SetTrigger("Shot");

            CameraShake.Instance.ShakeCamera(5f, 0.2f);

            //총알 발사부분
            if(!isFlip)
            {
                MuzzleFlashPoolManager.Instance.GetOrCreate(rightMuzzleTrm.position);

                for(int i = 0; i < 5; i++)
                {
                    Bullet bullet = BulletPoolManager.Instance.GetOrCreate(rightMuzzleTrm.position);
                    bullet.Shot(CalcAngle(shotDir));
                }
            }
            else
            {
                MuzzleFlashPoolManager.Instance.GetOrCreate(leftMuzzleTrm.position);

                for(int i = 0; i < 5; i++)
                {
                    Bullet bullet = BulletPoolManager.Instance.GetOrCreate(leftMuzzleTrm.position);
                    bullet.Shot(CalcAngle(shotDir));
                }
            }

            isReloaded = false;
        }
        else
        {
            CameraShake.Instance.ShakeCamera(2f, 0.2f);
            cantShotAudio.Play();
        }

        return isReloaded;
    }

    private Vector3 CalcAngle(Vector3 shotDir)
    {
        float spread = Random.Range(-spreadAngle, spreadAngle);
        return (shotDir + new Vector3(spread, spread, 0)).normalized;
    }

    public void PumpAction()
    {
        Catridge catridge = null;

        if(!isFlip)
        {
            catridge = CatridgePoolManager.Instance.GetOrCreate(rightEjectionTrm.position);
        }
        else
        {
            catridge = CatridgePoolManager.Instance.GetOrCreate(leftEjectionTrm.position);
        }

        catridge.Ejection(isFlip);
        
        reloadAudio.Play();
    }

    public void Reload()
    {
        isReloaded = true;
    }
}
