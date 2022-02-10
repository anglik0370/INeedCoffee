using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}

    public TimerUI timerUI;

    private float timer;
    private float upgradeTimer;

    private bool isGameStart = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        isGameStart = false;
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

        timer = 0f;
        upgradeTimer = EnemyManager.UPGRADE_DELAY;
    }

    private void Update() 
    {
        if(!isGameStart) return;

        timer += Time.deltaTime;
        timerUI.SetTimerText(timer);

        upgradeTimer -= Time.deltaTime;

        if(upgradeTimer <= 0)
        {       
            EnemyManager.Instance.RandomEnemyUpgrade();
            upgradeTimer += EnemyManager.UPGRADE_DELAY;
        }
    }
}
