using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScoreManager : MonoBehaviour
{
    private static int totalScore = 0;

    // Adds score to the total score
    public static void AddScore(int score)
    {
        totalScore += score;

        // Ensure that the total score does not go below 0
        if (totalScore < 0)
        {
            totalScore = 0;
        }
    }

    // Gets the current total score
    public static int GetTotalScore()
    {
        return totalScore;
    }

    // Method to reset the total score when play mode starts
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetTotalScore()
    {
        totalScore = 0;
    }
}
