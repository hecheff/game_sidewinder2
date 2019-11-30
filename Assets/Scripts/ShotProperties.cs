using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotProperties : MonoBehaviour {    
    public float    shotSpeed           = 20.0f;    // Flight speed of shot

    // Rotation properties of projectile
    public Axis     rotationAxis        = Axis.X;

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

    // Start is called before the first frame update
    void Start() {
        if(shotSpeed != 0) {
            rigidBody.velocity = new Vector3(shotSpeed, 0.0f, 0.0f);
        }
    }

    void Update() {
        /*
        if(rotationRightLimit != 0) {
            if(transform.rotation.z < rotationRightLimit) {
                switch(rotationAxis) {
                    case Axis.X:
                        transform.Rotate (new Vector3 (rotationRightSpeed, 0, 0) * Time.deltaTime);
                        break;

                    case Axis.Y:
                        transform.Rotate (new Vector3 (0, rotationRightSpeed, 0) * Time.deltaTime);
                        break;

                    case Axis.Z:
                        transform.Rotate (new Vector3 (0, 0, rotationRightSpeed) * Time.deltaTime);
                        break;
                }
            }
        }
        */
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
