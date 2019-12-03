using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Variable overrides for Attacks
public class AttackCollectionOverride : MonoBehaviour {
    [Header("Basic Shot Properties")]
    public GameObject           basicShot_playerShotPool;                                   // Shot Pool
    public List<ShotProperties> basicShot_shotProperties    = new List<ShotProperties>();   // List of all objects in pool
    public float    basicShot_shotSpeed         = 0.0f;         // Shot velocity
    public Axis     basicShot_rotationAxis      = Axis.Z;       // Rotation Axis 
    public float    basicShot_rotationAngle     = 0.0f;         // Rotation Angle
    private float    ref_basicShot_shotSpeed;
    private Axis     ref_basicShot_rotationAxis;
    private float    ref_basicShot_rotationAngle;

    [Header("Laser Properties")]
    public GameObject laser;

    void Awake() {
        // Populate Basic Shot attributes
        foreach (Transform child in basicShot_playerShotPool.transform) {
            var currShotProperties = child.gameObject.GetComponent<ShotProperties>();
            
            // Set basic properties
            basicShot_shotProperties.Add(currShotProperties);
            basicShot_shotSpeed     = currShotProperties.shotSpeed;
            basicShot_rotationAxis  = currShotProperties.rotationAxis;
            basicShot_rotationAngle = currShotProperties.rotationAngle;
        }
        // Set reference values
        ref_basicShot_shotSpeed     = basicShot_shotSpeed;
        ref_basicShot_rotationAxis  = basicShot_rotationAxis;
        ref_basicShot_rotationAngle = basicShot_rotationAngle;

        // Populate Laser attributes

    }

    void Update() {
        // Update basic shot attributes if change detected
        if(DetectChange_BasicShot()) {
            for(int i = 0; i < basicShot_shotProperties.Count; i++) {
                basicShot_shotProperties[i].shotSpeed       = basicShot_shotSpeed;
                basicShot_shotProperties[i].rotationAxis    = basicShot_rotationAxis;
                basicShot_shotProperties[i].rotationAngle   = basicShot_rotationAngle;
                basicShot_shotProperties[i].updateOverride  = true;
            }
        }
    }

    // Detect changes in Basic Shot values
    bool DetectChange_BasicShot() {
        if((ref_basicShot_shotSpeed != basicShot_shotSpeed) || 
            (ref_basicShot_rotationAxis != basicShot_rotationAxis) || 
            (ref_basicShot_rotationAngle != basicShot_rotationAngle)) {
            ref_basicShot_shotSpeed     = basicShot_shotSpeed;
            ref_basicShot_rotationAxis  = basicShot_rotationAxis;
            ref_basicShot_rotationAngle = basicShot_rotationAngle;
            return true;
        } return false;
    }
}
