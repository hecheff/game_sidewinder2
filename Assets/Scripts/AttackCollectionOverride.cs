using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Variable overrides for Attacks
public class AttackCollectionOverride : MonoBehaviour {

    [Header("Update Flag")]
    [Tooltip("Enable once during runtime to override existing settings")]
    public bool updateFlag = false;

    [Header("Basic Shot Properties")]
    public GameObject           basicShot_playerShotPool;                                   // Shot Pool
    public List<ShotProperties> basicShot_shotProperties    = new List<ShotProperties>();   // List of all objects in pool
    public float                basicShot_shotSpeed         = 0.0f;                         // Shot velocity
    public Axis                 basicShot_rotationAxis      = Axis.Z;                       // Rotation Axis 
    public float                basicShot_rotationAngle     = 0.0f;                         // Rotation Angle
    
    void Awake() {
        // Populate Basic Shot attributes
        foreach (Transform child in basicShot_playerShotPool.transform) {
            var currShotProperties = child.gameObject.GetComponent<ShotProperties>();

            basicShot_shotProperties.Add(currShotProperties);
            basicShot_shotSpeed     = currShotProperties.shotSpeed;
            basicShot_rotationAxis  = currShotProperties.rotationAxis;
            basicShot_rotationAngle = currShotProperties.rotationAngle;
        }
    }

    void Update() {
        if(updateFlag) {
            for(int i = 0; i < basicShot_shotProperties.Count; i++) {
                basicShot_shotProperties[i].shotSpeed       = basicShot_shotSpeed;
                basicShot_shotProperties[i].rotationAxis    = basicShot_rotationAxis;
                basicShot_shotProperties[i].rotationAngle   = basicShot_rotationAngle;
                basicShot_shotProperties[i].updateOverride  = true;
            }

            updateFlag = false;
        }
    }
}
