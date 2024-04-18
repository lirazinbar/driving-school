using System;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }
    [SerializeField] private int initialScore = 100;
    private int _currentScore;
    private List<FeedbackScore> _feedbackScores = new List<FeedbackScore>();

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _currentScore = initialScore;
    }

    public void UpdateScore(FeedbackScore feedbackScore)
    {
        _currentScore += (int) feedbackScore;
        Debug.Log("You lost " + (int) feedbackScore + " points!");
        Debug.Log("Current score: " + _currentScore);
        // Update UI
    }
}

public enum FeedbackScore
{
    NoEntry = -10,
    RedLight = -20,
    StopSign = -30
    // Add more feedback scores here
    // Example: Speeding = -10
    // Example: WrongLane = -20
    // Example: Collision = -30
    // Example: WrongDirection = -40
    // Example: OffRoad = -50
}
