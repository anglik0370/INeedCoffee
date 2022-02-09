using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadSoundEffect : MonoBehaviour
{
    AudioSource soundEffect;

    private void Awake() 
    {
        soundEffect = GetComponent<AudioSource>();
    }

    public void Play()
    {
        soundEffect.Play();
        StartCoroutine(DisableRoutine());
    } 

    private IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(soundEffect.clip.length);

        gameObject.SetActive(false);
    }
}
