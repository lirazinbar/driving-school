using System.Collections.Generic;
using Enums;
using Roads;
using UnityEngine;
using UnityEngine.Splines;


namespace Cars
{
    public class CarDriverAutonomous : MonoBehaviour
    {
        // [SerializeField] private Transform targetPositionTransform;
        // [SerializeField] private SplineContainer splineContainer; 
    
        private CarController _carController;
        private Vector3 _targetPosition;
        private readonly List<Vector3> _knotsPositions = new List<Vector3>();
        private int _currentKnotIndex;

        private float _forwardAmount;
        private float _turnAmount;
        private bool _isBreaking;

        private const float SlowDownDistance = 25f;
        private const float StopDistance = 3f;
    
        [Header("Sensors")]
        [SerializeField] private float frontSensorsStartPoint = 3f;
        [SerializeField] private float frontSideSensorsPosition = 1.2f;
        [SerializeField] private float frontSideSensorsAngle = 20f;
        [SerializeField] private float sensorSlowDownLength = 25f;
        [SerializeField] private float sensorStopLength = StopDistance;
        [SerializeField] private float sensorsHight = 1f;
        private Dictionary<RaycastType, int> _bitmasks = new Dictionary<RaycastType, int>( );

        private void Awake()
        {
            _carController = GetComponent<CarController>();
        
            _bitmasks.Add(RaycastType.Stop, -1);
            _bitmasks.Add(RaycastType.SlowDown, -1);
            _bitmasks[RaycastType.Stop] = IgnoreRaycastLayers(_bitmasks[RaycastType.Stop], 
                new[] {"StopSurfaceDetector", "TrafficLightSurfaceDetector", "Anchor", "DestroyPoint",
                    "JunctionExitDetector"});
            _bitmasks[RaycastType.SlowDown] = IgnoreRaycastLayers(_bitmasks[RaycastType.SlowDown],
                new[] {"TrafficLightSurfaceDetector", "Anchor", "DestroyPoint", "JunctionExitDetector"});
        }

        void FixedUpdate()
        {
            HitState hitState = Sensors();
            MoveToPoint(hitState);
        }
        
        public void Initialize(SplineContainer splineContainer)
        {
            // Debug.Log("Initialize car");
            PathUtils.SetKnotsPositions(splineContainer, _knotsPositions, ref _currentKnotIndex);
            SetTargetPosition(_knotsPositions[0]);
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Anchor"))
            {
                Anchor anchor = other.GetComponent<Anchor>();
                if (anchor.IsEnter())
                {
                    SplineContainer splineContainer = anchor.GetRandomSplineContainer();
                    if (splineContainer != null)
                    {
                        PathUtils.SetKnotsPositions(splineContainer, _knotsPositions, ref _currentKnotIndex);
                        SetTargetPosition(_knotsPositions[0]);
                    }
                }
            }
            
            if (other.CompareTag("DestroyPoint") && !gameObject.CompareTag("MainCar")) 
            {
                Debug.Log("Destroy car");
                Destroy(gameObject);
            }
        }

        private HitState Sensors()
        {
            HitState generalHit = GeneralRaycast();
            return generalHit;
        }
    
        private HitState GeneralRaycast()
        {
            if (CreateRaycasts(sensorStopLength, Color.red, _bitmasks[RaycastType.Stop]))
            {
                return HitState.Stop;
            }
            if (CreateRaycasts(sensorSlowDownLength, Color.yellow, _bitmasks[RaycastType.SlowDown]))
            {
                return HitState.SlowDown;
            }
            return HitState.None;
        }

        public void SetLayerOfRaycast(RaycastType raycastType, string layerName, bool toIgnore)
        {
            int layer = LayerMask.NameToLayer(layerName);
            _bitmasks[raycastType] = toIgnore ? _bitmasks[raycastType] & ~(1 << layer) : _bitmasks[raycastType] | 1 << layer;
        }
    
        private int IgnoreRaycastLayers(int bitmask, string[] layersToIgnore)
        {
            // Debug.Log("bitmask: " + Convert.ToString(bitmask, 2).PadLeft(32, '0'));
            int ignoreBitmask = 0;
            foreach (string layer in layersToIgnore)
            {
                ignoreBitmask |= 1 << LayerMask.NameToLayer(layer);
            }
        
            // Debug.Log("ignoreBitmask: " + Convert.ToString(~ignoreBitmask, 2).PadLeft(32, '0'));
            // Debug.Log("bitmask & ~ignoreBitmask: " + (Convert.ToString(bitmask & ~ignoreBitmask, 2).PadLeft(32, '0')));
            return bitmask & ~ignoreBitmask;
        }

