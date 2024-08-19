using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Managers
{
    public class FeedbackManager : MonoBehaviour
    {
        public static FeedbackManager Instance { get; private set; }
        
        [SerializeField] private int initialScore = 100;
        private int _currentScore;
        private bool _isUpdatingScore = true;
        private List<string> _feedbackScores = new List<string>();
        
        private int _turnsToWin;
        private int _turnsCounter;

        void Awake()
        {
            // Singleton
            Instance = this;
        }
        private void Start()
        {
            _currentScore = initialScore;
        }
        
        public void SetIsUpdatingScore(bool isUpdatingScore)
        {
            _isUpdatingScore = isUpdatingScore;
        }

        public void IncreaseTurnsAmount()
        {
            _turnsCounter++;
            Debug.Log("turnsAmount: " + _turnsCounter);
            if (_turnsCounter >= 3) GameManager.Instance.GameFinished(true, _feedbackScores);
        }

        public void UpdateScore(string feedbackScore)
        {
            if (_isUpdatingScore)
            {
                if (!IsValidFeedbackScore(feedbackScore))
                {
                    Debug.LogError("Invalid feedback score: " + feedbackScore);
                    return;
                }
                _currentScore += FeedbackScore.Table[feedbackScore];
                Debug.Log("You lost " + -FeedbackScore.Table[feedbackScore] + " points! " + "Current score: " + _currentScore);
                _feedbackScores.Add(feedbackScore);
                CanvasDashboard.Instance.DisplayUpdateScore(feedbackScore);
        
                if (_currentScore <= 0)
                {
                    GameManager.Instance.GameFinished(false, _feedbackScores);
                }
            }
        }
        
        private bool IsValidFeedbackScore(string feedbackScore)
        {
            return FeedbackScore.Table.ContainsKey(feedbackScore);
        }
        
        public void SetTurnsToWin(int turnsToWin)
        {
            _turnsToWin = turnsToWin;
        }
    }
}


public static class FeedbackScore
{
    public static readonly Dictionary<string, int> Table = new Dictionary<string, int>
    {
        { "Car Hit", -100 },
        { "Pedestrian Hit", -100 },
        { "Red Light", -50 },
        { "No Entry Sign", -20 },
        { "Stop Sign", -20 },
        { "Speed Limit", -10 },
        { "Give Way", -10 },
        { "Give Way Pedestrian", -10 },
        { "Wrong Direction", -5 }
    };
}

public class PlayersScores
{
    public string playerName;
    public int score;
    
    public PlayersScores(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}


[System.Serializable]
public class ScoresObject
{
    [XmlElement("Name")]
    public string playerName;
    
    [XmlArray("FeedbackTables")]
    [XmlArrayItem("FeedbackTable")]
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
    [XmlArray("Table")]
    [XmlArrayItem("Score")]
    public List<string> _feedbackScores;

    public FeedbackTable()
    {
    }

    public FeedbackTable(List<string> _feedbackScores)
    {
        this._feedbackScores = _feedbackScores;
    }
}