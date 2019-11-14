using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox_Damage : MonoBehaviour
{
    public PlayerController playerController;
    
    void OnTriggerEnter(Collider other)
    {

        // if(other.CompareTag("PowerUp")) {
        //     playerController.powerMeterController.IncrementPowerMeter();
        //     Destroy(other.gameObject);
        // }
    }
}
