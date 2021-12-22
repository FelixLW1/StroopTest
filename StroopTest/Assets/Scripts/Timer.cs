using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTimerText;
    [SerializeField] private TextMeshProUGUI roundTimerText;

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
}
