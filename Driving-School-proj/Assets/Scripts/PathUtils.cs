using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;


public class PathUtils: MonoBehaviour
{
    public static void SetKnotsPositions(SplineContainer splineContainer, List<Vector3> knotsPositions, ref int currentKnotIndex)
    {
        knotsPositions.Clear();
        currentKnotIndex = 0;
        for (int i = 0; i < splineContainer.Spline.Count; i++)
        {
            float3 knot = splineContainer.Spline.ToArray()[i].Position;
            Vector3 knotLocalPosition = new Vector3(knot.x, knot.y, knot.z);
            Vector3 knotWorldPosition = splineContainer.transform.TransformPoint(knotLocalPosition);
            knotsPositions.Add(knotWorldPosition);
        }
    }
}
