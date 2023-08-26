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

    public float CurrentTime { get => timer; }

    private void Start()
    {
        startTime = maxTime;
    }

    private void Update()
    {
        timer = startTime - Time.time;

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
    }
}