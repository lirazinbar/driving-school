using System.Collections;
using Enums;
using Managers;
using ParkingTest;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cars
{
    public class CarController : MonoBehaviour
    {
        private float _horizontalInput, _verticalInput;
        private float _currentBreakForce;
        private bool _isBreaking;
        private int _speedLimit;
        private GearState _currentGearState = GearState.Drive;
        private bool _isCheckingSpeed = true;
        private bool _isShowingSpeed = true;
        private float _currentMaxSteerAngle;
        private bool isGamePause = false;

        [SerializeField] private Rigidbody rb;
        

        [SerializeField] private GameObject GearStick;
    
        // Settings
        [Header("Car Performance Settings")]
        [SerializeField] private float motorForce;
        [SerializeField] private float breakForce;
        [SerializeField] private float maxSteerAngle;
        [SerializeField] private float acceleration;

        [Header("Control Settings")]
        [SerializeField] private bool isAutonomous;
        [SerializeField] private bool keyboardControlled;
        [SerializeField] private bool isParkingTest;
        
        // Wheel Colliders
        [Header("Front Wheel Colliders")]
        [SerializeField] private WheelCollider frontLeftWheelCollider;
        [SerializeField] private WheelCollider frontRightWheelCollider;

        [Header("Rear Wheel Colliders")]
        [SerializeField] private WheelCollider rearLeftWheelCollider;
        [SerializeField] private WheelCollider rearRightWheelCollider;

        // Wheels
        [Header("Wheel Transforms")]
        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private Transform frontRightWheelTransform;
        [SerializeField] private Transform rearLeftWheelTransform;
        [SerializeField] private Transform rearRightWheelTransform;

        // Front Lights
        [Header("Front Lights")]
        [SerializeField] private Light frontRightLight;
        [SerializeField] private Light frontRightLightPoint;
        [SerializeField] private Light frontLeftLight;
        [SerializeField] private Light frontLeftLightPoint;
        
        
        private void Start()
        {
            if (isParkingTest) return;
            
            _speedLimit = TrafficManager.Instance.GetSpeedLimit();
            
            FrontLightsToggle(GameManager.Instance.IsNightMode());            
        }

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
            if (!isAutonomous && !isParkingTest)
            {
                IsCarBrokeSpeedLimit();
            }
        }

        public void SetIsGamePause(bool isPause)
        {
            isGamePause = isPause;
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
            if (isGamePause)
            {
                _verticalInput = 0;
                _isBreaking = true;
            }
            else if (!keyboardControlled)
            {
                _verticalInput =  OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
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

            if (_currentGearState != GearState.Park)
            {
                rb.AddForce(_verticalInput * acceleration * transform.forward, ForceMode.VelocityChange);
            }
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
            if (isAutonomous || keyboardControlled)
            {
                float _currentSteerAngle = _horizontalInput * maxSteerAngle;

                frontLeftWheelCollider.steerAngle = _currentSteerAngle;
                frontRightWheelCollider.steerAngle = _currentSteerAngle;
            }
        
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
            if (_isCheckingSpeed)
            {
                if (GetSpeed() > _speedLimit)
                {
                    Debug.Log("Speed limit exceeded! (above " + _speedLimit + " KM/H)");
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
    
    private void FrontLightsToggle(bool state)
    {
        frontLeftLight.enabled = state;
        frontRightLightPoint.enabled = state;
        frontRightLight.enabled = state;
        frontLeftLightPoint.enabled = state;
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