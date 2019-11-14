using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    //public float lifetime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject);
    }
}
