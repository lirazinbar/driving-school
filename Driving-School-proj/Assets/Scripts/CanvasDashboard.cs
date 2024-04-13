using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDashboard : MonoBehaviour
{
    private CarController _carController;
    public TMP_Text gearState; 

    void Start()
    {
        gearState.SetText("P");

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
