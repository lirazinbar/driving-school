using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Pedestrian
{
    public class PedestrianSpawner: MonoBehaviour
    {
        [SerializeField] private GameObject[] pedestrianPrefabs;
        private List<SplineContainer> _splineContainers = new List<SplineContainer>();
        
        private float _spawnInterval = (float)PedestrianDifficulty.Hard;
        private float _timer;
        private bool _isSpawning = true;

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

            _timer = Random.Range(0, _spawnInterval / 2);
        }

        private void Update()
        {
            if (_isSpawning)
            {
                _timer += Time.deltaTime;
                if (_timer > _spawnInterval)
                {
                    _timer = Random.Range(0, _spawnInterval / 3);
                    // Instantiate a new pedestrian every SpawnInterval seconds randomly from the list of prefabs
                    GameObject pedestrianPrefab = pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Length)];
                
                    GameObject pedestrian = Instantiate(pedestrianPrefab, transform.position,
                        transform.rotation * Quaternion.Euler(0, 180, 0));
            
                    PedestrianController pedestrianController = pedestrian.GetComponent<PedestrianController>();
                    if (_splineContainers.Count > 0)
                    {
                        pedestrianController.Initialize(_splineContainers[Random.Range(0, _splineContainers.Count)], gameObject);
                    }
                }
            }
        }
        
        public void SetDifficulty(PedestrianDifficulty pedestrianDifficulty)
        {
            if (pedestrianDifficulty == PedestrianDifficulty.None)
            {
                _isSpawning = false;
            }
            else
            {
                _spawnInterval = (float)pedestrianDifficulty;
            }
        }
    }
}