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
            Debug.Log("You lost " + -(int) feedbackScore + " points! " + "Current score: " + _currentScore);
            _feedbackScores.Add(feedbackScore);
            // Update UI
        
            if (_currentScore <= 0)
            {
                GameManager.Instance.GameFinished(false);
            }
        }
    }

    public enum FeedbackScore
    {
        NoEntry = -20,
        RedLight = -40,
        StopSign = -20,
        Speeding = -10,
        GiveWay = -10,
        GiveWayPedestrian = -10
        // WrongLane = -20
        // OffRoad = -50
        // WrongDirection = -40
        // Collision = -30
    }
}