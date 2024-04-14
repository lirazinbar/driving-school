using UnityEngine;

public class CarController : MonoBehaviour
{
    private float _horizontalInput, _verticalInput;
    private float _currentSteerAngle, _currentBreakForce;
    private bool _isBreaking;
    private GearState _currentGearState = GearState.Drive;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private GameObject SteeringWheel;
    
    [SerializeField] private GameObject GearStick;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle, acceleration;
    [SerializeField] private bool isAutonomous;
    private float _currentMaxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // private void Update()
    // {
    //     if (isAutonomous) return;
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         ChangeGear(GearState.Park);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.G))
    //     {
    //         ChangeGear(GearState.Drive);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         ChangeGear(GearState.Reverse);
    //     }
    // }

    private void FixedUpdate()
    {
        if (!isAutonomous)
        {
            if (GearStick.transform.rotation.eulerAngles.x < 20 || GearStick.transform.rotation.eulerAngles.x > 340)
            {
                ChangeGear(GearState.Park);
            } else if (GearStick.transform.rotation.eulerAngles.x < 60)
            {
                ChangeGear(GearState.Drive);
            }
            else
            {
                ChangeGear(GearState.Reverse);
            }
            
            GetInput();
        }
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        // ShowCurrentSpeed();
    }
    
    // For autonomous control
    public void SetInputs( float verticalInput, float horizontalInput, bool isBreaking)
    {
        _verticalInput = verticalInput;
        _horizontalInput = horizontalInput;
        _isBreaking = isBreaking;
    }
    
    // For manual control
    private void GetInput()
    {
        // Steering Input
        _horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        _verticalInput = OVRInput.Get(OVRInput.RawButton.RIndexTrigger) ? 1 : 0 ;
        
        // Breaking Input
        _isBreaking = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
    }

    private void HandleMotor()
    {
        // If the car is not autonomous, disable backward input (move backward only if the car is in reverse gear)
        if (isAutonomous || _verticalInput > 0)
        {
            frontLeftWheelCollider.motorTorque = _verticalInput * motorForce * (float)_currentGearState;
            frontRightWheelCollider.motorTorque = _verticalInput * motorForce * (float)_currentGearState;
        }

        // rb.AddForce(_verticalInput * acceleration * transform.forward, ForceMode.VelocityChange);
        _currentBreakForce = _isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = _currentBreakForce;
        frontLeftWheelCollider.brakeTorque = _currentBreakForce;
        rearLeftWheelCollider.brakeTorque = _currentBreakForce;
        rearRightWheelCollider.brakeTorque = _currentBreakForce;
    }

    private void HandleSteering()
    {
        if (!isAutonomous)
        {
            _currentSteerAngle = SteeringWheel.transform.rotation.eulerAngles.z - 180;
        }
        else
        {
            _currentSteerAngle = _horizontalInput * maxSteerAngle;
        }
        
        // _currentSteerAngle += 180f;
        // _currentSteerAngle = maxSteerAngle * _horizontalInput;
        
        frontLeftWheelCollider.steerAngle = _currentSteerAngle;
        frontRightWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void ShowCurrentSpeed()
    {
        Debug.Log("Speed KM/H: " + GetSpeed());
    }
    
    public float GetSpeed() {
        return rb.velocity.magnitude * 3.6f;
    }
    
    public bool IsStopped()
    {
        return GetSpeed() < 1f;
    }
    
    public GearState GetGearState()
    {
        return _currentGearState;
    }

    private void ChangeGear(GearState newGearState)
    {
        if (_currentGearState != newGearState)
        {
            EventsManager.Instance.TriggerCarGearStateChangedEvent(newGearState);
        }
        _currentGearState = newGearState;
    }
}
public enum GearState
{
    Park = 0,
    Drive = 1,
    Reverse = -1
}



// Liraz functions: 


//    public GameObject Wheel;
//     private float initialWheelRotationZ; // Initial rotation around Z-axis
//     public float speed = 10f;
//     private Quaternion initialRotation;


//    void Start()
//     {
//         // initialWheelRotationZ = Wheel.transform.rotation.eulerAngles.z;
//         initialRotation = Wheel.transform.rotation;
//     }



//  void Update()
//     {
//         // Get the wheel rotation around the Z-axis
//         // float wheelRotationZ = Wheel.transform.rotation.eulerAngles.z - initialWheelRotationZ;
        
        
        
//         // transform.Rotate(transform.rotation.x, wheelRotationZ, transform.rotation.z);
//         transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        
//         // TODO -  here
//         // Calculate the rotation difference
//         Quaternion rotationDifference = Quaternion.Inverse(initialRotation) * Wheel.transform.rotation;

//         // Extract the rotation angle around the Z-axis
//         float zRotation = rotationDifference.eulerAngles.z;

//         // Apply the Y rotation to the car's rotation
//         Vector3 currentRotation = transform.rotation.eulerAngles;
//         // transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + zRotation, currentRotation.z);
//         transform.Rotate(transform.rotation.x, rotationDifference.eulerAngles.z, transform.rotation.z);
//     }
