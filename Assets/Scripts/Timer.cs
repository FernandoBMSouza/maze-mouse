using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpTimer;

    private float startTime, timer;
    private string minutes, seconds;
    [SerializeField] private float maxTime;

    public float TimerValue
    {
        get => timer;
        set => timer = value;
    }

    public float CurrentTime { get => timer;}
    [SerializeField] private Pause pause;


    private void Start()
    {
        //startTime = maxTime;
        timer = maxTime;
    }

    private void Update()
    {
        //timer = startTime - Time.time;
        timer -= Time.deltaTime;

        if (timer > 0)
        {
            minutes = ((int)timer / 60).ToString("00");
            seconds = (timer % 60).ToString("00");
            tmpTimer.text = $"{minutes}:{seconds}";

            if (timer < maxTime / 2)
            {
                tmpTimer.color = Color.red;
            }
        }
        else
        {
            timer = 0;
        }

        if(timer <= 0)
        {
            pause.Loose();
        }
    }
}