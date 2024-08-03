using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private GameObject slot;
    [SerializeField] private Sprite regular;
    [SerializeField] private Sprite stopSign;
    [SerializeField] private Sprite trafficLight;
    public void OnValueChangedToNull(bool? newVal)
    {
        Debug.Log("new value: " + newVal);
        Image imageComponent = slot.transform.GetChild(0).GetComponent<Image>();
         
        if (newVal == null)
        {
            imageComponent.sprite = regular;
        } else if (newVal == true)
        {
            imageComponent.sprite = stopSign;
        }
        else
        {
            imageComponent.sprite = trafficLight;
        }
    }
}
