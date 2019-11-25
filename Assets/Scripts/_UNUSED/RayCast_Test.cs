using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Raycast experiment for use in laser weapon
public class RayCast_Test : MonoBehaviour {

    private Ray ray;
    private RaycastHit hitInfo;

    //public LayerMask mask;
    public float currentLength = 0.0f;

    void Update() {
        ray = new Ray(transform.position, transform.right);
        
        if(Physics.Raycast(ray, out hitInfo, currentLength)) {
            if(hitInfo.transform.gameObject.tag == "Enemy" || hitInfo.transform.gameObject.tag == "Stage") {
                Debug.DrawLine(transform.position, hitInfo.point, Color.green);
                currentLength = hitInfo.point.x - transform.position.x;
            } else {
                Debug.DrawLine(transform.position, transform.position + new Vector3(currentLength,0,0), Color.red);
                currentLength += 0.3f;
            }
        } else {
            Debug.DrawLine(transform.position, transform.position + new Vector3(currentLength,0,0), Color.red);
            currentLength += 0.3f;
        }
    }
}
