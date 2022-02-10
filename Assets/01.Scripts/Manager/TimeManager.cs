using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance {get; private set;}

    public TimerUI timerUI;

    private float timer;

    private bool isGameStart = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start() 
    {
        timer = 0f;

        isGameStart = true;

        GameManager.Instance.SubGameOver(() =>
        {
            isGameStart = false;
        });
    }

    private void Update() 
    {
        if(!isGameStart) return;

        timer += Time.deltaTime;
        timerUI.SetTimerText(timer);
    }
}
