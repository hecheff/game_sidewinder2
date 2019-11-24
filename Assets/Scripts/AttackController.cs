using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {
    public Transform    shotOrigin;
    public ObjectPool   shotPool_shots;
    
    public LaserController laserController;
    
    public void FireWeapon(int type) {
        GameObject currentShot;
        GameObject currentShotEffect;

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
                    currentShot = shotPool_shots.objects[i];
                    if(!currentShot.activeInHierarchy) {
                        currentShot.transform.position = shotOrigin.position;
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
                break;
        }
    }
    
    // Resets animation sequence of laser.
    // Currently triggered upon player death.
    public void ResetLaser() {
        laserController.ResetLaser();
    }
}
