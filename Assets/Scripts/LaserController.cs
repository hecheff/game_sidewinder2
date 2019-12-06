using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Laser weapon used by player character
public class LaserController : MonoBehaviour {
    public Transform laserPivot;                        // Attack origin point (shot spawn source)
    private ShotSpawnController laserPivot_controller;  // Controller script for shot spawn (set upon start)

    public ParticleSystem laserParticleEmitter;         // Laser Particle effect emitter

    public float startingLength = 1.0f;
    
    public float    laserSpeed      = 18.0f;
    public float    firingDuration  = 1.0f;             // Amount of time laser is fired from 
    public Axis     rotationAxis    = Axis.Z;
    public float    rotationAngle   = 0.0f;

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

    // Raycasting experiment to replace previous collider approach
    private Ray ray;
    private RaycastHit hitInfo;

    public LayerMask mask_1;
    public LayerMask mask_2;

    //public Vector3 laser_currentDirection;

    void Awake() {
        shotHitPool = GameObject.Find("PlayerShotHit_Pool").GetComponent<ObjectPool>();
        
        capsuleCollider = transform.GetComponent<CapsuleCollider>();
        spriteRenderer  = transform.GetComponent<SpriteRenderer>();
        animator        = transform.GetComponent<Animator>();

        centerDifference = (spriteRenderer.size.x - capsuleCollider.height)/2;
        spriteRendererPivot = spriteRenderer.sprite.pivot;

        CalculateReferenceValues();
        laserParticleEmitter.Stop();

        //laser_currentDirection = transform.right;
    }

    // Start is called before the first frame update
    void Start() {
        laserPivot_controller   = laserPivot.gameObject.GetComponent<ShotSpawnController>();

        spriteRenderer.size = new Vector2(startingLength, spriteRenderer.size.y);

        capsuleCollider.center = laserCenter;
        capsuleCollider.height = spriteRenderer.size.x - centerDifference*2;

        SetLaserEmitter();
    }

    void Update() { 
        /*
        if(Input.GetButton("Fire3")) { 
            var val = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * 180 / Mathf.PI;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0,val));
            laser_currentDirection = new Vector3(0,val,0);
        }
        */
        if(attack_isFiring) {
            transform.position = laserPivot.position;
        }
    }

    public RaycastHit[] hits;   // Registers all hits on raycast (uses RaycastAll due to object interception issue that prevents updating length)
    
    public RaycastHit[] hits_depthCheck;  // Raycast to check if laser hits within a collider (using Z-axis check)

    bool CheckIfInsideCollider() {
        hits_depthCheck = Physics.RaycastAll(laserPivot.position + new Vector3(centerDifference,0,0), transform.forward, 1.0f);
        //Debug.DrawLine(transform.position, transform.position + new Vector3(0,0,1.0f), Color.red);
        if(hits_depthCheck.Length > 0) {
            for(int i = 0; i < hits_depthCheck.Length; i++) {
                if(hits_depthCheck[i].transform.gameObject.tag == "Enemy" || hits_depthCheck[i].transform.gameObject.tag == "Stage") {
                    return true;
                }
            }
        }
        return false;
    }

    public Vector3 raycast_direction;

    void FixedUpdate() {
        // Actions to perform while attack is being fired (no action taken when attack)
        if(attack_isFiring) {
            CalculateReferenceValues();

            // Update list of objects hit by raycast per frame
            raycast_direction = transform.right;
            hits = Physics.RaycastAll(laserPivot.position + new Vector3(centerDifference,0,0), raycast_direction, capsuleCollider.height);
            
            // If laser attack source is currently not inside another collider, check raycast as normal
            // Otherwise, keep length of laser to minimum
            if(!CheckIfInsideCollider()) {
                if(hits.Length > 0) {
                    for(int i = 0; i < hits.Length; i++) {
                        if(hits[i].transform.gameObject.tag == "Enemy" || hits[i].transform.gameObject.tag == "Stage") {
                            capsuleCollider.height = hits[i].point.x - laserPivot.position.x;
                            if(capsuleCollider.enabled) { PlaceShotHitFX(); }
                            break;
                        }
                        capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
                    }
                } else {
                    capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
                }
            } else {
                ResetLaserLength();
                if(capsuleCollider.enabled) { PlaceShotHitFX(); }
            }
            Debug.DrawLine(transform.position, transform.position + raycast_direction*capsuleCollider.height, Color.red);

            capsuleCollider.center = laserCenter;
            spriteRenderer.size = new Vector2(capsuleCollider.height + centerDifference*2, spriteRenderer.size.y);

            SetLaserEmitter();  // Set particle emitter to travel on tip of beam
        }
    }

