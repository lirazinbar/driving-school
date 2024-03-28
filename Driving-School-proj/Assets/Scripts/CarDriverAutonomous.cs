using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class CarDriverAutonomous : MonoBehaviour
{
    // [SerializeField] private Transform targetPositionTransform;
    [SerializeField] private SplineContainer splineContainer; 
    
    private CarController _carController;
    private Vector3 _targetPosition;
    private List<Vector3> _knotsPositions = new List<Vector3>();
    private int _currentKnotIndex;

    private float _forwardAmount;
    private float _turnAmount;
    private bool _isBreaking;

    private const float SlowDownDistance = 25f;
    private const float StopDistance = 5f;

    private void Awake()
    {
        _carController = GetComponent<CarController>();
        
        for (int i = 0; i < splineContainer.Spline.Count; i++)
        {
            float3 knot = splineContainer.Spline.ToArray()[i].Position;
            Vector3 knotLocalPosition = new Vector3(knot.x, knot.y, knot.z);
            Vector3 knotWorldPosition = splineContainer.transform.TransformPoint(knotLocalPosition);
            _knotsPositions.Add(knotWorldPosition);
            Debug.Log(knotWorldPosition);
        }
        SetTargetPosition(_knotsPositions[0]);
    }
    
    private void FixedUpdate()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);
        Debug.Log(distanceToTarget);
        Debug.Log(_carController.GetSpeed());
        
        // If the car is not close to the target position
        if (distanceToTarget > SlowDownDistance)
        {
            _forwardAmount = CalculateForwardAmount(distanceToTarget);
            _turnAmount = CalculateTurnAmount();
            _isBreaking = false;
        }
        // The car is close and driving too fast - don't press the gas
        else if (distanceToTarget < SlowDownDistance && distanceToTarget > StopDistance)
        {
            _forwardAmount = _carController.GetSpeed() > 10f ? 0f : CalculateForwardAmount(distanceToTarget);
            _turnAmount = CalculateTurnAmount();
            
            _isBreaking = _carController.GetSpeed() > 25f;
        }
        // The car reached the target position - stop
        else
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
