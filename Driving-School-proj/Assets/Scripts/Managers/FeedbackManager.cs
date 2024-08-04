using System.Collections.Generic;
using System.Xml.Serialization;
using Managers;
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
            _feedbackScores.Add(feedbackScore);
            CanvasDashboard.Instance.DisplayUpdateScore(feedbackScore);
        
            if (_currentScore <= 0)
            {
                GameManager.Instance.GameFinished(false, _feedbackScores);
            }
        }
    }

    public enum FeedbackScore
    {
        NoEntry = -20,
        RedLight = -100,
        StopSign = -20,
        Speeding = -10,
        GiveWay = -10
        // WrongLane = -20
        // OffRoad = -50
        // WrongDirection = -40
        // Collision = -30
    }
}


[System.Serializable]
public class ScoresObject
{
    [XmlElement("Name")]
    public string playerName;
    
    [XmlArray("FeedbackScores")]
    [XmlArrayItem("Score")]
    public List<FeedbackTable> _feedbackTables;

    public ScoresObject()
    {
        this.playerName = "";
    }

    public ScoresObject(string playerName, List<FeedbackTable> _feedbackTables)
    {
        this.playerName = playerName;
        this._feedbackTables = _feedbackTables;
    }
}

[System.Serializable]
public class FeedbackTable
{
    [XmlArray("FeedbackTable")]
    [XmlArrayItem("Score")]
    public List<FeedbackScore> _feedbackScores;

    public FeedbackTable()
    {
    }

    public FeedbackTable(List<FeedbackScore> _feedbackScores)
    {
        this._feedbackScores = _feedbackScores;
    }
}