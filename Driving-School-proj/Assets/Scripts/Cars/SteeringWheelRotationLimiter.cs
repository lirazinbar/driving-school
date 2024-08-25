using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class SteeringWheelRotationLimiter : MonoBehaviour
{
    public float minRotation = -60f; // Minimum rotation limit
    public float maxRotation = 60f;  // Maximum rotation limit
    public float returnSpeed = 1.5f;   // Speed at which the wheel returns to center when not grabbed

    public Grabbable grabbable;

    private float currentAngle = 0f;

    void Update()
    {
        // Get the current Z-axis rotation
        currentAngle = transform.eulerAngles.z + 180;
        
        // Normalize the angle to be within -180 to 180 degrees
        if (currentAngle > 180) currentAngle -= 360;

        // Clamp the rotation
        currentAngle = Mathf.Clamp(currentAngle, minRotation, maxRotation);

        // Apply the clamped rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentAngle+180);
        
        if (grabbable.SelectingPointsCount == 0)
        {
            // Gradually rotate back to 0 degrees
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y,
                Mathf.LerpAngle(transform.localEulerAngles.z, 180, Time.deltaTime * returnSpeed));
        }
    }
}
