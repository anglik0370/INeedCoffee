using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public AudioSource shotAudio;
    public AudioSource reloadAudio;

    public bool isReloaded = true;
    
    public void Shot()
    {
        shotAudio.Play();

        isReloaded = false;
    }

    public void PlayReloadSound()
    {
        reloadAudio.Play();
    }

    public void Reload()
    {
        isReloaded = true;
    }
}
