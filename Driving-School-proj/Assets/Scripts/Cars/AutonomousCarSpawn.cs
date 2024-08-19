using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Cars
{
    public class AutonomousCarSpawn : MonoBehaviour
    { 
        [SerializeField] private GameObject[] vehiclesPrefabs;
        private Dictionary<string, Material[]> _materials = new Dictionary<string, Material[]>();
        [SerializeField] private List<SplineContainer> splineContainers;
        private float _spawnInterval;
        private float _timer;
        private bool _isSpawning;

        private void Start()
        {
            // Load prefabs form directory
            vehiclesPrefabs = Resources.LoadAll<GameObject>("Prefabs/Vehicles");
            
            // Load materials from all directories under Materials/Vehicles
            string[] materialsDirectoriesPaths = Directory.GetDirectories("Assets/Resources/Materials/Vehicles");
            foreach (string directoryPath in materialsDirectoriesPaths)
            {
                string directoryName = Path.GetFileNameWithoutExtension(directoryPath);
                if (directoryName != null)
                    _materials[directoryName] = Resources.LoadAll<Material>("Materials/Vehicles/" + directoryName);
            }
        }

        private void Update()
        {
            if (_isSpawning)
            {
                _timer += Time.deltaTime;
                if (_timer > _spawnInterval)
                {
                    _timer = 0;
                    // Instantiate a new vehicle every SpawnInterval seconds randomly from the list of prefabs
                    GameObject vehiclePrefab = vehiclesPrefabs[Random.Range(0, vehiclesPrefabs.Length)];

                    // Set a random material for the vehicle if there are any materials for the vehicle type
                    string vehicleType = vehiclePrefab.name;
                    if (_materials.ContainsKey(vehicleType) && _materials[vehicleType].Length > 0)
                    {
                        var vehicleBody = vehiclePrefab.transform.Find("Body");
                        vehicleBody.GetComponent<Renderer>().material = _materials["PrivateCar"][Random.Range(0, _materials["PrivateCar"].Length)];
                    }
                    GameObject car = Instantiate(vehiclePrefab, transform.position, transform.parent.rotation);
            
                    CarDriverAutonomous autonomousCar = car.GetComponent<CarDriverAutonomous>();
                    autonomousCar.Initialize(splineContainers[Random.Range(0, splineContainers.Count)]);
                }
            }
        }
        
        public void SetSpawnInterval(float interval)
        {
            _spawnInterval = interval;
            if (_spawnInterval != 0f)
            {
                _isSpawning = true;
                _timer = _spawnInterval;
            }
        }
    }
}
