using System.Collections;
using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;

public class CrossSectionDitectorController : MonoBehaviour
{
    System.Random rnd = new System.Random();
    [SerializeField] private bool isRightTurnOptional = true;
    [SerializeField] private bool isLeftTurnOptional = true;
    [SerializeField] private bool isForwardTurnOptional = true;
    private List<CrossSectionDirections> optionalDirectionsArray = new List<CrossSectionDirections>();

    void Start()
    {
        if (isRightTurnOptional)
        {
            optionalDirectionsArray.Add(CrossSectionDirections.Right);
        }
        
        if (isLeftTurnOptional)
        {
            optionalDirectionsArray.Add(CrossSectionDirections.Left);
        }
        
        if (isForwardTurnOptional)
        {
            optionalDirectionsArray.Add(CrossSectionDirections.Forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("enter junc");
        if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
        {
           CrossSectionDirections selectedDirection = optionalDirectionsArray[rnd.Next(optionalDirectionsArray.Count)];
           EventsManager.Instance.TriggerCarEnteredCrossSectionEvent(selectedDirection);
        }
    }
    
}
