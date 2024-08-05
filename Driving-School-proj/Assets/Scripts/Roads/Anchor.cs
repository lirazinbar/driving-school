using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Roads
{
    public class Anchor : MonoBehaviour
    {
        [SerializeField] private bool isEnterAnchor;
        [SerializeField] private List<SplineContainer> splineContainers;


        public SplineContainer GetRandomSplineContainer()
        {
            return isEnterAnchor ? splineContainers[UnityEngine.Random.Range(0, splineContainers.Count)] : null;
        }
    
        public bool IsEnter()
        {
            return isEnterAnchor;
        }
    }
}
