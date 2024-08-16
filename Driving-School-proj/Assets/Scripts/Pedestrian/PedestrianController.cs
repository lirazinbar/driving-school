using System;
using System.Collections.Generic;
using Enums;
using TrafficObjects.GiveWay;
using Unity.VisualScripting;
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
        private const float FrontSideSensorsAngle = 20f;
        private const float SensorStopLength = 2f;
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
                if (!agent.isStopped)
                {
                    if (hit.collider.CompareTag("Car"))
                    {
                        Stop();
                    }
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
            if (_knotsPositions.Count == 0)
            {
                Debug.LogWarning("No knots found in the spline container for pedestrian");
                Destroy(gameObject);
            }
            
            agent.SetDestination(_knotsPositions[_currentKnotIndex]);
            
            _pedestriansSpawner = spawner.gameObject;
            animator.SetFloat("Vertical", !agent.isStopped ? 1 : 0);
        }
        
        private void Roam()
        {
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
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
         
            Vector3 frontCenterSensorPos = position + rotation * new Vector3(0, SensorsHight, FrontSensorsStartPoint);
        
            // Front center sensor
            if (Physics.Raycast(frontCenterSensorPos, transform.forward, out hit, SensorStopLength))
            {
                Debug.DrawLine(frontCenterSensorPos, hit.point, Color.red);
                return true;
            }
            
            // Front right angle sensor
            if (Physics.Raycast(frontCenterSensorPos, 
                    Quaternion.AngleAxis(FrontSideSensorsAngle, transform.up) * transform.forward,
                    out hit, SensorStopLength))
            {
                Debug.DrawLine(frontCenterSensorPos, hit.point, Color.red);
                return true;
            }
            
            // Front left angle sensor
            if (Physics.Raycast(frontCenterSensorPos, 
                    Quaternion.AngleAxis(-FrontSideSensorsAngle, transform.up) * transform.forward,
                    out hit, SensorStopLength))
            {
                Debug.DrawLine(frontCenterSensorPos, hit.point, Color.red);
                return true;
            }
        
            return false;
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