using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Automatically populates object pool to make contents accessible publicly.
public class ObjectPool : MonoBehaviour
{
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
}
