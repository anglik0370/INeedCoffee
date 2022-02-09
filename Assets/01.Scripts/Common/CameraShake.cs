using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance {get; private set;}

    CinemachineVirtualCamera cmVcam;
    
    float shakeTimer;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        cmVcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmPerlin = cmVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cmPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cmPerlin = cmVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                
                cmPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
