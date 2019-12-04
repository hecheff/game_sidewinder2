using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Automatically populates object pool to make contents accessible publicly.
public class ObjectPool : MonoBehaviour {
    // Auto-generated lists. Currently set public for debugging.
    public List<GameObject> objects = new List<GameObject>();       // List of all objects in pool
    public List<Animator> objectAnimator = new List<Animator>();    // List of animatiors in objects (if available)

    public ObjectPool effectsObjectPool;                            

    // Populate list with all children game objects.
    void Awake() {
        foreach (Transform child in transform) {
            objects.Add(child.gameObject);

            if(child.GetComponent<Animator>()) {
                objectAnimator.Add(child.GetComponent<Animator>());
            } 
        }
    }

    // Set angle of object on play
    public void SetAngleOfEffect(int index, Axis currentAxis, Transform transformData) {
        objects[index].transform.rotation = Quaternion.identity;   // Reset rotation to default (0,0,0)

        switch(currentAxis) {
            case Axis.X:
                objects[index].transform.Rotate(new Vector3 (transformData.eulerAngles.x, 0, 0));
                break;
            case Axis.Y:
                objects[index].transform.Rotate(new Vector3 (0, transformData.eulerAngles.y, 0));
                break;
            case Axis.Z:
            default:
                objects[index].transform.Rotate(new Vector3 (0, 0, transformData.eulerAngles.z));
                Debug.Log("transformData.eulerAngles.z = " + transformData.eulerAngles.z);
                break;
        }
        
    }
}
