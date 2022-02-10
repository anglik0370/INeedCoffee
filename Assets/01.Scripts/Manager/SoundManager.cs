using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}

    [SerializeField]
    private AudioSource mainBGM;
    [SerializeField]
    private AudioSource inGameBGM;
    [SerializeField]
    private AudioSource gameOverBGM;
    [SerializeField]
    private AudioSource playerMoveSound;
    [SerializeField]
    private AudioSource enemyLevelUPSoundEffect;

    [SerializeField]
    private ButtonClickSoundEffect btnClikcSoundPrefab;

    [SerializeField]
    private Transform poolManagerTrm;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<ButtonClickSoundEffect>(btnClikcSoundPrefab.gameObject, poolManagerTrm, 3);

        mainBGM.Play();
    }

    private void Start() 
    {
        GameManager.Instance.SubGameStart(() => 
        {
            mainBGM.Stop();
            inGameBGM.Play();
        });

        GameManager.Instance.SubGameOver(() => 
        {
            inGameBGM.Stop();
            gameOverBGM.Play();

            playerMoveSound.Stop();
        });

        GameManager.Instance.SubBackToMain(() => 
        {
            gameOverBGM.Stop();
            inGameBGM.Stop();
            mainBGM.Play();
        });

        GameManager.Instance.SubPause(isPause => 
        {
            if(isPause)
            {
                inGameBGM.Pause();
            }
            else
            {
                inGameBGM.UnPause();
            }
        });

        EnemyManager.Instance.SubUpgradeHealth((max, increment) => 
        {
            enemyLevelUPSoundEffect.Play();
        });

        EnemyManager.Instance.SubUpgradeMoveSpeed(moveSpeed => 
        {
            enemyLevelUPSoundEffect.Play();
        });

        EnemyManager.Instance.SubUpgradeSpawnDelay(spawnDelay => 
        {
            enemyLevelUPSoundEffect.Play();
        });
    }

    public void PlayPlayerMoveSound(bool isMoving)
    {
        if(isMoving)
        {
            playerMoveSound.Play();
        }
        else
        {
            playerMoveSound.Stop();
        }
    }

    public void PlayBtnClickSound()
    {
        ButtonClickSoundEffect soundEffect = PoolManager.GetItem<ButtonClickSoundEffect>();
        soundEffect.Play();
    }
}
