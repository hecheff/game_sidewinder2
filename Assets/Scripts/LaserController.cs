using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Laser weapon used by player character
public class LaserController : MonoBehaviour {
    public Transform laserPivot;                // AttackSource
    public ParticleSystem laserParticleEmitter;  // Laser Particle effect emitter

    public float startingLength = 1.0f;
    
    public float laserSpeed = 18.0f;
    public float firingDuration = 1.0f;     // Amount of time laser is fired from 

    public  ObjectPool      shotHitPool;

    private CapsuleCollider capsuleCollider;
    private SpriteRenderer  spriteRenderer;
    private Animator        animator;

    // Static Values (only need to calculate at start)
    private float centerDifference;             //  
    private Vector2 spriteRendererPivot;        // Sprite render pivot point (where beam originates rom)

    // Constantly changing values calculated for references (e.g. particle effect calculations)
    public Vector3  laserCenter;                // Center point of beam (also capsule collider)
    public Vector3  laserTip;                   // Beam tip (longest distance from firing source)

    // Flags for checking status
    public bool attack_isFiring = false;        // When laser is being fired (disabled when in standby)
    public bool laser_isExtend  = true;         // When laser is currently being extended


    void Awake() {
        shotHitPool = GameObject.Find("PlayerShotHit_Pool").GetComponent<ObjectPool>();
        
        capsuleCollider = transform.GetComponent<CapsuleCollider>();
        spriteRenderer  = transform.GetComponent<SpriteRenderer>();
        animator        = transform.GetComponent<Animator>();

        centerDifference = (spriteRenderer.size.x - capsuleCollider.height)/2;
        spriteRendererPivot = spriteRenderer.sprite.pivot;

        CalculateReferenceValues();
        laserParticleEmitter.Stop();
    }

    // Start is called before the first frame update
    void Start() {
        spriteRenderer.size = new Vector2(startingLength, spriteRenderer.size.y);
        SetLaserEmitter();
    }

    void FixedUpdate() {
        // Actions to perform while attack is being fired (no action taken when attack)
        if(attack_isFiring) {
            CalculateReferenceValues();
            transform.position = laserPivot.position;

            // Update laser length when not hitting any enemies
            // INCOMPLETE: Needs detection fix for when enemy collider vanishes when still active
            if(laser_isExtend) {
                spriteRenderer.size = new Vector2(spriteRenderer.size.x + laserSpeed/GlobalController.Instance.targetFrameRate, spriteRenderer.size.y);
            }

            // Adjust capsule collider according to length (height) of laser
            capsuleCollider.center = laserCenter;
            capsuleCollider.height = spriteRenderer.size.x - centerDifference*2;

            // Set particle emitter to travel on tip of beam
            SetLaserEmitter();
        }
    }

    void OnTriggerEnter(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
            laser_isExtend = false;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
            laser_isExtend = true;
        }
    }

    void OnTriggerStay(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
            //laser_isExtend = false;
        }
    }
    
    public void FireLaser() {
        spriteRenderer.size = new Vector2(startingLength, spriteRenderer.size.y);
        SetLaserEmitter();
        animator.SetTrigger("AttackStart");
        ToggleEmission(true);
        laser_isExtend = true;
    }

    public void ResetLaser() {
        animator.SetTrigger("PlayerDie");
        ToggleEmission(false);
    }

    // Set animation of shot hit effect
    void PlaceShotHitFX() {
        for(int i = 0; i < shotHitPool.objects.Count; i++) {
            if(shotHitPool.objects[i].activeInHierarchy == false) {
                shotHitPool.objects[i].SetActive(true);
                
                shotHitPool.objects[i].transform.position = laserTip - new Vector3(centerDifference*2,0,0); // Manual adjustment to make impact effect line up with laser tip visual
                shotHitPool.objectAnimator[i].SetTrigger("ShotHit");
                break;
            }
        }
    }

    // Sets position of particle emitter for laser
    void SetLaserEmitter() {
        laserParticleEmitter.transform.position = laserTip;
    }

    // Calculate reference Vector3 values (center of laser, length from center, and tip)
    void CalculateReferenceValues() {
        laserCenter = new Vector3(spriteRendererPivot.x + (centerDifference*2) + ((spriteRenderer.size.x - centerDifference*2)/2),0,0);
        laserTip    = gameObject.transform.position + (new Vector3(laserCenter.x,0,0)*2);
    }

    void ToggleEmission(bool enable) {
        laserParticleEmitter.enableEmission = enable;
        if(enabled) {
            laserParticleEmitter.Play();
        }
    }
}
