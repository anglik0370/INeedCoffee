using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class MuzzleFlash : MonoBehaviour
{
    Light2D flash;
    Tween tween;

    public float lifeTime = 0.2f;
    float maxIntensity;
    float minIntensity = 0;

    private void Awake() 
    {
        flash = GetComponent<Light2D>();
        maxIntensity = flash.intensity;
        flash.intensity = 0f;
    }

    private void OnEnable() 
    {
        flash.intensity = maxIntensity;
        tween = DOTween.To(() => flash.intensity, x => flash.intensity = x, minIntensity, lifeTime)
                            .OnComplete(() => gameObject.SetActive(false));
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
