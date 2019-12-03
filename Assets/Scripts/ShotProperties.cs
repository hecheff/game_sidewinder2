using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotProperties : MonoBehaviour {    
    public bool updateOverride = false;

    public float    shotSpeed           = 20.0f;    // Flight speed of shot

    // Rotation properties of projectile
    public Axis     rotationAxis        = Axis.X;
    public float    rotationAngle       = 0;        // Rotation of shot (counter-clockwise from right = 0 degrees)

    private Rigidbody rigidBody;             // Rigidbody for current shot fired. Only used for kinetic-fired weapons.
    private Transform shotPool_location;     // ShotPool object location (to allow expended shots to return)
    private ObjectPool shotHitPool;          // Shot hit pool (called immediately)
    //public GameObject contactExplosion;

    void Awake() {
        if(shotSpeed != 0) {
            rigidBody = transform.GetComponent<Rigidbody>();
        }
        shotPool_location = transform.parent.transform;
        shotHitPool = GameObject.Find("PlayerShotHit_Pool").GetComponent<ObjectPool>();
    }


    void Start() {
        SetShotValues();
    }

    void LateUpdate() { 
        if(updateOverride) {
            SetShotValues();
            updateOverride = false;
        }
    }
    
    // Checks if bullet leaves Boundary
    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Boundary")) {
            ReturnToShotPool();
        }
    }

    void OnTriggerEnter(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
            ReturnToShotPool();
        }
    }

    void OnTriggerStay(Collider other) {
        // If bullet is within enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
            ReturnToShotPool();
        }
    }
    
    void SetShotValues() {
        float velocity_x = 0.0f;
        float velocity_y = 0.0f;

        // If rotation is divisible by 90, apply method unique to those cases
        // Otherwise, use calculated method
        if((rotationAngle%90 == 0)) {
            if(rotationAngle == 0) {            // Fire forward
                Debug.Log("Fire: Right");
                velocity_x = shotSpeed;
                velocity_y = 0.0f;
            } else if(rotationAngle == -90) {   // Fire downward
                Debug.Log("Fire: Down");
                velocity_x = 0.0f;
                velocity_y = -shotSpeed;
            } else if(rotationAngle == 90) {    // Fire upward
                Debug.Log("Fire: Up");
                velocity_x = 0.0f;
                velocity_y = shotSpeed;
            } else if(rotationAngle == 180) {   // Fire backward
                Debug.Log("Fire: Back");
                velocity_x = -shotSpeed;
                velocity_y = 0.0f;
            }
        } else {
            // Mathf.Sin and Mathf.Cos takes value in radians, so conversion from degrees to radians is required to get desired acceleration values
            velocity_x = shotSpeed * Mathf.Cos(rotationAngle * Mathf.PI/180);
            velocity_y = shotSpeed * Mathf.Sin(rotationAngle * Mathf.PI/180);
        }
        rigidBody.velocity = new Vector3(velocity_x, velocity_y, 0.0f);
        
        // Set rotation of object
        transform.rotation = Quaternion.identity;   // Reset rotation to default (0,0,0)
        if(rotationAngle != 0) {
            switch(rotationAxis) {
                case Axis.X:
                    transform.Rotate (new Vector3 (rotationAngle, 0, 0));
                    break;
                case Axis.Y:
                    transform.Rotate (new Vector3 (0, rotationAngle, 0));
                    break;
                case Axis.Z:
                default:
                    transform.Rotate (new Vector3 (0, 0, rotationAngle));
                    break;
            }
        }
    }


    // If called, disable and return to ShotPool
    // Only intended to be called if attack does not persist on screen after hitting target (e.g. lasers or special weapons)
    void ReturnToShotPool() {
        gameObject.transform.position = shotPool_location.position;     // Send shot object back to where shot pool is located
        gameObject.SetActive(false);                                    // Deactivate shot
    }

    // Set animation of shot hit effect
    void PlaceShotHitFX() {
        for(int i = 0; i < shotHitPool.objects.Count; i++) {
            if(shotHitPool.objects[i].activeInHierarchy == false) {
                shotHitPool.objects[i].SetActive(true);
                shotHitPool.objects[i].transform.position = gameObject.transform.position;
                shotHitPool.objectAnimator[i].SetTrigger("ShotHit");
                break;
            }
        }
    }
}
