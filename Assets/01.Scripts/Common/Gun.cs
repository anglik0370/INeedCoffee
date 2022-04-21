using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{  
    Animator anim;

    [Header("총구 위치")]
    public Transform leftMuzzleTrm;
    public Transform rightMuzzleTrm;

    [Header("약실 위치")]
    public Transform leftEjectionTrm;
    public Transform rightEjectionTrm;

    [Header("오디오 소스들")]
    public AudioSource shotAudio;
    public AudioSource reloadAudio;
    public AudioSource cantShotAudio;
    
    [Header("산탄 범위")]
    public float spreadAngle;
    [Header("산탄 갯수")]
    public int bulletAmount;

    [Header("펌프액션 여부")]
    public bool isPumped = true;
    [Header("Spirte Filp 여부")]
    public bool isFlip = false;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.SubGameStart(() =>
        {
            isPumped = true;
        });
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
        if(isPumped)
        {
            shotAudio.Play();
            anim.SetTrigger("Shot");

            CameraShake.Instance.ShakeCamera(5f, 0.5f);

            MuzzleFlash muzzleFlash = PoolManager.GetItem<MuzzleFlash>();

            //총알 발사부분
            if(!isFlip)
            {
                muzzleFlash.SetPosition(rightMuzzleTrm.position);

                for(int i = 0; i < bulletAmount; i++)
                {
                    Bullet bullet = PoolManager.GetItem<Bullet>();
                    bullet.SetPosition(rightMuzzleTrm.position);
                    bullet.Shot(CalcAngle(shotDir));
                }
            }
            else
            {
                muzzleFlash.SetPosition(leftMuzzleTrm.position);

                for(int i = 0; i < bulletAmount; i++)
                {
                    Bullet bullet = PoolManager.GetItem<Bullet>();
                    bullet.SetPosition(leftMuzzleTrm.position);
                    bullet.Shot(CalcAngle(shotDir));
                }
            }

            isPumped = false;
        }
        else
        {
            CameraShake.Instance.ShakeCamera(2f, 0.2f);
            cantShotAudio.Play();
        }

        return isPumped;
    }

    private Vector3 CalcAngle(Vector3 shotDir)
    {
        float spread = Random.Range(-spreadAngle, spreadAngle);
        return (shotDir + new Vector3(spread, spread, 0)).normalized;
    }

    public void PumpAction()
    {
        Catridge catridge = PoolManager.GetItem<Catridge>();

        if(!isFlip)
        {
            catridge.SetPosition(rightEjectionTrm.position);
        }
        else
        {
            catridge.SetPosition(leftEjectionTrm.position);
        }

        catridge.Ejection(isFlip);
        
        reloadAudio.Play();
    }

    public void Reload()
    {
        isPumped = true;
    }
}
