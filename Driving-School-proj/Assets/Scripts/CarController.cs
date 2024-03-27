using UnityEngine;

public class CarController : MonoBehaviour
{
    private float _horizontalInput, _verticalInput;
    private float _currentSteerAngle, _currentBreakForce;
    private bool _isBreaking;
    private GearState _currentGearState = GearState.Drive;

    [SerializeField] private Rigidbody rb;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle, acceleration;
    [SerializeField] private bool isAutonomous;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Update()
    {
        if (isAutonomous) return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeGear(GearState.Park);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeGear(GearState.Drive);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeGear(GearState.Reverse);
        }
    }

    private void FixedUpdate()
    {
        if (!isAutonomous)
        { 
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
        _verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        _isBreaking = Input.GetKey(KeyCode.Space);
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
        _currentSteerAngle = maxSteerAngle * _horizontalInput;
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
        Debug.Log("Speed KM/H: " + rb.velocity.magnitude * 3.6f);
    }
    
    public float GetSpeed() {
        return rb.velocity.magnitude * 3.6f;
    }

    private void ChangeGear(GearState newGearState)
    {
        _currentGearState = newGearState;
    }
}
public enum GearState
{
    Park = 0,
    Drive = 1,
    Reverse = -1
}