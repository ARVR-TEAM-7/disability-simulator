using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCard : MonoBehaviour
{
    public TextMeshProUGUI score;

    public void SetScore(string scoreIn)
    {
        score.text = scoreIn;
    }
}
