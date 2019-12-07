using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_MasterController : MonoBehaviour {
    public List<AttackCollectionOverride> attackCollectionOverride = new List<AttackCollectionOverride>();

    [Header("Player Attacks")]
    public float    player_basicShot_shotSpeed         = 0.0f;         // Shot velocity
    public Axis     player_basicShot_rotationAxis      = Axis.Z;       // Rotation Axis 
    public float    player_basicShot_rotationAngle     = 0.0f;         // Rotation Angle
    private float   ref_player_basicShot_shotSpeed;
    private Axis    ref_player_basicShot_rotationAxis;
    private float   ref_player_basicShot_rotationAngle;

    [Header("Options Attacks")]
    public float    options_basicShot_shotSpeed        = 0.0f;         // Shot velocity
    public Axis     options_basicShot_rotationAxis     = Axis.Z;       // Rotation Axis 
    public float    options_basicShot_rotationAngle    = 0.0f;         // Rotation Angle
    private float   ref_options_basicShot_shotSpeed;
    private Axis    ref_options_basicShot_rotationAxis;
    private float   ref_options_basicShot_rotationAngle;

    //[Header("Missile Attacks")]
    //public 

    void Start() {
        foreach (Transform child in transform) {
            attackCollectionOverride.Add(child.gameObject.GetComponent<AttackCollectionOverride>());
        }

        // Get attack values for Player
        for(int i = 0; i < attackCollectionOverride.Count; i++) {
            if(attackCollectionOverride[i].isPlayer) {
                player_basicShot_shotSpeed      = attackCollectionOverride[i].basicShot_shotSpeed;
                player_basicShot_rotationAxis   = attackCollectionOverride[i].basicShot_rotationAxis;
                player_basicShot_rotationAngle  = attackCollectionOverride[i].basicShot_rotationAngle;

                ResetRefValues_Player();   // Initialize reference values to detect changes
                break;
            }
        }

        // Get attack values for Options
        for(int i = 0; i < attackCollectionOverride.Count; i++) {
            if(!attackCollectionOverride[i].isPlayer) {
                options_basicShot_shotSpeed      = attackCollectionOverride[i].basicShot_shotSpeed;
                options_basicShot_rotationAxis   = attackCollectionOverride[i].basicShot_rotationAxis;
                options_basicShot_rotationAngle  = attackCollectionOverride[i].basicShot_rotationAngle;

                ResetRefValues_Options();   // Initialize reference values to detect changes
                break;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        // Check if Player stats are to be changed
        if(DetectChange_Player()) {
            for(int i = 0; i < attackCollectionOverride.Count; i++) {
                if(attackCollectionOverride[i].isPlayer) {
                    attackCollectionOverride[i].basicShot_shotSpeed     = player_basicShot_shotSpeed;
                    attackCollectionOverride[i].basicShot_rotationAxis  = player_basicShot_rotationAxis;
                    attackCollectionOverride[i].basicShot_rotationAngle = player_basicShot_rotationAngle;
                    break;  // Can change once found since there is only one Player source
                }
            }
        }
        
        // Check if Option stats are changed
        if(DetectChange_Options()) {
            for(int i = 0; i < attackCollectionOverride.Count; i++) {
                if(!attackCollectionOverride[i].isPlayer) {
                    attackCollectionOverride[i].basicShot_shotSpeed     = options_basicShot_shotSpeed;
                    attackCollectionOverride[i].basicShot_rotationAxis  = options_basicShot_rotationAxis;
                    attackCollectionOverride[i].basicShot_rotationAngle = options_basicShot_rotationAngle;
                }
            }
        }
    }

    bool DetectChange_Player() {
        if((ref_player_basicShot_shotSpeed          != player_basicShot_shotSpeed) 
            ||(ref_player_basicShot_rotationAxis    != player_basicShot_rotationAxis)
            ||(ref_player_basicShot_rotationAngle   != player_basicShot_rotationAngle)) {
            ResetRefValues_Player();
            return true;
        } return false;
    }

    bool DetectChange_Options() {
        if((ref_options_basicShot_shotSpeed         != options_basicShot_shotSpeed) 
            ||(ref_options_basicShot_rotationAxis   != options_basicShot_rotationAxis)
            ||(ref_options_basicShot_rotationAngle  != options_basicShot_rotationAngle)) {
            ResetRefValues_Options();
            return true;
        } return false;
    }

    // Reset (or Initialize) reference values for attacks fired by Player
    void ResetRefValues_Player() {
        ref_player_basicShot_shotSpeed      = player_basicShot_shotSpeed;
        ref_player_basicShot_rotationAxis   = player_basicShot_rotationAxis;
        ref_player_basicShot_rotationAngle  = player_basicShot_rotationAngle;
    }

    // Reset (or Initialize) reference values for attacks fired by Options
    void ResetRefValues_Options() {
        ref_options_basicShot_shotSpeed      = options_basicShot_shotSpeed;
        ref_options_basicShot_rotationAxis   = options_basicShot_rotationAxis;
        ref_options_basicShot_rotationAngle  = options_basicShot_rotationAngle;
    }
}
