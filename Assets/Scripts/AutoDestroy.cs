using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(particleSystem) {
            if(!particleSystem.IsAlive()) {
                Destroy(gameObject);
            }
        }
    }
}
