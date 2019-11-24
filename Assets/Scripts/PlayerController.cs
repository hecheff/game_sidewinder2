using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Establish player movement boundary on-screen
[System.Serializable]
public class Boundary {
    public float xMin, xMax, yMin, yMax;
}


public class PlayerController : MonoBehaviour {
    public bool canControl = true;  // Determine if player can control ship

    public PowerMeterController powerMeterController;
    public AttackController attackController;
    public LaserController laserController;

    public Animator     animator;
    public GameObject   dieExplosion;

    public Boundary     boundary;           // Moveable range boundary (not to be confused with boundary limits for everything else)
    public Rigidbody    rigidbody;
    public Transform    shotSpawn;
    public ObjectPool   shotPool_shots;

    public Animator     powerUpFX_RippleFX;
    public Animator     powerUpFX_0_speedUp_thruster;

    public OptionController[] options;

    // Returns response upon relevant hitbox interacted with
    public PlayerHitbox_PowerUp     collider_powerUp;       // Response to collecting power-up on contact. Hitbox spans dimensions of visible ship.
    public PlayerHitbox_Damage      collider_damage;        // Response when player takes damage. Small hitbox in critical area (cockpit).

    public float    moveSpeed      = 5.0f;          // Max. Initial movement speed
    public float    rateOfFire     = 0.1f;          // Time interval between each shot
    //public float    fireCooldown   = 0.4f;          // 
    //public int      fireBurstCount = 5;             // 

    private float nextFire;

    // Player temp stats
    private bool    attack_isLaser  = false;
    private float   speedUp_boost   = 0;
    private int     optionCount     = 2;

    void Awake() { }

