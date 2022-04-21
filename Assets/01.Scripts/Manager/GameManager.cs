using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Player player;

    [Header("프리팹")]
    public MuzzleFlash muzzleFlashPrefab;
    public Catridge CatridgePreafab;
    public CatridgeDropSoundEffect catridgeDropSoundEffectPrefab;
    public PlayerHitSoundEffect playerHitSoundEffectPrefab;

    [Header("핸들러")]
    public LifeUIHandler lifeUIHandler;
    public KillCountUI killCountUI;
    public AleartUI aleartUI;
    public GameOverPanel gameOverPanel;
    public PausePanel pausePanel;
    public BulletUpgradeCards bulletUpgradeCards;

    [Header("캔버스")]
    public CanvasGroup mainCvs;
    public CanvasGroup inGameCvs;

    [Header("풀매니저")]
    public Transform poolManagerTrm;

    [Header("라이프")]
    public float maxLife;
    public float life;

    [Header("킬카운트")]
    public int killCount;

    [Header("생존시간")]
    public float lifeTime;

    [Header("점수")]
    public int score;
    public int highScore;

    [Header("디버그")]
    public bool isInvincibility = false;
    
    //이벤트
    private Action GameStartActon = () => {};
    private Action GameOverAction = () => {};

    private Action BackToMainAction = () => {};
    private Action<bool> GamePauseAction = isPause => {};

    private const string HIGHSCORE_KEY = "HighScore";

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<MuzzleFlash>(muzzleFlashPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<Catridge>(CatridgePreafab.gameObject, poolManagerTrm, 13);
        PoolManager.CreatePool<CatridgeDropSoundEffect>(catridgeDropSoundEffectPrefab.gameObject, poolManagerTrm, 10);
        PoolManager.CreatePool<PlayerHitSoundEffect>(playerHitSoundEffectPrefab.gameObject, poolManagerTrm, 3);

        if(!PlayerPrefs.HasKey(HIGHSCORE_KEY))
        {
            PlayerPrefs.SetInt(HIGHSCORE_KEY, 0);
        }

        highScore = PlayerPrefs.GetInt(HIGHSCORE_KEY);

        ChangeCvs(false);
    }

    private void Start() 
    {
        SubGameStart(() => 
        {
            ChangeCvs(true);

            life = maxLife;

            lifeUIHandler.ReFillLife();

            killCount = 0;
            killCountUI.UpdateCountText(killCount);

            player.gameObject.SetActive(true);
        });

        SubGameOver(() => 
        {
            player.gameObject.SetActive(false);
            player.transform.position = Vector3.zero;

            int score = Mathf.RoundToInt((lifeTime * 2) * killCount);

            if(score > highScore)
            {
                PlayerPrefs.SetInt(HIGHSCORE_KEY, score);
                highScore = score;
            }

            gameOverPanel.Open(killCount, Mathf.RoundToInt(lifeTime), highScore, score);
        });

        pausePanel.SubPauseOnClick(() => 
        {
            GamePauseAction?.Invoke(true);
        });

        pausePanel.SubContunueOnClick(() => 
        {
            GamePauseAction?.Invoke(false);
        });
        
        pausePanel.SubBackToMainOnClick(() => 
        {
            GamePauseAction?.Invoke(false);
            BackToMainAction?.Invoke();

            ChangeCvs(false);
        });

        life = maxLife;
        lifeUIHandler.ReFillLife();

        killCount = 0;
        killCountUI.UpdateCountText(killCount);
    }

    public static Player GetPlayer()
    {
        return Instance.player;
    }

    public static void Aleart(string msg)
    {
        Instance.aleartUI.Aleart(msg);
    }

    public void ChangeCvs(bool isNowMain)
    {
        if(isNowMain)
        {
            mainCvs.alpha = 0f;
            mainCvs.interactable = false;
            mainCvs.blocksRaycasts = false;

            inGameCvs.alpha = 1f;
            inGameCvs.interactable = true;
            inGameCvs.blocksRaycasts = true;
        }
        else
        {
            mainCvs.alpha = 1f;
            mainCvs.interactable = true;
            mainCvs.blocksRaycasts = true;

            inGameCvs.alpha = 0f;
            inGameCvs.interactable = false;
            inGameCvs.blocksRaycasts = false;
        }
    }

    public void GameStart()
    {
        GameStartActon?.Invoke();
    }

    public void BackToMain()
    {
        BackToMainAction?.Invoke();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void SubGameStart(Action Callback)
    {
        GameStartActon += Callback;
    }

    public void SubGameOver(Action Callback)
    {
        GameOverAction += Callback;
    }

    public void SubBackToMain(Action Callback)
    {
        BackToMainAction += Callback;
    }

    public void SubPause(Action<bool> Callback)
    {
        GamePauseAction += Callback;
    }

    public void OccurPause(bool pause)
    {
        GamePauseAction?.Invoke(pause);
    }

    public void PlayerDamaged()
    {
        if(isInvincibility) return;

        if(life - 1 == 0)
        {
            GameOverAction?.Invoke();
        }

        life--;
        lifeUIHandler.RemoveLife();
    }

    public void EnemyDead()
    {
        killCount++;

        //if(killCount % 10 == 0)
        //{
        //    bulletUpgradeCards.StartUpgrade();
        //}

        killCountUI.UpdateCountText(killCount);
    }
}