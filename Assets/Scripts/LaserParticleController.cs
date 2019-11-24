using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller for particle effects used for player beam attack
public class LaserParticleController : MonoBehaviour {
    public LaserController laserController;

    void Update() {
        transform.position = laserController.gameObject.transform.position;
    }
}
