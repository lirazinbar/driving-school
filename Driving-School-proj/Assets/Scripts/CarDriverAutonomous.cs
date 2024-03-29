using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class CarDriverAutonomous : MonoBehaviour
{
    // [SerializeField] private Transform targetPositionTransform;
    [SerializeField] private SplineContainer splineContainer; 
    
    private CarController _carController;
    private Vector3 _targetPosition;
    private readonly List<Vector3> _knotsPositions = new List<Vector3>();
    private int _currentKnotIndex;

    private float _forwardAmount;
    private float _turnAmount;
    private bool _isBreaking;

    private const float SlowDownDistance = 25f;
    private const float StopDistance = 5f;
    
    [Header("Sensors")]
    [SerializeField] private float frontSensorsStartPoint = 3f;
    [SerializeField] private float frontSideSensorsPosition = 1.2f;
    [SerializeField] private float frontSideSensorsAngle = 20f;
    [SerializeField] private float sensorsHight = 1f;
    [SerializeField] private float sensorSlowDownLength = 15f;
    [SerializeField] private float sensorStopLength = StopDistance;

    private void Awake()
    {
        _carController = GetComponent<CarController>();
        
        for (int i = 0; i < splineContainer.Spline.Count; i++)
        {
            float3 knot = splineContainer.Spline.ToArray()[i].Position;
            Vector3 knotLocalPosition = new Vector3(knot.x, knot.y, knot.z);
            Vector3 knotWorldPosition = splineContainer.transform.TransformPoint(knotLocalPosition);
            _knotsPositions.Add(knotWorldPosition);
        }
        SetTargetPosition(_knotsPositions[0]);
    }
    
    private void FixedUpdate()
    {
        HitState hitState = Sensors();
        MoveToPoint(hitState);
    }

    private HitState Sensors()
    {
        if (CreateRaycasts(sensorStopLength, Color.red))
        {
            return HitState.Stop;
        }
        if (CreateRaycasts(sensorSlowDownLength, Color.yellow))
        {
            return HitState.SlowDown;
        }
        
        return HitState.None;
    }

    private bool CreateRaycasts(float sensorsLength, Color color)
    {
        RaycastHit hit;
        bool isHit = false;
        Vector3 position = transform.position;
        Vector3 frontCenterSensorPos = position + new Vector3(0, sensorsHight, frontSensorsStartPoint);
        Vector3 frontRightSensorPos = position + new Vector3(frontSideSensorsPosition, sensorsHight, frontSensorsStartPoint);
        Vector3 frontLeftSensorPos = position + new Vector3(-frontSideSensorsPosition, sensorsHight, frontSensorsStartPoint);
        
        // Front center sensor
        if (Physics.Raycast(frontCenterSensorPos, transform.forward, out hit, sensorsLength))
        {
            Debug.DrawLine(frontCenterSensorPos, hit.point, color);
            isHit = true;
        }
        
        // Front right sensor
        if (Physics.Raycast(frontRightSensorPos, transform.forward, out hit, sensorsLength))
        {
            Debug.DrawLine(frontRightSensorPos, hit.point, color);
            isHit = true;
        }

        // Front right angle sensor
        if (Physics.Raycast(frontRightSensorPos, 
                Quaternion.AngleAxis(frontSideSensorsAngle, transform.up) * transform.forward,
                out hit, sensorsLength / 5f))
        {
            Debug.DrawLine(frontRightSensorPos, hit.point, color);
            isHit = true;
        }
        
        // Front left sensor
        if (Physics.Raycast(frontLeftSensorPos, transform.forward, out hit, sensorsLength))
        {
            Debug.DrawLine(frontLeftSensorPos, hit.point, color);
            isHit = true;
        }
        
        // Front left angle sensor
        if (Physics.Raycast(frontLeftSensorPos, 
                Quaternion.AngleAxis(-frontSideSensorsAngle, transform.up) * transform.forward,
                out hit, sensorsLength / 5f))
        {
            Debug.DrawLine(frontLeftSensorPos, hit.point, color);
            isHit = true;
        }
        
        return isHit;
    }

    private void MoveToPoint(HitState hitState)
    {
        float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);
        Debug.Log("Speed KM/H: " + _carController.GetSpeed());
        
        // The car reached the target position - stop
        if (distanceToTarget < StopDistance || hitState == HitState.Stop)
        {
            if (_currentKnotIndex < _knotsPositions.Count - 1)
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
            _forwardAmount = CalculateForwardAmount(distanceToTarget);
            _turnAmount = CalculateTurnAmount();
            _isBreaking = false;
        }
        // The car is close and driving too fast - don't press the gas
        else if (distanceToTarget <= SlowDownDistance && distanceToTarget > StopDistance || hitState == HitState.SlowDown)
        {
            _forwardAmount = _carController.GetSpeed() > 15f ? 0f : CalculateForwardAmount(distanceToTarget);
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
    private float CalculateForwardAmount(float distanceToTarget)
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

public enum HitState
{
    None,
    SlowDown,
    Stop
}
