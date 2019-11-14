using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Asteroid : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float tumble;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
