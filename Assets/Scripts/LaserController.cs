using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    Laser weapon used by player character

    Algorithm of operation:
    1. Player fires 
*/ 
public class LaserController : MonoBehaviour
{
    public Transform laserPivot;

    public float startingLength = 1.0f;
    
    public float laserSpeed = 18.0f;
    public float firingDuration = 1.0f;     // Amount of time laser is fired from 

    public  ObjectPool      shotHitPool;

    private CapsuleCollider capsuleCollider;
    private SpriteRenderer  spriteRenderer;
    private Animator        animator;

    private float centerDifference;
    private Vector2 spriteRendererPivot;

    public bool attack_isFiring = false;

    void Awake() {
        shotHitPool = GameObject.Find("PlayerShotHit_Pool").GetComponent<ObjectPool>();
        
        capsuleCollider = transform.GetComponent<CapsuleCollider>();
        spriteRenderer  = transform.GetComponent<SpriteRenderer>();
        animator        = transform.GetComponent<Animator>();

        centerDifference = (spriteRenderer.size.x - capsuleCollider.height)/2;
        spriteRendererPivot = spriteRenderer.sprite.pivot;
    }

    // Start is called before the first frame update
    void Start() {
        spriteRenderer.size = new Vector2(startingLength, spriteRenderer.size.y);
    }

    void FixedUpdate() {
        if(attack_isFiring) {
            transform.position = laserPivot.position;

            // Update laser length
            spriteRenderer.size = new Vector2(spriteRenderer.size.x + laserSpeed/GlobalController.Instance.targetFrameRate, spriteRenderer.size.y);

            // Adjust capsule collider according to length (height) of laser
            capsuleCollider.center = new Vector3(spriteRendererPivot.x + centerDifference + ((spriteRenderer.size.x - centerDifference*2)/2),0,0);
            capsuleCollider.height = spriteRenderer.size.x - centerDifference*2;
        }
    }
    
    void OnTriggerEnter(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
        }
    }

    void OnTriggerStay(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
            Debug.Log("[STAY] Stage hit.");
        }
    }

    public void FireLaser() {
        spriteRenderer.size = new Vector2(startingLength, spriteRenderer.size.y);
        animator.SetTrigger("AttackStart");
    }

    public void ResetLaser() {
        animator.SetTrigger("PlayerDie");
    }

    // Set animation of shot hit effect
    void PlaceShotHitFX() {
        for(int i = 0; i < shotHitPool.objects.Count; i++) {
            if(shotHitPool.objects[i].activeInHierarchy == false) {
                shotHitPool.objects[i].SetActive(true);
                shotHitPool.objects[i].transform.position = gameObject.transform.position + new Vector3(spriteRenderer.size.x - centerDifference*2,0,0);
                shotHitPool.objectAnimator[i].SetTrigger("ShotHit");
                break;
            }
        }
    }
}
