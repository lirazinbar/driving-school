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
        
        private GameObject pedestriansSpawner;
        private float _minDistance = 1f;
        private int _currentKnotIndex = 0;
        private readonly List<Vector3> _knotsPositions = new List<Vector3>();
        
        private float _timer = 0;
        private readonly float _ttl = 10f;
        
        private StopSign _stopSign;
        private bool _isCrossing;

        private float frontSensorStartPoint = 0f;
        private float sensorStopLength = 3f;
        private float sensorsHight = 1f;
        // private Dictionary<RaycastType, int> _bitmasks = new Dictionary<RaycastType, int>();

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < _ttl)
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
                if (agent.isActiveAndEnabled && agent.enabled && agent.isOnNavMesh && agent.isStopped)
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
            
            pedestriansSpawner = spawner.gameObject;
            animator.SetFloat("Vertical", !agent.isStopped ? 1 : 0);
        }
        
        private void Roam()
        {
            if (_knotsPositions.Count == 0)
            {
                Destroy(gameObject);
            }

            if (_currentKnotIndex < _knotsPositions.Count && Vector3.Distance(transform.position, _knotsPositions[_currentKnotIndex]) < _minDistance) 
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
            return pedestriansSpawner.GetInstanceID();
        }
        
        private bool TryRaycast(out RaycastHit hit)
        {
            bool isHit = false;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
         
            Vector3 frontCenterSensorPos = position + rotation * new Vector3(0, sensorsHight, frontSensorStartPoint);
        
            // Front center sensor
            if (Physics.Raycast(frontCenterSensorPos, transform.forward, out hit, sensorStopLength))
            {
                Debug.DrawLine(frontCenterSensorPos, hit.point, Color.red);
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