using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] private Text text = null;
    private float countdown = 3;
    private bool isCountdownStarted = false;

    private void Update()
    {
        if (isCountdownStarted)
        {
            countdown -= Time.deltaTime;
            int t = Mathf.CeilToInt(countdown);
            text.text = t > 0 ? t.ToString() : "GO!";
            if (t < 0)
                text.enabled = false;
        }
    }
    public void StartCountdown()
    {
        isCountdownStarted = true;

    }
}
