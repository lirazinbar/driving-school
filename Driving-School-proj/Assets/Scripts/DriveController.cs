using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveController : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public float forwardForce = 1000f;
    void Start()
    {
        
    }

    void Update()
    {
        // OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);

        // if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        // {
            // carRigidbody.velocity = Vector3.forward * 10;
            // (transform.forward * forwardForce * Time.deltaTime);
        // }

    }
}
