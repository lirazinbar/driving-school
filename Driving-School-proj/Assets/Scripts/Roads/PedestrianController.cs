using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

namespace Roads
{
    public class PedestrianController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        
        private float _minDistance = 1f;
        private int _currentKnotIndex = 0;
        private readonly List<Vector3> _knotsPositions = new List<Vector3>();
        

        private void Update()
        {
            roam();
        }
        
        public void Initialize(SplineContainer splineContainer)
        {
            PathUtils.SetKnotsPositions(splineContainer, _knotsPositions, ref _currentKnotIndex);
            agent.SetDestination(_knotsPositions[_currentKnotIndex]);
        }
        
        private void roam()
        {
            if (_knotsPositions.Count == 0)
            {
                Destroy(gameObject);
            }

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
}