using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    [SerializeField] private TMP_Text value;        
    [SerializeField] private Slider slider;
    [SerializeField] private bool isNumberValue;
    
    public void SetText()
    {
        if (isNumberValue)
        {
            value.SetText((int)slider.value + "");
        }
        else
        {
            switch (slider.value)
            {
                case 1:
                    value.SetText("None");
                    break;
                case 2:
                    value.SetText("Easy");
                    break;
                case 3:
                    value.SetText("Medium");
                    break;
                default:
                    value.SetText("Hard");
                    break; 
            }
        }
    }
}
