using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisabilityEvent : MonoBehaviour
{
    public static DisabilityEvent instance { get; private set; }

    [Header("Timers")]
    public float sessionTimeInSeconds = 30f;
    public float switchDisabilitiesInSeconds = 10f;

    [Header("Components")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI timer;

    private int scoreCount;

    private float sessionTimeNow = 0f;
    private float timeSinceLastSession = 0f;
    private float timeTillNewSession = 0f;

    private float disabilityTimeNow = 0f;
    private float timeSinceLastDisability = 0f;
    private float timeTillNewDisability = 0f;

    private DisabilityManager disabilityManager;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        disabilityManager = GetComponent<DisabilityManager>();
    }

    public void IncrementScore(int increment)
    {
        scoreCount += increment;
    }
    // Update is called once per frame
    void Update()
    {
        sessionTimeNow = Time.realtimeSinceStartup - timeSinceLastSession;

        if (sessionTimeNow > timeTillNewSession)
        {
            timeSinceLastSession = Time.realtimeSinceStartup;
            timeTillNewSession = sessionTimeInSeconds;
            StartNewSession();
        }

        disabilityTimeNow = Time.realtimeSinceStartup - timeSinceLastDisability;

        if (disabilityTimeNow > timeTillNewDisability)
        {
            timeSinceLastDisability = Time.realtimeSinceStartup;
            timeTillNewDisability = switchDisabilitiesInSeconds;
            disabilityManager.SetRandomDisability();
        }

        score.text = scoreCount.ToString();
        timer.text = string.Format("{0:0.0}", timeTillNewSession - sessionTimeNow);

    }

    void StartNewSession()
    {
        Debug.Log("New Session");
        scoreCount = 0;
    }

}
