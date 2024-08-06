using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using TMPro;
using UnityEngine;

public class PlayModeMenu : MonoBehaviour
{
    [SerializeField] private Canvas playModeMenuCanvas;     
    [SerializeField] private Canvas chooseRouteMenuCanvas;     
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas highScoreCanvas;
    [SerializeField] private GameObject scoreComponentPrefab;
    [SerializeField] private GameObject gridContainerGameOverMenu;
    
    public void onGetBackToMainMenu()
    {
        playModeMenuCanvas.gameObject.SetActive(false);
        highScoreCanvas.gameObject.SetActive(false);
        chooseRouteMenuCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
    }

    public int calcSumOfScoresInTable(List<FeedbackScore> _feedbackScores)
    {
        int sumScores = 100;
        for (int index = 0; index < _feedbackScores.Count; index++)
        {
            FeedbackScore score = _feedbackScores[index];
            sumScores += (int)score;
        }

        return sumScores;
    }
    
    public void onGoToHighScore()
    {
        playModeMenuCanvas.gameObject.SetActive(false);

        foreach (Transform child in gridContainerGameOverMenu.transform)
        {
            Destroy(child.gameObject);
        }

        List<ScoresObject> scoresCollection = XMLManager.Instance.LoadScores();
        Debug.Log("scoresCollection: " + scoresCollection.Count);
        List<PlayersScores> players = new List<PlayersScores>();

        for (int scoreObjIndex = 0; scoreObjIndex < scoresCollection.Count; scoreObjIndex++)
        {
            // for each player - run on his tables scores
            ScoresObject scoresObject = scoresCollection[scoreObjIndex];
            int maxScoreOfPlayer = 0;
            for (int playerTableIndex = 0; playerTableIndex < scoresObject._feedbackTables.Count; playerTableIndex++)
            {
                int currentTableScore = calcSumOfScoresInTable(scoresObject._feedbackTables[playerTableIndex]._feedbackScores);
                if (currentTableScore > maxScoreOfPlayer)
                {
                    maxScoreOfPlayer = currentTableScore;
                }
            }
            Debug.Log(scoresObject.playerName + ": " + maxScoreOfPlayer.ToString());
            players.Add(new PlayersScores(scoresObject.playerName, maxScoreOfPlayer));
        }
        
        List<PlayersScores> topPlayers = players.OrderByDescending(p => p.score).ToList();
        // List<PlayersScores> topPlayers = players.OrderByDescending(p => p.score).Take(3).ToList();

        for (int topPlayerIndex = 0; topPlayerIndex < topPlayers.Count; topPlayerIndex++)
        {
            GameObject newComponent = Instantiate(scoreComponentPrefab, gridContainerGameOverMenu.transform);
                
            newComponent.transform.GetComponent<TextMeshProUGUI>().text = topPlayers[topPlayerIndex].playerName + ":    " + topPlayers[topPlayerIndex].score.ToString();
                
            newComponent.name = "ScoreLine" + (topPlayerIndex+1);
        }
        
        highScoreCanvas.gameObject.SetActive(true);
    }
    
    
}
