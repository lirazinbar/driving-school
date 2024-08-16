using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;

public class CrossSectionChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("enter junc");
        if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
        {
            Transform junctionDetector = transform.parent.GetChild(0);
            Debug.Log("wasJunctionChecked: " + junctionDetector.GetComponent<CrossSectionDitectorController>().wasJunctionChecked);
            if (junctionDetector.GetComponent<CrossSectionDitectorController>().wasJunctionChecked) return;

            CrossSectionDirections selectedDirection =
                junctionDetector.GetComponent<CrossSectionDitectorController>().selectedDirection; 
            
            CrossCheckersDirections checkerDirection;

            Enum.TryParse(transform.name, out checkerDirection);
            
            Debug.Log("selectedDirection: " + selectedDirection);
            Debug.Log("checkerDirection: " + checkerDirection);

            if ((int)selectedDirection != (int)checkerDirection) // minus points
            {
                EventsManager.Instance.TriggerCarTookWrongTurnEvent();
            }
            else 
            { // add points
                FeedbackManager.Instance.increaseTurnsAmount();
            }
            junctionDetector.GetComponent<CrossSectionDitectorController>().wasJunctionChecked = true;
        }
    }
}