        private bool CreateRaycasts(float sensorsLength, Color color, int bitmask)
        {
            RaycastHit hit;
            bool isHit = false;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
         
            Vector3 frontCenterSensorPos = position + rotation * new Vector3(0, sensorsHight, frontSensorsStartPoint);
            Vector3 frontRightSensorPos = position + rotation * new Vector3(frontSideSensorsPosition, sensorsHight, frontSensorsStartPoint);
            Vector3 frontLeftSensorPos = position + rotation * new Vector3(-frontSideSensorsPosition, sensorsHight, frontSensorsStartPoint);
        
            // Front center sensor
            if (Physics.Raycast(frontCenterSensorPos, transform.forward, out hit, sensorsLength, bitmask))
            {
                Debug.DrawLine(frontCenterSensorPos, hit.point, color);
                isHit = true;
            }
        
            // Front right sensor
            if (Physics.Raycast(frontRightSensorPos, transform.forward, out hit, sensorsLength,bitmask))
            {
                Debug.DrawLine(frontRightSensorPos, hit.point, color);
                isHit = true;
            }

            // Front right angle sensor
            if (Physics.Raycast(frontRightSensorPos, 
                    Quaternion.AngleAxis(frontSideSensorsAngle, transform.up) * transform.forward,
                    out hit, sensorsLength / 10f, bitmask))
            {
                Debug.DrawLine(frontRightSensorPos, hit.point, color);
                isHit = true;
            }
        
            // Front left sensor
            if (Physics.Raycast(frontLeftSensorPos, transform.forward, out hit, sensorsLength, bitmask))
            {
                Debug.DrawLine(frontLeftSensorPos, hit.point, color);
                isHit = true;
            }

            // Front left angle sensor
            if (Physics.Raycast(frontLeftSensorPos, 
                    Quaternion.AngleAxis(-frontSideSensorsAngle, transform.up) * transform.forward,
                    out hit, sensorsLength / 10f, bitmask))
            {
                Debug.DrawLine(frontLeftSensorPos, hit.point, color);
                isHit = true;
            }
        
            return isHit;
        }

        private void MoveToPoint(HitState hitState)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

            // The car reached the target position - stop
            if (distanceToTarget < StopDistance || hitState == HitState.Stop)
            {
                if (_currentKnotIndex < _knotsPositions.Count - 1 && distanceToTarget < StopDistance)
                {
                    _currentKnotIndex++;
                    SetTargetPosition(_knotsPositions[_currentKnotIndex]);
                }

                _forwardAmount = 0f;
                _turnAmount = 0f;
                _isBreaking = true;
            }
        
            // If the car is not close to the target position
            else if (distanceToTarget > SlowDownDistance && hitState == HitState.None)
            {
                _forwardAmount = CalculateForwardAmount();
                _turnAmount = CalculateTurnAmount();
                _isBreaking = false;
            }
            // The car is close and driving too fast - don't press the gas
            else if (distanceToTarget <= SlowDownDistance && distanceToTarget > StopDistance || hitState == HitState.SlowDown)
            {
                _forwardAmount = _carController.GetSpeed() > 15f ? 0f : CalculateForwardAmount();
                _turnAmount = CalculateTurnAmount();
                _isBreaking = _carController.GetSpeed() > 25f;
            }
        
            _carController.SetInputs(_forwardAmount, _turnAmount, _isBreaking);
        }

        private void SetTargetPosition(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        // Calculates the forward amount based on the direction to the target position.
        private float CalculateForwardAmount()
        {
            Transform transform1 = transform;
            Vector3 dirToMovePosition = (_targetPosition - transform1.position).normalized;
            float dot = Vector3.Dot(transform1.forward, dirToMovePosition);
            // Target position is in front of the car
            if (dot > 0)
            {
                return 1f;
            }

            return -1f;
        }
    
        // Calculates the turn amount based on the direction to the target position.
        private float CalculateTurnAmount()
        {
            Transform transform1 = transform;
            Vector3 dirToMovePosition = (_targetPosition - transform1.position).normalized;
            float angleToDir = Vector3.SignedAngle(transform1.forward, dirToMovePosition, Vector3.up);
            // Debug.Log("Angle to dir: " + angleToDir);

            if (angleToDir > 10 && angleToDir < 45 || angleToDir < 170 && angleToDir > 45)
            {
                return 1f;
            }
            if (angleToDir < -10 && angleToDir > -45 || angleToDir > -170 && angleToDir < -45)
            {
                return -1f;
            }
            if (angleToDir < 5 && angleToDir > -5 || angleToDir < -175 || angleToDir > 175)
            {
                return 0f;
            }
        
            return _turnAmount;
        }

    }
}