using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawnController : MonoBehaviour {
    public bool isInsideCollider = false;

    void OnTriggerEnter(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            isInsideCollider = true;
        } else {
            isInsideCollider = false;
        }
    }
    void OnTriggerStay(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            isInsideCollider = true;
        } else {
            isInsideCollider = false;
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            isInsideCollider = false;
        }
    }
}
