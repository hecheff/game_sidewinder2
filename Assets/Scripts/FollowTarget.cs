using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform    target_transform;
    public Vector3      offset;

    void Update() {
        transform.position = target_transform.position + offset;
    }
}
