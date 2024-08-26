using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPositionTracker : MonoBehaviour
{
    [SerializeField] private Transform gameOverCanvasPosition;

    void Update()
    {
        transform.position = gameOverCanvasPosition.position;
        transform.rotation = gameOverCanvasPosition.rotation;
    }
}
