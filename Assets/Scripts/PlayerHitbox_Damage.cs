using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox_Damage : MonoBehaviour
{
    public PlayerController playerController;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("EnemyAttack") || other.CompareTag("Stage")) {
            playerController.PlayerDie();
        }
    }
}
