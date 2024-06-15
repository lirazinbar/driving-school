using Managers;
using UnityEngine;

namespace TrafficObjects
{
    public static class TrafficObjectsUtils
    {
        // other is the object that has hit obj
        public static string CheckHitSide(Transform obj, Collider other)
        {
            // Get the contact point (approximation by using collider positions)
            Vector3 contactPoint = other.ClosestPoint(obj.position);

            // Get the direction of the impact
            Vector3 direction = (contactPoint - obj.position).normalized;
            
            // Convert the direction to the child's local space
            Vector3 localDirection = obj.InverseTransformDirection(direction);

            // Determine the side based on the direction
            string hitSide = DetermineHitSide(localDirection);

            return hitSide;
        }
    
        private static string DetermineHitSide(Vector3 direction)
        {
            string hitSide = "";
            // Determine which side was hit based on the direction vector
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                hitSide = direction.x > 0 ? "Right" : "Left";
            }
            else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && Mathf.Abs(direction.y) > Mathf.Abs(direction.z))
            {
                hitSide = direction.y > 0 ? "Top" : "Bottom";
            }
            else
            {
                hitSide = direction.z > 0 ? "Front" : "Back";
            }
        
            return hitSide;
        }
    }
}