using System;
using Cars;
using TMPro;
using UnityEngine;

namespace VR
{
    public class Speedometer : MonoBehaviour
    {
        private const float MAX_SPEED_ANGLE = -20;
        private const float ZERO_SPEED_ANGLE = 210;
        
        private Transform needleTransform;
        private Transform speedLabelTemplateTransform;
        
        [SerializeField] private CarController carController;

        private float speedMax;
        private float speed;

        private void Awake()
        {
            needleTransform = transform.Find("Needle");
            speedLabelTemplateTransform = transform.Find("SpeedLabelTemplate");
            speedLabelTemplateTransform.gameObject.SetActive(false);
            
            speed = 0f;
            speedMax = 200f;
            
            CreateSpeedLabels();
        }

        private void Update()
        {
            speed = carController.GetSpeed();
            if (speed > speedMax) speed = speedMax;
            
            Vector3 currentRotation = needleTransform.localEulerAngles;
            // Set the Z axis to your desired rotation and keep the X and Y axes unchanged
            needleTransform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, GetSpeedRotation());
        }

        private void CreateSpeedLabels()
        {
            int labelAmount = 10;
            float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
            
            Vector3 currentRotation = speedLabelTemplateTransform.localEulerAngles;
            
            for (int i = 0; i <= labelAmount; i++)
            {
                Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
                
                float labelSpeedNormalized = (float) i / labelAmount;
                float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
                
                // Rotate around the Z axis for the circular layout
                speedLabelTransform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, speedLabelAngle);
                
                // Ensure the text itself is upright
                speedLabelTransform.Find("SpeedText").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
                speedLabelTransform.Find("SpeedText").localEulerAngles = new Vector3(0, 0, -speedLabelAngle);
                
                speedLabelTransform.gameObject.SetActive(true);
            }
            
            needleTransform.SetAsLastSibling();
        }
        
        private float GetSpeedRotation()
        {
            float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
            
            float speedNormalized = speed / speedMax;
            
            return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
        }
    }
}
