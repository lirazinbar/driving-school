using UnityEngine;

public class StreetLight : MonoBehaviour
{
    [SerializeField] private Light streetLight;
    [SerializeField] private Light lampLight;
    
    public void TurnOn()
    {
        streetLight.enabled = true;
        lampLight.enabled = true;
    }
    
    public void TurnOff()
    {
        streetLight.enabled = false;
        lampLight.enabled = false;
    }
}
