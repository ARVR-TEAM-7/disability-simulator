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

    [Header("Audio")]
    public AudioSource gameMusic;
    public AudioSource warningSound;
    public AudioSource endingSound;

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

    private List<int> leaderboardStats = new List<int>();

    private DisabilityManager disabilityManager;

    private bool timeWarningSounded = false;

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

        if (timerRunning && timeTillNewSession - sessionTimeNow < 3 && !timeWarningSounded)
        {
            StartCoroutine(PlayTimeWarning());
            timeWarningSounded = true;
        }

        score.text = scoreCount.ToString();
    }

    public void StartNewSession()
    {
        if (timerRunning)
            return;

        Debug.Log("New Session"); 

        gameMusic.PlayOneShot(gameMusic.clip, 0.75f);
        timeWarningSounded = false;
        scoreCount = 0;
        timeSinceLastSession = Time.realtimeSinceStartup;
        timeTillNewSession = sessionTimeInSeconds;
        SetTimerRunning(true);
        StartCoroutine(RandomDisabilities());
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
        disabilityManager.ApplyNormalVision();
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

    private IEnumerator PlayTimeWarning()
    {
        while (true)
        {
            warningSound.PlayOneShot(warningSound.clip, 0.75f);
            yield return new WaitForSeconds(1f);
            warningSound.PlayOneShot(warningSound.clip, 0.75f);
            yield return new WaitForSeconds(1f);
            warningSound.PlayOneShot(warningSound.clip, 0.75f);
            yield return new WaitForSeconds(1f);
            endingSound.PlayOneShot(endingSound.clip, 1f);
            yield break;
        }
    }

    private IEnumerator RandomDisabilities()
    {
        while (timerRunning)
        {
            disabilityManager.SetRandomDisability();

            for (int i = 0; i < switchDisabilitiesInSeconds; i++)
            {
                if (!timerRunning) {
                    yield break;
                }
                yield return new WaitForSeconds(1.1f); // offset so we don't get a new disability at the very end
            } 
        }
    }

}
