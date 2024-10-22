using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public static int scoreCount;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the score to zero when the game starts
        scoreCount = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + Mathf.Round(scoreCount);
    }
}
