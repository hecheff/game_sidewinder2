using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    public Transform    optionPivot;
    public Vector3      positionFromPivot;
    
    public Transform    shotSpawn;
    public ObjectPool   shotPool_shots;

    public bool followPlayer;

    // Update is called once per frame
    void Update() {
        if(followPlayer) {
            transform.position = optionPivot.position + positionFromPivot;
        }
    }

    public void FireWeapon() {
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
}
