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
            Debug.Log("You lost " + -(int) feedbackScore + " points! " + "Current score: " + _currentScore);
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
        CarHit = -100,
        PedestrianHit = CarHit,

        RedLight = -50,
        
        NoEntry = -20,
        StopSign = NoEntry,

        Speeding = -10,
        GiveWay = Speeding,
        GiveWayPedestrian = Speeding
        // OffRoad = -50
        // WrongDirection = -40
    }
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
    public List<FeedbackScore> _feedbackScores;

    public FeedbackTable()
    {
    }

    public FeedbackTable(List<FeedbackScore> _feedbackScores)
    {
        this._feedbackScores = _feedbackScores;
    }
}