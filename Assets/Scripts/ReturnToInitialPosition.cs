using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a recurring script which is run for all objects and effects which are to return to their initially set position after their animation cycle has been completed
// Examples include bullet hit effects (return to their bullet origin) 
public class ReturnToInitialPosition : MonoBehaviour
{
    public bool disableOnResetOverride  = false;    // If true, set gameObject to inactive after resetting position

    public bool resetPositionFlag       = false;    // Flag to check if position is ready to be reset
    public Vector3 initialPosition;                 // Set this to public for debugging
    
    void Awake() {
        initialPosition = gameObject.transform.position;
    }

    void Update() {
        // Reset object position if flag is set to true (can only be set outside of script, e.g. via Animation)
        if(resetPositionFlag == true) {
            ResetPosition();
            resetPositionFlag = false;  // Revert flag to false

            // If true, set gameObject to inactive as last step in reset
            if(disableOnResetOverride) {
                gameObject.SetActive(false);
            }
        }
    }

    // When called, return to initial position
    public void ResetPosition() {
        gameObject.transform.position = initialPosition;
    }
}
