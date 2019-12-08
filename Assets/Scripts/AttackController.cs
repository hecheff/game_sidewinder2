using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    [Header("Basic Shots")]
    public Transform    shotOrigin;
    public ObjectPool   shotPool_shots;
    public Animator shotAnimation;
    
    [Header("Missile")]
    public ObjectPool   missilePool;
    public MissilePattern missilePattern = MissilePattern.TwoWay;
    public bool missilePool_initRun_up      = false;                // Bool flag for checking if missile object already initialized
    public bool missilePool_initRun_down    = false;                // Bool flag for checking if missile object already initialized

    [Header("Laser")]
    public LaserController laserController;
    
    public void FireWeapon(int type) {
        // Main Weapon
        switch(type) {
            case 1:     // Laser
                laserController.gameObject.SetActive(true);
                laserController.FireLaser();
                break;

            case 2:     // Ripple
                
                break;

            case 0:     // For normal attack
            default:
                for(int i = 0; i < shotPool_shots.objects.Count; i++) {
                    GameObject currentShot = shotPool_shots.objects[i];
                    if(!currentShot.activeInHierarchy) {
                        currentShot.transform.position = shotOrigin.position;
                        currentShot.SetActive(true);
                        SetFiringAnimation();

                        // Assign effect from effects pool to attack shot
                        // Foreach deprecated due to being slower than For loop in context of handling List contents
                        if(shotPool_shots.effectsObjectPool) {
                            //foreach(GameObject effectObject in shotPool_shots.effectsObjectPool.objects) {
                            for(int j = 0; j < shotPool_shots.effectsObjectPool.objects.Count; j++) {
                                GameObject currentShotEffect = shotPool_shots.effectsObjectPool.objects[j];
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
                break;
        }
    }
    
    // Fire missiles. Higher level means faster travel speed
    public void FireMissiles(int currLevel) {
        // Missiles
        MissileController currentMissile_up     = missilePool.objects[0].GetComponent<MissileController>();
        MissileController currentMissile_down   = missilePool.objects[1].GetComponent<MissileController>();

        if(!currentMissile_up.gameObject.activeInHierarchy && Time.time >= currentMissile_up.fireAgainTiming) {
            currentMissile_up.missilePattern = missilePattern;
            currentMissile_up.shotSpeed = 5.0f * currLevel;
            currentMissile_up.rotationStartingAngle = 30;
            currentMissile_up.rotationLimit = 70;
            currentMissile_up.twoWay_isUp = true;
            currentMissile_up.InitializeMissile();
            currentMissile_up.gameObject.SetActive(true);
        }

        if(!currentMissile_down.gameObject.activeInHierarchy && Time.time >= currentMissile_down.fireAgainTiming) {
            currentMissile_down.missilePattern = missilePattern;
            currentMissile_down.shotSpeed = 5.0f * currLevel;
            currentMissile_down.rotationStartingAngle = -30;
            currentMissile_down.rotationLimit = 70;
            currentMissile_down.twoWay_isUp = false;
            currentMissile_down.InitializeMissile();
            currentMissile_down.gameObject.SetActive(true);
        }

        // Version 1: Deprecate due to priority bug causing up-firing missiles to always shoot upwards
        /*
        for(int i = 0; i < missilePool.objects.Count; i++) {
            MissileController currentMissile_up = missilePool.objects[i].GetComponent<MissileController>();
            if(!currentMissile_up.gameObject.activeInHierarchy) {
                currentMissile_up.missilePattern = missilePattern;
                currentMissile_up.rotationStartingAngle = 30;
                currentMissile_up.twoWay_isUp = true;
                currentMissile_up.InitializeMissile();
                currentMissile_up.gameObject.SetActive(true);
                break;
            }
        }
        for(int i = 0; i < missilePool.objects.Count; i++) {
            MissileController currentMissile_down = missilePool.objects[i].GetComponent<MissileController>();
            if(!currentMissile_down.gameObject.activeInHierarchy) {
                currentMissile_down.missilePattern = missilePattern;
                currentMissile_down.rotationStartingAngle = -30;
                currentMissile_down.twoWay_isUp = false;
                currentMissile_down.InitializeMissile();
                currentMissile_down.gameObject.SetActive(true);
                break;
            }
        }
        */
    }

    // Set trigger to play firing animation
    public void SetFiringAnimation() {
        shotAnimation.gameObject.SetActive(true);
        shotAnimation.SetTrigger("FireShot");
    }

    // Resets animation sequence of laser.
    // Currently triggered upon player death.
    public void ResetLaser() {
        laserController.ResetLaser();
    }
}
