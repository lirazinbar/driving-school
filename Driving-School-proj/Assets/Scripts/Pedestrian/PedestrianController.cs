using System;
using System.Collections.Generic;
using Enums;
using TrafficObjects.GiveWay;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

namespace Pedestrian
{
    public class PedestrianController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        
        private GameObject _pedestriansSpawner;
        private const float MinDistance = 1f;
        private int _currentKnotIndex = 0;
        private readonly List<Vector3> _knotsPositions = new List<Vector3>();
        
        private float _timer = 0;
        private const float Ttl = 10f;
        
        private StopSign _stopSign;
        private bool _isCrossing;

        private const float FrontSensorsStartPoint = 0f;
        private const float FrontSideSensorsPosition = FrontSensorsStartPoint;
        private const float FrontSideSensorsAngle = 20f;
        private const float SensorStopLength = 3f;
        private const float SensorsHight = 1f;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < Ttl)
            {
                Roam();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (TryRaycast(out RaycastHit hit))
            {
                if (!agent.isStopped && hit.collider.CompareTag("Car"))
                {
                    Stop();
                }
            }
            else
            {
                if (agent.isStopped)
                {
                    Resume();
                }
            }
        }

        private void OnDestroy()
        {
            if (_isCrossing)
            {
                _stopSign.PedestrianFinishCrossing();
            }
        }

        public void Initialize(SplineContainer splineContainer, GameObject spawner)
        {
            PathUtils.SetKnotsPositions(splineContainer, _knotsPositions, ref _currentKnotIndex);
            agent.SetDestination(_knotsPositions[_currentKnotIndex]);
            
            _pedestriansSpawner = spawner.gameObject;
            animator.SetFloat("Vertical", !agent.isStopped ? 1 : 0);
        }
        
        private void Roam()
        {
            if (_knotsPositions.Count == 0)
            {
                Destroy(gameObject);
            }

            if (Vector3.Distance(transform.position, _knotsPositions[_currentKnotIndex]) < MinDistance) 
            {
                _currentKnotIndex++;
                if (_currentKnotIndex < _knotsPositions.Count)
                {
                    agent.SetDestination(_knotsPositions[_currentKnotIndex]);
                }
                else
                {
                    // Destroy the pedestrian when it reaches the end of the path
                    Destroy(gameObject);
                }
            }
        }
        
        public int SpawnerId()
        {
            return _pedestriansSpawner.GetInstanceID();
        }
        
        private bool TryRaycast(out RaycastHit hit)
        {
            bool isHit = false;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
         
            Vector3 frontCenterSensorPos = position + rotation * new Vector3(0, SensorsHight, FrontSensorsStartPoint);
            Vector3 frontRightSensorPos = position + rotation * new Vector3(FrontSideSensorsPosition, SensorsHight, FrontSensorsStartPoint);
            Vector3 frontLeftSensorPos = position + rotation * new Vector3(-FrontSideSensorsPosition, SensorsHight, FrontSensorsStartPoint);
        
            // Front center sensor
            if (Physics.Raycast(frontCenterSensorPos, transform.forward, out hit, SensorStopLength))
            {
                Debug.DrawLine(frontCenterSensorPos, hit.point, Color.red);
                isHit = true;
            }
            
            // Front right angle sensor
            if (Physics.Raycast(frontRightSensorPos, 
                    Quaternion.AngleAxis(FrontSideSensorsAngle, transform.up) * transform.forward,
                    out hit, SensorStopLength / 10f))
            {
                Debug.DrawLine(frontRightSensorPos, hit.point, Color.red);
                isHit = true;
            }
            
            // Front left angle sensor
            if (Physics.Raycast(frontLeftSensorPos, 
                    Quaternion.AngleAxis(-FrontSideSensorsAngle, transform.up) * transform.forward,
                    out hit, SensorStopLength / 10f))
            {
                Debug.DrawLine(frontLeftSensorPos, hit.point, Color.red);
                isHit = true;
            }
        
            return isHit;
        }

        private void Stop()
        {
            agent.isStopped = true;
            animator.SetFloat("Vertical", 0);
        }
        
        private void Resume()
        {
            agent.isStopped = false;
            animator.SetFloat("Vertical", 1);
        }
        
        public void SetIsCrossing(bool isCrossing, StopSign stopSign)
        {
            _isCrossing = isCrossing;
            _stopSign = stopSign;
        }
    }
}