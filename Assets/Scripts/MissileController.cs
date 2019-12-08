using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {
    public bool updateOverride = false;

    public MissilePattern missilePattern = MissilePattern.TwoWay;

    public float    shotSpeed               = 10.0f;        // Flight speed of shot    
    public Axis     rotationAxis            = Axis.Z;       // Axis of rotation
    public float    rotationStartingAngle   = 0.0f;         // Starting Rotation of missile (counter-clockwise from right = 0 degrees)
    public float    rotationSpeed           = 30.0f;        // Speed of rotation per second
    public float    rotationLimit           = 90.0f;        // Rotation limit of missile
    
    public Transform    missileSpawnPoint;
    public Vector3      spawnPointDelta         = new Vector3(0.0f, 0.1f, 0.0f);

    private Transform   missilePool_location;               // Missile Pool object location (to allow expended missiles to return)
    private ObjectPool  missileHitPool_normal;             // Missile hit pool (called immediately)

    public float        fireAgainTiming = 0.0f;             // Can fire shot again after time has passed

    // For two-way patterns
    public bool     twoWay_isUp = false;    

    private Rigidbody rigidBody;                            // Rigidbody for current shot fired. Only used for kinetic-fired weapons.

    void Awake() {
        rigidBody = transform.GetComponent<Rigidbody>();
        InitializeMissile();
        
        missilePool_location = transform.parent.transform;
        missileHitPool_normal = GameObject.Find("PlayerMissileHitNormal_Pool").GetComponent<ObjectPool>();
    }

    void Start() {
        
    }

    void Update() {
        if(updateOverride) {
            InitializeMissile();
            updateOverride = false;
        }

        switch(missilePattern) {
            case MissilePattern.SpreadBomb:
                break;

            case MissilePattern.TwoWay_Back:
                UpdateAngle_TwoWay();
                break;
            
            case MissilePattern.TwoWay:
            default:
                UpdateAngle_TwoWay();
                break;
        }
        rigidBody.velocity = transform.right * shotSpeed;
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
            PlaceMissileHitFX_Normal();
            ReturnToShotPool();
        }
    }

    void OnTriggerStay(Collider other) {
        // If bullet is within enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceMissileHitFX_Normal();
            ReturnToShotPool();
        }
    }



    // Set starting angle of missile using rotation
    public void InitializeMissile() {
        float rotation = rotationStartingAngle;
        
        // Check if missile is firing backwards
        // If true, mirror on Y-axis. Otherwise, treat as normal.
        if(missilePattern == MissilePattern.TwoWay_Back) {
            rotation = OutputMirroredAngle(rotationStartingAngle);
        }

        transform.rotation = Quaternion.identity;   // Reset transformation before deployment
        switch(rotationAxis) {
            case Axis.X:
                transform.Rotate(new Vector3(rotation, 0, 0));
                break;
            case Axis.Y:
                transform.Rotate(new Vector3(0, rotation, 0));
                break;
            case Axis.Z:
            default:
                transform.Rotate(new Vector3(0, 0, rotation));
                break;
        }

        // Set spawn point of missile
        if(missilePattern == MissilePattern.TwoWay || missilePattern == MissilePattern.TwoWay_Back) {
            if(twoWay_isUp) {
                transform.position = missileSpawnPoint.position + spawnPointDelta;
            } else {
                transform.position = missileSpawnPoint.position - spawnPointDelta;
            }
        }
        fireAgainTiming = Time.time + 1.0f;
    }

    float OutputMirroredAngle(float inputAngle) {
        if(inputAngle > 0 && inputAngle < 90) {
            return (180 - inputAngle);
        } else if(inputAngle < 0 && inputAngle > -90) {
            return (180 + Mathf.Abs(inputAngle));
        }
        return inputAngle;
    }

    // Update current angle of two-way missile
    void UpdateAngle_TwoWay() {
        int multiplier = 1;
        if(!twoWay_isUp) { multiplier = -1; }
        
        // Determine rotation value
        float newAngleDelta = (rotationSpeed/GlobalController.Instance.targetFrameRate) * multiplier;
        float newRotationLimit = rotationLimit;

        // Determine which axis to update
        switch(rotationAxis) {
            case Axis.X:
                if(CheckIfNextAngleMax_TwoWay(transform.eulerAngles.x, newAngleDelta)) {
                    transform.eulerAngles = new Vector3(rotationLimit, 0, 0);
                } else {
                    transform.Rotate(new Vector3(newRotationLimit, 0, 0));
                }
                break;
            case Axis.Y:
                if(CheckIfNextAngleMax_TwoWay(transform.eulerAngles.y, newAngleDelta)) {
                    transform.eulerAngles = new Vector3(0, rotationLimit, 0);
                } else {
                    transform.Rotate(new Vector3(0, newRotationLimit, 0));
                }
                break;

            case Axis.Z:
            default:
                if(CheckIfNextAngleMax_TwoWay(transform.eulerAngles.z, newAngleDelta)) {
                    if(!twoWay_isUp) { newRotationLimit = -newRotationLimit; }
                    transform.eulerAngles = new Vector3(0, 0, newRotationLimit);
                } else {
                    if(missilePattern == MissilePattern.TwoWay_Back) {
                        newAngleDelta = -newAngleDelta;
                    }
                    transform.Rotate(new Vector3(0, 0, newAngleDelta));
                }
                break;
        }
    }

    // For UpdateAngle_TwoWay: Return true/false to check if angle will hit/exceed maximum rotation based on next calculated value 
    // Takes euler angles, meaning value of currentAngle will always be within 0 and 360 (inclusive), i.e. no negative values
    bool CheckIfNextAngleMax_TwoWay(float currentAngle, float newAngleDelta) {
        if(twoWay_isUp) {
            if(missilePattern == MissilePattern.TwoWay_Back) {
                if((currentAngle + newAngleDelta) <= (180 - rotationLimit) && currentAngle != 180) {
                    return true;
                }
            } else {
                if(((currentAngle + newAngleDelta) >= rotationLimit) && currentAngle != 0) {
                    return true;
                }
            }
        } else {
            if(missilePattern == MissilePattern.TwoWay_Back) {
                if((currentAngle + newAngleDelta) >= (180 + rotationLimit) && currentAngle != 180) {
                    return true;
                }
            } else {
                //Debug.Log((currentAngle + newAngleDelta) + " <= " + (360 - rotationLimit));
                if((currentAngle + newAngleDelta) <= (360 - rotationLimit) && (currentAngle != 0)) {
                    return true;
                }
            }
        }
        return false;
    }

    // Set missile effect and bind current missile to the effect (to set disable trigger)
    void PlaceMissileHitFX_Normal() {
        for(int i = 0; i < missileHitPool_normal.objects.Count; i++) {
            if(missileHitPool_normal.objects[i].activeInHierarchy == false) {
                missileHitPool_normal.objects[i].GetComponent<MissileHitParticle_Controller>().ResetEffect(this);
                missileHitPool_normal.objects[i].SetActive(true);
                //missileHitPool_normal.SetAngleOfEffect(i, rotationAxis, transform); // Set tilt angle of impact effect based on angle of bullet fired
                missileHitPool_normal.objects[i].transform.position = gameObject.transform.position;
                //missileHitPool_normal.objectAnimator[i].SetTrigger("ShotHit");
                break;
            }
        }
    }

    // If called, return to ShotPool
    void ReturnToShotPool() {
        gameObject.transform.position = missilePool_location.position;      // Send shot object back to where shot pool is located
        gameObject.SetActive(false);                                      // Deactivate shot
    }
}
