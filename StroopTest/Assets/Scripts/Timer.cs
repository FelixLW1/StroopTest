using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTimerText;
    [SerializeField] private TextMeshProUGUI roundTimerText;

    [HideInInspector] public float calculatedScore;

    public static event Action RoundTimerFinished;

    // The 3, 2, 1 Countdown when the game starts
    public async Task StartGameCountdown(int waitTime = 3)
    {
        gameTimerText.text = waitTime.ToString();

        while (waitTime > 0)
        {
            await Task.Delay(1000);
            
            waitTime--;
            
            gameTimerText.text = waitTime.ToString();
        }
    }

    // The countdown for each answer in a round
    public IEnumerator StartRoundCountdown(int seconds)
    {
        StartCoroutine(CountdownScore(seconds));
        
        roundTimerText.text = seconds.ToString();

        while (seconds > 0)
        {
            yield return new WaitForSecondsRealtime(1f);

            seconds --;

            roundTimerText.text = seconds.ToString();
        }
        
        RoundTimerFinished?.Invoke();
    }

    // Counts down each millisecond to give a bonus score
    private IEnumerator CountdownScore(int seconds)
    {
        calculatedScore = seconds * 1000;

        while (true)
        {
            yield return new WaitForEndOfFrame();

            calculatedScore -= Time.deltaTime * 1000;
        }
    }
}
