using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    public Transform effectPivot;
    public Animator animator;
    public bool followPlayer = true;

    public float defaultZLayer = -1.0f;

    private Vector3 annotateZLayer;
    void Awake() {
        annotateZLayer = new Vector3(0,0,defaultZLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer) {
            gameObject.transform.position = effectPivot.transform.position + annotateZLayer;
        }
    }

    public void PlayFX() {
        animator.SetTrigger("RestartFX");
    }
}