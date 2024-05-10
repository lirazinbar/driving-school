using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Cars
{
    public class AutonomousCarSpawn : MonoBehaviour
    { 
        [SerializeField] private GameObject carPrefab; 
        [SerializeField] private List<SplineContainer> splineContainers;
        private const float SpawnInterval = 12f;
        private float _timer = SpawnInterval;
        
       // Instantiate a new car every SpawnInterval seconds
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > SpawnInterval)
            {
                _timer = 0;
                GameObject car = Instantiate(carPrefab, transform.position, transform.parent.rotation);

                CarDriverAutonomous autonomousCar = car.GetComponent<CarDriverAutonomous>();
                autonomousCar.Initialize(splineContainers[Random.Range(0, splineContainers.Count)]);
            }
        }
    }
}
