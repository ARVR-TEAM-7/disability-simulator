using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabilityEvent : MonoBehaviour
{
    public float sessionTimeInSeconds = 30f;
    public float switchDisabilitiesInSeconds = 10f;

    private float sessionTimeNow = 0f;
    private float timeSinceLastSession = 0f;
    private float timeTillNewSession = 0f;

    private float disabilityTimeNow = 0f;
    private float timeSinceLastDisability = 0f;
    private float timeTillNewDisability = 0f;

    private DisabilityManager disabilityManager;
    void Awake()
    {
        disabilityManager = GetComponent<DisabilityManager>();
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
    }

    void StartNewSession()
    {
        Debug.Log("New Session");
    }

}
