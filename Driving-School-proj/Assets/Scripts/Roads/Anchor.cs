using System;
using System.Collections.Generic;
using Cars;
using UnityEngine;
using UnityEngine.Splines;

namespace Roads
{
    public class Anchor : MonoBehaviour
    {
        [SerializeField] private bool isEnterAnchor;
        [SerializeField] private List<SplineContainer> splineContainers;
    
        // private bool _hasCollided;
        

        // private void OnCollisionStay(Collision other)
        // {
        //     if (!_hasCollided)
        //     {
        //         if (other.gameObject.CompareTag("Anchor"))
        //         {
        //             Anchor _adjacentAnchor = other.gameObject.GetComponent<Anchor>();
        //             if (_adjacentAnchor.IsEnter())
        //             {
        //                 splineContainer.SetAdjacentSpline(_adjacentAnchor.GetSplineContainer());
        //             }
        //             _hasCollided = true;
        //         }
        //     }
        // }


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
