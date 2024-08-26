using System;
using System.Collections;
using System.Collections.Generic;
using Cars;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class SteeringWheelRotationLimiter : MonoBehaviour
{
    public float returnSpeed = 50f;   // Speed at which the wheel returns to center when not grabbed

    public Grabbable grabbable;
    
    private float maxRotation = 530; // Max steering wheel rotation in either direction
    private float minRotation = -530; // Min steering wheel rotation in either direction

    private float cumulativeRotation = 0f; // Tracks the total rotation
    private float lastZAngle = 0f; // Tracks the last Z rotation angle
    public float carRatio = 6f; // Ratio of steering wheel rotation to wheel turn
    private const float initialOffset = 180; // Initial offset for the starting position
    private float _currentSteerAngle;
    
    
    private void Start()
    {
        lastZAngle = transform.localEulerAngles.z - initialOffset;
    }

    void Update()
    {
        // if the wheel is not grabbed
        if (grabbable.SelectingPointsCount == 0)
        {
            // Gradually rotate back to 0 degrees
            cumulativeRotation = Mathf.MoveTowards(cumulativeRotation, 0f, returnSpeed * Time.deltaTime);
        }
        else
        {

            // Get the current Z rotation
            float currentZAngle = transform.localEulerAngles.z - initialOffset;

            // Normalize the Z angle to be within -180 to 180 degrees
            if (currentZAngle > 180f)
                currentZAngle -= 360f;

            // Calculate the difference in rotation since the last frame
            float deltaRotation = currentZAngle - lastZAngle;

            // Handle the wrapping around the 180/-180 boundary
            if (deltaRotation > 180f)
                deltaRotation -= 360f;
            else if (deltaRotation < -180f)
                deltaRotation += 360f;

            // Update the cumulative rotation
            cumulativeRotation += deltaRotation;

            // Directly clamp the cumulative rotation to the limits
            if (cumulativeRotation > maxRotation)
            {
                cumulativeRotation = maxRotation;
            }
            else if (cumulativeRotation < minRotation)
            {
                cumulativeRotation = minRotation;
            }

            // Update the last Z angle for the next frame
            lastZAngle = cumulativeRotation;

            // Debug.Log("cumulativeRotation_2: " + cumulativeRotation);

        }

        float clampedRotation = cumulativeRotation + initialOffset;
        transform.localEulerAngles =
            new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, clampedRotation);

        _currentSteerAngle = cumulativeRotation / carRatio;

        CarController.setCurrentSteerAngle(_currentSteerAngle);
    }
}