    /*
    void OnTriggerEnter(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            //PlaceShotHitFX();
            //laser_isExtend = false;
        }
    }
    void OnTriggerStay(Collider other) {
        // If bullet collides with enemy or terrain
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            PlaceShotHitFX();
            //laser_isExtend = false;
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Enemy") || other.CompareTag("Stage")) {
            //PlaceShotHitFX();
            //laser_isExtend = true;
        }
    }
    */

    /*
    void OnCollisionEnter(Collision collision) {
        Debug.Log("[Laser] Collision enter confirmed.");
        if((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Stage")) {
            PlaceShotHitFX();
            laser_isExtend = false;
        }
    }

    void OnCollisionExit(Collision collision) {
        Debug.Log("[Laser] Collision exit confirmed.");
        if((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Stage")) {
            Debug.Log("[Laser] Collision exit confirmed.");
            PlaceShotHitFX();
            laser_isExtend = true;
        }
    }

    void OnCollisionStay(Collision collision) {
        Debug.Log("[Laser] Collision stay confirmed.");
        // If bullet collides with enemy or terrain
        if((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "Stage")) {
            PlaceShotHitFX();
            laser_isExtend = false;
        }
    }
    */
    
    public void FireLaser() {
        //laser_isExtend = true;
        
        ResetLaserLength();
        SetLaserEmitter();
        animator.SetTrigger("AttackStart");
        ToggleEmission(true);
    }

    public void ResetLaserLength() {
        spriteRenderer.size = new Vector2(startingLength, spriteRenderer.size.y);
        capsuleCollider.center = laserCenter;
        capsuleCollider.height = spriteRenderer.size.x - centerDifference*2;
    }

    public void ResetLaser() {
        animator.SetTrigger("PlayerDie");
        ToggleEmission(false);
    }

    // [WIP] Update speed and angle of shot fired
    void SetLaserValues() {
        
    }
    
