using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class FeedbackManager : MonoBehaviour
    {
        public static FeedbackManager Instance { get; private set; }
        [SerializeField] private int initialScore = 100;
        private int _currentScore;
        private List<FeedbackScore> _feedbackScores = new List<FeedbackScore>();

        void Awake()
        {
            // Singleton
            Instance = this;
        }
        private void Start()
        {
            _currentScore = initialScore;
        }

        public void UpdateScore(FeedbackScore feedbackScore)
        {
            _currentScore += (int) feedbackScore;
            Debug.Log("You lost " + -(int) feedbackScore + " points!");
            Debug.Log("Current score: " + _currentScore);
            // Update UI
        
            if (_currentScore <= 0)
            {
                Debug.Log("Game Over! You failed the test!");
                // Time.timeScale = 0; // Pause the game
                // Update UI: Show a board of the scores and the feedbacks and an exit button
            }
        }
    }

    public enum FeedbackScore
    {
        NoEntry = -10,
        RedLight = -20,
        StopSign = -30,
        Speeding = -10
        // WrongLane = -20
        // OffRoad = -50
        // WrongDirection = -40
        // Collision = -30
    }
}