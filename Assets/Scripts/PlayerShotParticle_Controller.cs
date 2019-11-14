using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Follows assigned player attack when active, kept outside of screen when inactive
public class PlayerShotParticle_Controller : MonoBehaviour
{
    public Transform currentPlayerShot;     // Player shot to follow
    public ReturnToInitialPosition returnToInitialPosition;
    public ParticleSystem particleSys;
    
    private float lifetime;                 // Variable to check active time of particle emitter
    private float currentTime;              // Countup to object lifetime before returning to original position

    // Start is called before the first frame update
    void Awake()
    {
        lifetime = particleSys.main.duration + particleSys.main.startLifetime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        // When particles are still active
        if(Time.time < (currentTime + lifetime)) {
            // If current playershot is active and not null, follow player shot and emit particles
            if((currentPlayerShot != null) && (currentPlayerShot.gameObject.activeInHierarchy)) {
                transform.position = currentPlayerShot.position;
            } else {
                ToggleEmission(false);
            }
        } else {
            // When time over
            currentPlayerShot = null;
            returnToInitialPosition.ResetPosition();
            gameObject.SetActive(false);
        }
    }

    // If true, set object to follow assigned shot, otherwise stop following
    public void SetFollowPlayerStatus(Transform targetPlayerShot) {
        ToggleEmission(true);
        currentPlayerShot = targetPlayerShot;
        currentTime = Time.time;
    }

    void ToggleEmission(bool enable) {
        particleSys.enableEmission = enable;
    }
}
