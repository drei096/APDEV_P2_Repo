using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public GameHandler gameHandler;
    public Image timerBar;
    public GameObject tapButton;

    // Update is called once per frame
    void Update()
    {
        timerBar.fillAmount = 1 - (gameHandler.CurrentTime / gameHandler.MaxTime);

        if(timerBar.fillAmount < 0.25f)
        {
            timerBar.color = Color.green;
            tapButton.SetActive(true);
        }
        else if(timerBar.fillAmount < 0.50f)
        {
            timerBar.color = Color.yellow;
        }
        else
        {
            timerBar.color = Color.red;
            tapButton.SetActive(false);
        }
    }
}
