using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDashboard : MonoBehaviour
{
    private CarController _carController;
    public TMP_Text gearState; 
    public TMP_Text turnDirection; 

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
        Debug.Log("Car entered cross section ");
        switch (newDirection)
        {
            case CrossSectionDirections.Right:
                turnDirection.SetText("right");
                break;
            case CrossSectionDirections.Forward:
                turnDirection.SetText( "forward");
                break;
            case CrossSectionDirections.Left:
                turnDirection.SetText("left");
                break;
            default:
                turnDirection.SetText("N/A"); // Not applicable or unknown state
                break;
        }
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
