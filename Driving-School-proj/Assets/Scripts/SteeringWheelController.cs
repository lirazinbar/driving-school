// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SteeringWheelController : MonoBehaviour
// {
//     private OVRGrabbable grabbable;
//     private Vector3 initialLocalRotation;
//     public float rotateSpeed = 5f;
    
//     private void Awake()
//     {
//         // get the grabbable component on this object
//         grabbable = GetComponent<OVRGrabbable>();
//     }
 
//     // Unity function called once when the game starts
//     private void Start()
//     {
//         initialLocalRotation = transform.localRotation.eulerAngles;
//     }

//     private void FixedUpdate()
//     {
//         if (grabbable && !grabbable.isGrabbed)
//             OnReleased();
//     }


//     public void OnReleased()
//     {
//         Quaternion targetRotation = Quaternion.LookRotation(initialLocalRotation, transform.up);

//         // Smoothly rotate the wheel back to center
//         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
//     }
 
    
    
// }
