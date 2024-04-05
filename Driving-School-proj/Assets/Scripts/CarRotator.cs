using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotator : MonoBehaviour
{
    private Quaternion initialRotation;
    public GameObject Car;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    // void Update()
    // {
    //     Debug.Log("rotation: " + transform.rotation.eulerAngles);
        // // Calculate the rotation difference
        // Quaternion rotationDifference = Quaternion.Inverse(initialRotation) * transform.rotation;
        //
        // // Extract the rotation angle around the Y-axis
        // float yRotation = rotationDifference.eulerAngles.y;
        //
        // // Apply the Y rotation to the car's rotation
        // Vector3 currentRotation = Car.transform.rotation.eulerAngles;
        // Car.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + yRotation, currentRotation.z);
    // }
}
