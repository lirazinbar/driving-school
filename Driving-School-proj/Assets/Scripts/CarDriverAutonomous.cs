using UnityEngine;

public class CarDriverAutonomous : MonoBehaviour
{
    [SerializeField] private Transform targetPositionTransform;
    
    private CarController _carController;
    private Vector3 _targetPosition;
    
    float _forwardAmount;
    float _turnAmount;
    bool _isBreaking;
    
    const float SlowDownDistance = 30f;
    const float StopDistance = 5f;

    private void Awake()
    {
        _carController = GetComponent<CarController>();
    }

    private void FixedUpdate()
    {
        SetTargetPosition(targetPositionTransform.position);
        
        float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);
        Debug.Log("Distance to target: " + distanceToTarget);
        
        // If the car is not close to the target position
        if (distanceToTarget > SlowDownDistance)
        {
            _forwardAmount = CalculateForwardAmount();
            _turnAmount = CalculateTurnAmount();
            _isBreaking = false;
        }
        // The car is close and driving too fast - don't press the gas
        else if (distanceToTarget < SlowDownDistance && distanceToTarget > StopDistance)
        {
            _forwardAmount = _carController.GetSpeed() > 10f ? 0f : CalculateForwardAmount();
            Debug.Log("Speed: " + _forwardAmount);
            _turnAmount = CalculateTurnAmount();
            
            _isBreaking = _carController.GetSpeed() > 30f;
        }
        // The car reached the target position - stop
        else
        {
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
    private float CalculateForwardAmount()
    {
        Transform transform1 = transform;
        Vector3 dirToMovePosition = (_targetPosition - transform1.position).normalized;
        float dot = Vector3.Dot(transform1.forward, dirToMovePosition);
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
        Debug.Log("Angle to dir: " + angleToDir);

        return angleToDir switch
        {
            > 10 => 1f,
            < -10 => -1f,
            _ => angleToDir is < 5 and > -5 ? 0f : _turnAmount
        };
    }
}