    // Set animation of shot hit effect
    void PlaceShotHitFX() {
        for(int i = 0; i < shotHitPool.objects.Count; i++) {
            if(shotHitPool.objects[i].activeInHierarchy == false) {
                var impactCoordinates = laserTip - new Vector3(centerDifference*3,0,0);

                shotHitPool.objects[i].SetActive(true);
                shotHitPool.objects[i].transform.position = impactCoordinates; // Manual adjustment to make impact effect line up with laser tip visual
                
                shotHitPool.SetAngleOfEffect(i, Axis.Z, shotHitPool.objects[i].transform); // Set tilt angle of impact effect based on angle of bullet fired
                
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




    // DEPRECATED
    /*
    void FixedUpdate() {
        // DEPRECATED METHOD 4
            // Beam stops when fired from inside a collider, but inside collider priority should be highest
            hits = Physics.RaycastAll(laserPivot.position + new Vector3(centerDifference,0,0), transform.right, capsuleCollider.height);
            if(hits.Length > 0) {
                for(int i = 0; i < hits.Length; i++) {
                    if(hits[i].transform.gameObject.tag == "Enemy" || hits[i].transform.gameObject.tag == "Stage") {
                        capsuleCollider.height = hits[i].point.x - laserPivot.position.x;
                        if(capsuleCollider.enabled) { PlaceShotHitFX(); }
                        break;
                    }
                    capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
                }
            } else {
                // If firing point collider is inside another collider (enemy or stage), capsule length to initial
                // Otherwise allow laser to continue to extend
                if(laserPivot_controller.isInsideCollider) {
                    ResetLaserLength();
                    if(capsuleCollider.enabled) { PlaceShotHitFX(); }
                } else {
                    capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
                }
            }
            capsuleCollider.center = laserCenter;
            spriteRenderer.size = new Vector2(capsuleCollider.height + centerDifference*2, spriteRenderer.size.y);
            */

            // DEPRECATED METHOD 3
            // Beam travels through objects if fired from inside a collider
            /*
            hits = Physics.RaycastAll(laserPivot.position + new Vector3(centerDifference,0,0), transform.right, capsuleCollider.height);
            if(hits.Length > 0) {
                for(int i = 0; i < hits.Length; i++) {
                    if(hits[i].transform.gameObject.tag == "Enemy" || hits[i].transform.gameObject.tag == "Stage") {
                        capsuleCollider.height = hits[i].point.x - laserPivot.position.x;
                        if(capsuleCollider.enabled) { PlaceShotHitFX(); }
                        break;
                    }
                    capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
                }
            } else {
                capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
            }
            capsuleCollider.center = laserCenter;
            spriteRenderer.size = new Vector2(capsuleCollider.height + centerDifference*2, spriteRenderer.size.y);
            */

            // DEPRECATED METHOD 2
            // Length of beam does not catch closest affected target if instant switched.
            /*
            ray = new Ray(laserPivot.position + new Vector3(centerDifference,0,0), transform.right);
            if(
                Physics.Raycast(ray, out hitInfo, capsuleCollider.height, mask_1, QueryTriggerInteraction.Collide) ||
                Physics.Raycast(ray, out hitInfo, capsuleCollider.height, mask_2, QueryTriggerInteraction.Collide)
            ) {
                if(hitInfo.transform.gameObject.tag == "Enemy" || hitInfo.transform.gameObject.tag == "Stage") {
                    // If raycast come into contact with target (enemy or stage element), set length to closest target
                    capsuleCollider.height = hitInfo.point.x - laserPivot.position.x;
                    if(capsuleCollider.enabled) { PlaceShotHitFX(); }
                    Debug.Log("[" + gameObject.name + "] HIT");
                } else {
                    // Otherwise, continue to extend laser
                    capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
                    Debug.Log("[" + gameObject.name + "] EXTEND");
                }
            } else {
                // If raycast yields no results, continue to extend laser
                capsuleCollider.height = capsuleCollider.height + laserSpeed/GlobalController.Instance.targetFrameRate;
                Debug.Log("[" + gameObject.name + "] NO CONTACT");
            }
            capsuleCollider.center = laserCenter;
            spriteRenderer.size = new Vector2(capsuleCollider.height + centerDifference*2, spriteRenderer.size.y);
            */

            // DEPRECATED METHOD 1
            // Goes through everything, does not stop when hitting solid targets or stage
            /*
            if(Physics.Raycast(ray, out hitInfo, spriteRenderer.size.x)) {
                if(hitInfo.transform.gameObject.tag == "Enemy" || hitInfo.transform.gameObject.tag == "Stage") {
                    
                    Vector2 target = new Vector3(hitInfo.transform.position.x, transform.position.y);
                    Vector2 origin = new Vector3(transform.position.x, transform.position.y);

                    spriteRenderer.size = target - origin;
                    
                    //currentLength = hitInfo.point.x - transform.position.x;
                    Debug.DrawLine(transform.position, hitInfo.point, Color.green); 
                } else {
                    spriteRenderer.size = new Vector2(spriteRenderer.size.x + laserSpeed/GlobalController.Instance.targetFrameRate, spriteRenderer.size.y);

                    Debug.DrawLine(transform.position, transform.position + new Vector3(spriteRenderer.size.x,0,0), Color.red);
                    //currentLength += laserSpeed;
                }
            } else {
                spriteRenderer.size = new Vector2(spriteRenderer.size.x + laserSpeed/GlobalController.Instance.targetFrameRate, spriteRenderer.size.y);
                
                Debug.DrawLine(transform.position, transform.position + new Vector3(spriteRenderer.size.x,0,0), Color.red);
                //currentLength += laserSpeed;
            }

            // Adjust capsule collider according to length (height) of laser
            capsuleCollider.center = laserCenter;
            capsuleCollider.height = spriteRenderer.size.x - centerDifference*2;
    }
    */
}
