using System.Collections;
using Enums;
using Managers;
using UnityEngine;

namespace Cars
{
    public class CarController : MonoBehaviour
    {
        private float _horizontalInput, _verticalInput;
        private float _currentSteerAngle, _currentBreakForce;
        private bool _isBreaking;
        private GearState _currentGearState = GearState.Drive;
        private bool _isCheckingSpeed = true;
        private bool _isShowingSpeed = true;

        [SerializeField] private Rigidbody rb;

        [SerializeField] private GameObject SteeringWheel;
    
        [SerializeField] private GameObject GearStick;

        // Settings
        [SerializeField] private float motorForce, breakForce, maxSteerAngle, acceleration;
        [SerializeField] private bool isAutonomous, keyboardControlled;
        private float _currentMaxSteerAngle;

        // Wheel Colliders
        [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
        [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

        // Wheels
        [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
        [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

        private void FixedUpdate()
        {
            if (!isAutonomous)
            {
                if (!keyboardControlled) {
                    if (GearStick.transform.rotation.eulerAngles.x < 10 || GearStick.transform.rotation.eulerAngles.x > 350)
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
                }
            
                GetInput();
            }
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            // ShowCurrentSpeed();
            IsCarBrokeSpeedLimit();
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

            // Acceleration and Breaking Input
            if (!keyboardControlled)
            {
                _verticalInput = OVRInput.Get(OVRInput.RawButton.RIndexTrigger) ? 1 : 0 ;
                _isBreaking = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
            }
            else
            {
                _verticalInput = Input.GetAxis("Vertical");
                _isBreaking = Input.GetKey(KeyCode.Space);
            }
        }

        private void HandleMotor()
        {
            // If the car is not autonomous, disable backward input (move backward only if the car is in reverse gear)
            if (!isAutonomous && !keyboardControlled)
            {
                frontLeftWheelCollider.motorTorque = _verticalInput * motorForce * (float)_currentGearState;
                frontRightWheelCollider.motorTorque = _verticalInput * motorForce * (float)_currentGearState;
            }
            else
            {
                frontLeftWheelCollider.motorTorque = _verticalInput * motorForce;
                frontRightWheelCollider.motorTorque = _verticalInput * motorForce;
            }

            rb.AddForce(_verticalInput * acceleration * transform.forward, ForceMode.VelocityChange);
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
            if (!isAutonomous && !keyboardControlled)
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
            if (_isShowingSpeed)
            {
                Debug.Log("Speed KM/H: " + GetSpeed());
                _isShowingSpeed = false;
                StartCoroutine(WaitForShowSpeed());
            }
        }
    
        public float GetSpeed() {
            return rb.velocity.magnitude * 3.6f;
        }
    
        private void IsCarBrokeSpeedLimit()
        {
            if (_isCheckingSpeed && !isAutonomous)
            {
                if (GetSpeed() > TrafficManager.Instance.GetSpeedLimit())
                {
                    Debug.Log("Speed limit exceeded! (above " + TrafficManager.Instance.GetSpeedLimit() + " KM/H)");
                    EventsManager.Instance.TriggerCarBrokeSpeedLimitEvent();
                    _isCheckingSpeed = false;
                    StartCoroutine(WaitForCheckSpeed());
                }
            }
        }
    
        private IEnumerator WaitForCheckSpeed()
        {
            yield return new WaitForSeconds(10);
            _isCheckingSpeed = true;
        }
    
        private IEnumerator WaitForShowSpeed()
        {
            yield return new WaitForSeconds(1);
            _isShowingSpeed = true;
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
}