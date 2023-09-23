using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeLeft;
    [SerializeField] private float extraTime;
    [SerializeField] private bool timerReachedZero;
    [SerializeField] private float timerSpeed;

    public static event EventHandler<bool> timerStatus;

    // Start is called before the first frame update
    void Start()
    {
        timerReachedZero = false;
        timeLeft = 10.0f;
        extraTime = 5.0f;

        timerStatus?.Invoke(this, timerReachedZero);
        StartCoroutine(TimeLeft());
    }

    // Get more time
    public void GetMoreTime()
    {
        timeLeft = timeLeft + extraTime;
    }

    // Reduce timer
    public void ReduceTime()
    {
        timeLeft = timeLeft - extraTime;
    }

    // Timer controller
    IEnumerator TimeLeft()
    {
        while (timeLeft >= 0.0f)
        {
            if (GameManager.GetPosInt() >= 3)
            {
                timerSpeed = 0.001f;
            }
            else if (GameManager.GetPosInt() < 3)
            {
                timerSpeed = 0.01f;
            }
            timerText.text = Math.Round(timeLeft, 2).ToString();
            yield return new WaitForSeconds(timerSpeed);
            timeLeft -= 0.01f;
        }
        timerText.text = "0";
        timerReachedZero = true;
        timerStatus?.Invoke(this, timerReachedZero);
        Debug.Log("Time reached zero!");
    }
}
