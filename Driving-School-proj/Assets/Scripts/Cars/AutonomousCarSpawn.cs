using UnityEngine;
using UnityEngine.Splines;

namespace Cars
{
    public class AutonomousCarSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject carPrefab;
       [SerializeField] private SplineContainer splineContainer;
       
       private float _timer = 10f;
        
        // Instantiate a new car every 10 seconds
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > 10)
            {
                _timer = 0;
                GameObject car = Instantiate(carPrefab, transform.position, Quaternion.identity);
                CarDriverAutonomous autonomousCar = car.GetComponent<CarDriverAutonomous>();
                autonomousCar.Initialize(splineContainer);
            }
        }
    }
}
