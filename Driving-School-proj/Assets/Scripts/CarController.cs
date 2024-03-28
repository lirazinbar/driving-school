using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorByWheel : MonoBehaviour
{
    public GameObject Wheel;
    private float initialWheelRotationZ; // Initial rotation around Z-axis
    public float speed = 10f;
    private Quaternion initialRotation;

    void Start()
    {
        // initialWheelRotationZ = Wheel.transform.rotation.eulerAngles.z;
        initialRotation = Wheel.transform.rotation;
    }

    void Update()
    {
        // Get the wheel rotation around the Z-axis
        // float wheelRotationZ = Wheel.transform.rotation.eulerAngles.z - initialWheelRotationZ;
        
        
        
        // transform.Rotate(transform.rotation.x, wheelRotationZ, transform.rotation.z);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        
        // TODO -  here
        // Calculate the rotation difference
        Quaternion rotationDifference = Quaternion.Inverse(initialRotation) * Wheel.transform.rotation;

        // Extract the rotation angle around the Z-axis
        float zRotation = rotationDifference.eulerAngles.z;

        // Apply the Y rotation to the car's rotation
        Vector3 currentRotation = transform.rotation.eulerAngles;
        // transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + zRotation, currentRotation.z);
        transform.Rotate(transform.rotation.x, rotationDifference.eulerAngles.z, transform.rotation.z);
    }
}
