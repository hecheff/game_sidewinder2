using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    public float scrollSpeed = 4.0f;    // Scroll speed per second (requires pre-processing)

    private float scrollSpeedPerFrame;  // Processing of scroll speed from per second to per frame

    void Start() 
    {
        scrollSpeedPerFrame = scrollSpeed/(GlobalController.Instance.targetFrameRate);  // Convert scroll speed per second to per frame
    }

    void Update()
    {
        transform.position = transform.position + Vector3.left * scrollSpeedPerFrame;
    }
}
