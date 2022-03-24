using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Accelerometer : MonoBehaviour
{
    public float shakeDetectionThreshold;
    public float minShakeInterval;

    private float sqrSDT;
    private float timeSinceLastShake;
    public GameHandler gameHandler;

    // Start is called before the first frame update
    void Start()
    {
        sqrSDT = Mathf.Pow(shakeDetectionThreshold, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //shake
        if (Input.acceleration.sqrMagnitude >= sqrSDT && Time.unscaledTime >= timeSinceLastShake + minShakeInterval)
        {
            Debug.Log("shake");
            Time.timeScale = 1;
            timeSinceLastShake = Time.unscaledTime;
            if(gameHandler.canUseShake == true)
            {
                gameHandler.ClearHistoryNotes();
                gameHandler.canUseShake = false;
            }

        }
    }

    
}
