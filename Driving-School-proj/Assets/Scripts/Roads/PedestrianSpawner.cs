using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Roads
{
    public class PedestrianSpawner: MonoBehaviour
    {
        [SerializeField] private GameObject[] pedestrianPrefabs;
        private List<SplineContainer> _splineContainers = new List<SplineContainer>();
        
        private const float SpawnInterval = 12f;
        private float _timer = SpawnInterval;

        void Start()
        {
            // Load splines
            Transform pedestriansSplines = transform.Find("PedestriansSplines");
            for (int i = 0; i < pedestriansSplines.childCount; i++)
            {
                SplineContainer splineContainer = pedestriansSplines.GetChild(i).GetComponent<SplineContainer>();
                _splineContainers.Add(splineContainer);
            }
            
            // Load prefabs form directory
            pedestrianPrefabs = Resources.LoadAll<GameObject>("Prefabs/Pedestrians");
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > SpawnInterval)
            {
                _timer = 0;
                // Instantiate a new pedestrian every SpawnInterval seconds randomly from the list of prefabs
                GameObject pedestrianPrefab = pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Length)];
                
                GameObject pedestrian = Instantiate(pedestrianPrefab, transform.position, transform.parent.rotation);
            
                PedestrianController pedestrianController = pedestrian.GetComponent<PedestrianController>();
                pedestrianController.Initialize(_splineContainers[Random.Range(0, _splineContainers.Count)]);
            }
        }
    }
}