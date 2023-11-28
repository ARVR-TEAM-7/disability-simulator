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

    public GameObject inSession;
    public GameObject outSession;

    public GameObject leaderboard;

    [Header("Prefabs")]
    public GameObject scoreCardPrefab;

    private int scoreCount;
    private bool timerRunning;

    private float sessionTimeNow = 0f;
    private float timeSinceLastSession = 0f;
    private float timeTillNewSession = 0f;

    private float disabilityTimeNow = 0f;
    private float timeSinceLastDisability = 0f;
    private float timeTillNewDisability = 0f;

    private List<int> leaderboardStats = new List<int>();

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
        if (timerRunning)
        {
            scoreCount += increment;
        }
    }
    // Update is called once per frame
    void Update()
    {
        sessionTimeNow = Time.realtimeSinceStartup - timeSinceLastSession;

        if (sessionTimeNow > timeTillNewSession)
        {
            SessionEnded();
        }
        else
        {
            timer.text = string.Format("{0:0.0}", timeTillNewSession - sessionTimeNow);
        }

        disabilityTimeNow = Time.realtimeSinceStartup - timeSinceLastDisability;

        if (disabilityTimeNow > timeTillNewDisability)
        {
            timeSinceLastDisability = Time.realtimeSinceStartup;
            timeTillNewDisability = switchDisabilitiesInSeconds;
            disabilityManager.SetRandomDisability();
        }

        score.text = scoreCount.ToString();
    }

    public void StartNewSession()
    {
        if (timerRunning)
            return;

        Debug.Log("New Session");
        scoreCount = 0;
        timeSinceLastSession = Time.realtimeSinceStartup;
        timeTillNewSession = sessionTimeInSeconds;
        SetTimerRunning(true);
    }

    private void SessionEnded()
    {
        if (!timerRunning)
            return;

        SetTimerRunning(false);
        timer.text = "0";
        leaderboardStats.Add(scoreCount);
        leaderboardStats.Sort();
        leaderboardStats.Reverse();
        RefreshLeaderboardUI();
    }

    private void RefreshLeaderboardUI()
    {
        foreach (Transform child in leaderboard.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (int score in leaderboardStats)
        {
            GameObject scoreCard = Instantiate(scoreCardPrefab, leaderboard.transform);
            scoreCard.GetComponent<ScoreCard>().SetScore(score.ToString());
        }
    }


    private void SetTimerRunning(bool isRunning)
    {
        timerRunning = isRunning;
        inSession.SetActive(timerRunning);
        outSession.SetActive(!timerRunning);
    }

    public bool isTimerRunning()
    {
        return timerRunning;
    }

}
