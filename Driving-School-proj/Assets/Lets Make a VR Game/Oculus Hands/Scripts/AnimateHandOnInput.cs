using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator handAnimator;
    [SerializeField] private bool isRightHand;


    // Update is called once per frame
    void Update()
    {
        // float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        // float gripValue = gripAnimationAction.action.ReadValue<float>();

        float gripValue;
        float triggerValue;
        
        if (isRightHand)
        {
            triggerValue = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
            gripValue = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
        }
        else
        {
            triggerValue = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
            gripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
        }

        handAnimator.SetFloat("Trigger", triggerValue);

        handAnimator.SetFloat("Grip", gripValue);
    }
}
