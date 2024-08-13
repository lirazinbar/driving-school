using System.Collections.Generic;
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
        

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < _ttl)
            {
                roam();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Initialize(SplineContainer splineContainer, GameObject spawner)
        {
            PathUtils.SetKnotsPositions(splineContainer, _knotsPositions, ref _currentKnotIndex);
            agent.SetDestination(_knotsPositions[_currentKnotIndex]);
            
            pedestriansSpawner = spawner.gameObject;
            animator.SetFloat("Vertical", !agent.isStopped ? 1 : 0);
        }
        
        private void roam()
        {
            if (_knotsPositions.Count == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                if (Vector3.Distance(transform.position, _knotsPositions[_currentKnotIndex]) < _minDistance) 
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
        }
        
        public int SpawnerId()
        {
            return pedestriansSpawner.GetInstanceID();
        }
    }
}