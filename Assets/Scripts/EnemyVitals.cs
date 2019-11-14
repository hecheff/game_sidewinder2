using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVitals : MonoBehaviour
{
    public GameObject explosion;

    public int maxHP = 1;   // Maximum HP
    public int currHP;      // Current HP (same as maxHP when initialized in-game)

    public int score_destroy    = 100;  // Score earned by player when destroyed successfully
    public int score_hit        = 0;    // Score earned by player when dealing 1 point of damage      

    void Awake() {
        currHP = maxHP;     // Initialize current HP to be same as max
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerAttack")) {
            GameController.Instance.AnnotateScore(score_hit);
            currHP--;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currHP == 0) {
            Instantiate(explosion, transform.position, transform.rotation);
            GameController.Instance.AnnotateScore(score_destroy);
            Destroy(gameObject);
        }
    }
}
