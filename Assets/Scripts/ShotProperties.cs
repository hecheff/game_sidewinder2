using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotProperties : MonoBehaviour
{
    public Rigidbody rigidBody;             // Rigidbody for current shot fired
    public float shotSpeed = 20.0f;         // Flight speed of shot
    //public ParticleSystem shotParticles;

    private Transform shotPool_location;     // ShotPool object location (to allow expended shots to return)
    private ObjectPool shotHitPool;          // Shot hit pool (called immediately after )
    //public GameObject contactExplosion;

    void Awake() {
        shotPool_location = transform.parent.transform;
        shotHitPool = GameObject.Find("PlayerShotHit_Pool").GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.velocity = new Vector3(shotSpeed, 0.0f, 0.0f);
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
        
        /*
        if(other.bounds.Contains(this.transform.position)) {
            if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
                PlaceShotHitFX();
                ReturnToShotPool();
            }
        }*/
    }

    // If the bullet leaves boundary, disable and return to ShotPool
    void ReturnToShotPool() {
        gameObject.transform.position = shotPool_location.position;     // Send shot object back to where shot pool is located
        
        //shotParticles.transform.parent = null;
        //shotParticles.enableEmission = false;
        
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
