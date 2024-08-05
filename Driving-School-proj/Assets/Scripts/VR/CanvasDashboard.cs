using System.Collections.Generic;
using Cars;
using Enums;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDashboard : MonoBehaviour
{
    public static CanvasDashboard Instance { get; private set; }

    private CarController _carController;
    public TMP_Text gearState; 
    [SerializeField] TMP_Text score; 
    public Image turnDirection;
    
    public Sprite leftTurnImage;
    public Sprite rightTurnImage;
    public Sprite forwardTurnImage;

    void Awake()
    {
        // Singleton
        Instance = this;
    }

    void Start()
    {
        gearState.SetText("P");
        EventsManager.Instance.carGearStateChangedEvent.AddListener(OnCarGearStateChangedEvent);
        EventsManager.Instance.carEnteredCrossSectionEvent.AddListener(OnCarEnteredCrossSectionEvent);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe the events triggered by each stop sign
        EventsManager.Instance.carGearStateChangedEvent.RemoveListener(OnCarGearStateChangedEvent);
        EventsManager.Instance.carEnteredCrossSectionEvent.RemoveListener(OnCarEnteredCrossSectionEvent);
    }
    
    private void OnCarGearStateChangedEvent(GearState currentGearState)
    {
        Debug.Log("Car changed gear state ");
        switch (currentGearState)
        {
            case GearState.Park:
                gearState.SetText("P");
                break;
            case GearState.Drive:
                gearState.SetText( "D");
                break;
            case GearState.Reverse:
                gearState.SetText("R");
                break;
            default:
                gearState.SetText("N/A"); // Not applicable or unknown state
                break;
        }
    }

    private void OnCarEnteredCrossSectionEvent(CrossSectionDirections newDirection)
    {
        turnDirection.enabled = true;
        Debug.Log("Car entered cross section ");
        switch (newDirection)
        {
            case CrossSectionDirections.Right:
                turnDirection.sprite = rightTurnImage;
                break;
            case CrossSectionDirections.Forward:
                turnDirection.sprite = forwardTurnImage;
                break;
            case CrossSectionDirections.Left:
                turnDirection.sprite = leftTurnImage;
                break;
            default:
                turnDirection.sprite = null; // Not applicable or unknown direction
                break;
        }
        
        StartCoroutine(CleanDirectionAfterDelay());
        
        // turnDirection.enabled = true;

        // float displayStartTime = Time.time;

        // Wait for 5 seconds
        // while (Time.time - displayStartTime < 5f)
        // {
        //     
        // }
        
        // Hide the image after 5 seconds
        // turnDirection.enabled = false;
    }

    public void DisplayUpdateScore(FeedbackScore feedbackScore)
    {
        score.SetText(feedbackScore.ToString() + " " + (int)feedbackScore);
        StartCoroutine(CleanScoreAfterDelay());
    }
    
    private IEnumerator<WaitForSeconds> CleanScoreAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        score.SetText("");
    }
    
    private IEnumerator<WaitForSeconds> CleanDirectionAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        turnDirection.enabled = false;
    }

    /* void FixedUpdate()
    {
        GearState currentGearState = _carController.GetGearState();
        switch (currentGearState)
        {
            case GearState.Park:
                gearState.SetText("P");
                break;
            case GearState.Drive:
                gearState.SetText( "D");
                break;
            case GearState.Reverse:
                gearState.SetText("R");
                break;
            default:
                gearState.SetText("N/A"); // Not applicable or unknown state
                break;
        }
    } */
}