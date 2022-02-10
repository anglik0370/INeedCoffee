using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}

    public TimerUI timerUI;

    [SerializeField]
    private float timer;
    private float upgradeTimer;

    private bool isGameStart = false;
    private bool isPause = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        isGameStart = false;
        isPause = false;
    }

    private void Start() 
    {
        GameManager.Instance.SubGameStart(() => 
        {
            isGameStart = true;
            
            timer = 0f;
            timerUI.SetTimerText(timer);
        });

        GameManager.Instance.SubGameOver(() =>
        {
            isGameStart = false;
        });

        GameManager.Instance.SubPause(isPause => 
        {
            this.isPause = isPause;
        });

        timer = 0f;
        upgradeTimer = EnemyManager.UPGRADE_DELAY;
    }

    private void Update() 
    {
        if(!isGameStart || isPause) return;

        timer += Time.deltaTime;
        timerUI.SetTimerText(timer);

        GameManager.Instance.lifeTime = timer;

        upgradeTimer -= Time.deltaTime;

        if(upgradeTimer <= 0)
        {       
            EnemyManager.Instance.RandomEnemyUpgrade();
            upgradeTimer += EnemyManager.UPGRADE_DELAY;
        }
    }
}