    void Start() { }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Boundary")) {
            // Do nothing
        }

        /* 
        else {
            if(other.CompareTag("PowerUp")) {
                Debug.Log("Power Up!");
                // Do nothing
                powerMeterController.IncrementPowerMeter();
                Destroy(other.gameObject);
            }
        }
        if (GetComponentInChildren<Collider> ().CompareTag ("hitbox_player_item") == true) { 
            // Items
            //Debug.Log (GetComponentInChildren<Collider> ());
            if (other.gameObject.CompareTag ("powerup_1_bronze") || other.gameObject.CompareTag ("powerup_2_silver") || other.gameObject.CompareTag ("powerup_3_gold")) {
                if (other.gameObject.CompareTag ("powerup_1_bronze") == true) {

        
        */
        /* 
        if(other.CompareTag("Enemy")) {

        }
        */
    }

    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Boundary")) {
            // Do nothing
        }
    }

    
    void Update() {
        if(canControl) {
            // Attack Controls
            // NEED TO UPDATE: Attack timing and type depends on current base attack (shot or laser)
            if (Input.GetButton("Fire1")) {
                if(!attack_isLaser) {
                    if(Time.time > nextFire) {
                        Debug.Log("Fire1 pressed");
                        attackController.FireWeapon(0);
                        nextFire = Time.time + rateOfFire;

                        // Fire attacks from Options
                        for(int i = 0; i < optionCount; i++) {
                            options[i].attackController.FireWeapon(0);
                        }
                    }
                } else {
                    // Check if laser is currently in-use
                    if(!attackController.laserController.attack_isFiring) {
                        attackController.FireWeapon(1);
                        
                        // Fire attacks from Options
                        for(int i = 0; i < optionCount; i++) {
                            options[i].attackController.FireWeapon(1);
                        }
                    }
                }
            }

            // Powerup Controls
            if(Input.GetButton("Fire2")) {
                //Debug.Log("Fire2 pressed. Power Meter = " + powerMeterController.currentLitMeter);
                switch(powerMeterController.currentLitMeter) {
                    case 0:         // Speed Up
                        PowerUpFX_0_SpeedUp();
                        break;
                        
                    case 1:         // Missile
                        break;
                        
                    case 2:         // Laser
                        PowerUpFX_2_Laser();
                        break;
                        
                    case 3:         // Charge
                        break;
                        
                    case 4:         // Option
                        break;
                        
                    case 5:         // Shield
                        break;
                    
                    default:        // Do nothing
                    case -1:        
                        break;
                }

                // Reset power meter if power up used (when button pressed when not empty)
                if(powerMeterController.currentLitMeter != -1) {
                    powerUpFX_RippleFX.gameObject.SetActive(true);
                    powerUpFX_RippleFX.SetTrigger("RestartFX");
                    powerMeterController.ResetMeter();
                }
            }
        }
        
        if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Player_SpawnIn") {
            animator.applyRootMotion = true;
        } else {
            animator.applyRootMotion = false;
        }
    }

    void FixedUpdate() {
        if(canControl) {
            //if(!Input.GetButton("Fire3")) { 
            float moveHorizontal = Input.GetAxis("Horizontal") * (moveSpeed + speedUp_boost);
            float moveVertical = Input.GetAxis("Vertical") * (moveSpeed + speedUp_boost);
            
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            rigidbody.velocity = movement;

            SetObjectBoundary();
            MovementTilt();
            //}
        }
    }

    // Return result if currently inside boundary
    // bool CheckObjectWithinBoundary()
    // {
    //     if ((rigidBody.position.x > boundary.xMin && rigidBody.position.x < boundary.xMax) && (rigidBody.position.y > boundary.yMin && rigidBody.position.y < boundary.yMax)) {
    //         return true;
    //     }
    //     return false;
    // }

    // Force object to stay within set boundaries
    void SetObjectBoundary() {
        rigidbody.position = new Vector3
        (
            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rigidbody.position.y, boundary.yMin, boundary.yMax),
            0.0f
        );
    }

    public float    tiltLimit           = 22.5f;    // Max. amount of tilt (max. upward = positive, max. downward = negative)
    public float    tiltSpeed           = 0.5f;     // Speed of tilt change per frame
    private float   tiltCurrent         = 0.0f;     // Current amount of tilt
    private float   tiltResetMultiplier = 4.0f;     // Tilt speed multiplier when resetting to 0

    void MovementTilt() {
        // Update value of tilt based on current vertical input (speed of movement also determined by player max. speed)
        // Check if vertical movement present
        if(Input.GetAxisRaw("Vertical") == 1) {
            // When movement is upward, add to current tilt
            if(tiltCurrent + tiltSpeed * rigidbody.velocity.y >= tiltLimit) {       // Force tilt to upper limit
                tiltCurrent = tiltLimit;
            } else if(tiltCurrent < tiltLimit) {            // Otherwise, increase tilt
                tiltCurrent += tiltSpeed * rigidbody.velocity.y;
            }
        } else if(Input.GetAxisRaw("Vertical") == -1) {
            // When movement is downward, subtract from current tilt
            if(tiltCurrent - tiltSpeed * rigidbody.velocity.y <= -tiltLimit) {      // Force tilt to upper limit
                tiltCurrent = -tiltLimit;
            } else if(tiltCurrent > -tiltLimit) {                                   // Otherwise, decrease tilt
                tiltCurrent -= tiltSpeed * -rigidbody.velocity.y;
            }
        } else {
            // When movement is neutral, return tilt to neutral
            if(tiltCurrent > 0) {                           // Decrease tilt to 0
                if(tiltCurrent - tiltSpeed * tiltResetMultiplier <= 0) {            // Force tilt to 0
                    tiltCurrent = 0;
                } else if(tiltCurrent > 0) {                                        // Otherwise, decrease tilt
                    tiltCurrent -= tiltSpeed * tiltResetMultiplier;
                }
            } else if(tiltCurrent < 0) {                    // Increase tilt to 0
                 if(tiltCurrent + tiltSpeed * tiltResetMultiplier >= 0) {           // Force tilt to 0
                    tiltCurrent = 0;
                } else if(tiltCurrent < 0) {                                        // Otherwise, increase tilt
                    tiltCurrent += tiltSpeed * tiltResetMultiplier;
                }
            } else if(tiltCurrent == 0) {                   // Tilt already at 0
                // Do nothing
            }
        }
        rigidbody.rotation = Quaternion.Euler(tiltCurrent, 0.0f, 0.0f); // Set based on calculated value

        // Problems with original version:
        //  - Tilt is bound to vertical movement magnitude, causing unwanted tilt amount (no limit set) 
        //  - Tilt becomes instant when using gamepad due to lack of acceleration
        //rigidbody.rotation = Quaternion.Euler(rigidbody.velocity.y * 4.5f, 0.0f, 0.0f)
        //Debug.Log(rigidbody.velocity.y * tiltLimit);
    }

    // Enable Speed Up
    public void PowerUpFX_0_SpeedUp() {
        powerUpFX_0_speedUp_thruster.gameObject.SetActive(true);
        powerUpFX_0_speedUp_thruster.SetTrigger("RestartFX");
        speedUp_boost += 1.0f;
    }

    // Enable Laser attack ability
    public void PowerUpFX_2_Laser() {
        // Adding potential increment potential for tiered laser attack system
        //if(!attack_isLaser) {
        attack_isLaser = true;
        //}
    }

    // If player is destroyed by enemy attack or enemy/stage collision
    public void PlayerDie() {
        canControl = false;

        // Play explosion
        Instantiate(dieExplosion, transform.position, transform.rotation);

        // [WIP] If player still has lives, proceed with respawn sequence

        // Set respawn sequence
        animator.SetBool("Player_Respawn", true);   // Set respawn flag in animator to 'true' to allow respawn invincibility sequence upon reentry.
        animator.SetTrigger("Player_WasKilled");    // 
        
        // Cancel laser attack animation sequence
        if(attack_isLaser) {
            attackController.ResetLaser();

            // Cancel for each active Option
            for(int i = 0; i < optionCount; i++) {
                options[i].attackController.ResetLaser();
            }
        }

        // Reset player power ups
        PlayerPowerReset();

        // [WIP] Nullify all enemy attacks for brief amount of time (2-3 seconds after respawn)
        
    }

    // Reset player active power ups
    // Throw any Options forward from player's death spot
    public void PlayerPowerReset() {
        speedUp_boost = 0;                          // Speed boost to 0
        attack_isLaser = false;                     // Laser attack status to false (revert to default attack)
        powerMeterController.ResetMeter_Die();      // Reset power meter (as applicable)
        
        //optionCount = 0;                          // Reset Option count
    }




    // ======================================= DEPRECATED ITEMS =======================================

    // [DEPRECATED] Shot system now part of AttackController (allows sharing of code for Options)
    // Fire basic attack shot
    /*
    void FireShotFromShotPool() {
        GameObject currentShot;
        GameObject currentShotEffect;

        for(int i = 0; i < shotPool_shots.objects.Count; i++) {
            currentShot = shotPool_shots.objects[i];
            if(!currentShot.activeInHierarchy) {
                currentShot.transform.position = shotSpawn.position;
                currentShot.SetActive(true);

                // Assign effect from effects pool to attack shot
                // Foreach deprecated due to being slower than For loop in context of handling List contents
                if(shotPool_shots.effectsObjectPool) {
                    //foreach(GameObject effectObject in shotPool_shots.effectsObjectPool.objects) {
                    for(int j = 0; j < shotPool_shots.effectsObjectPool.objects.Count; j++) {
                        currentShotEffect = shotPool_shots.effectsObjectPool.objects[j];
                        if(!currentShotEffect.activeInHierarchy) {
                            currentShotEffect.transform.GetComponent<PlayerShotParticle_Controller>().SetFollowPlayerStatus(currentShot.transform);
                            currentShotEffect.SetActive(true);
                            break;
                        }
                    }
                }
                break;
            }
        }
    }
    */

    // [DEPRECATED] Cannot be applied due to difference in input value handling by input device (keyboard, gamepad, analog, etc.)
    // Set tilt action applied to object during vertical movement
    /* 
    void MovementTilt() {
        rigidbody.rotation = Quaternion.Euler(rigidbody.velocity.y * tilt, 0.0f, 0.0f); // Set tilt
    }
    */
}