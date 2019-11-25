using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Raycast experiment for collision with stage elements
public class RayCast_Test2 : MonoBehaviour {
    
    const float skinWidth = 0.15f;

    BoxCollider collider;
    RaycastOrigins raycastOrigins;

    void Start() {
        collider = GetComponent<BoxCollider>();
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector3();
    }

    struct RaycastOrigins {
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;
    }
}
