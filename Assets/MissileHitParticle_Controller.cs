using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHitParticle_Controller : MonoBehaviour {
    public MissileController        currentMissile;
    public ReturnToInitialPosition  returnToInitialPosition;
    public ParticleSystem           particleSys;

    private float lifetime;                 // Variable to check active time of particle emitter
    private float currentTime;              // Countup to object lifetime before returning to original position

    void Start() {
        //lifetime = particleSys.main.duration + particleSys.main.startLifetime.constant;
    }

    void Update() {
        if(Time.time >= (currentTime + lifetime)) {
            returnToInitialPosition.ResetPosition();

            // Disable current missile after effect finishes playing.
            currentMissile = null;
            
            gameObject.SetActive(false);
        }
    }

    public void ResetEffect(MissileController newMissile) {
        currentMissile = newMissile;

        currentTime = Time.time;
        lifetime = particleSys.main.duration + particleSys.main.startLifetime.constant;
        newMissile.fireAgainTiming = Time.time + lifetime;
    }
}