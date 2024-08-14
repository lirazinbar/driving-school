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
    public CrossSectionDirections selectedDirection = CrossSectionDirections.Forward;
    public bool wasJunctionChecked = true;

    void Start()
    {
        wasJunctionChecked = true;
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
           selectedDirection = optionalDirectionsArray[rnd.Next(optionalDirectionsArray.Count)];
           Debug.Log("setToFalse");
           wasJunctionChecked = false;
           EventsManager.Instance.TriggerCarEnteredCrossSectionEvent(selectedDirection);
        }
    }
    
}
