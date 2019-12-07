using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVitals : MonoBehaviour {
    public GameObject explosion;
    public MeshRenderer meshRenderer;
    public Color32 flashColor_damage = new Color32(255,255,255,255);
    public float flashTime = 0.05f;
    private Color32 originalColor;
    private Material originalMaterial;

    public int maxHP = 1;   // Maximum HP
    public int currHP;      // Current HP (same as maxHP when initialized in-game)

    public int score_destroy    = 100;  // Score earned by player when destroyed successfully
    public int score_hit        = 0;    // Score earned by player when dealing 1 point of damage      

    void Awake() {
        currHP = maxHP;     // Initialize current HP to be same as max
        
        // Establish mesh renderer defaults for reference when showing damage taking effects
        if(meshRenderer) {
           originalMaterial = meshRenderer.material;
           originalColor    = meshRenderer.material.color;
        }
    }
    
    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PlayerAttack")) {
            GameController.Instance.AnnotateScore(score_hit);
            TakeDamage(other.GetComponent<ShotDamage>().attackDamage);
        }
    }
    void OnTriggerStay(Collider other) {
        if(other.CompareTag("PlayerAttack")) {
            GameController.Instance.AnnotateScore(score_hit);
            TakeDamage(other.GetComponent<ShotDamage>().attackDamage);
        }
    }

    // Update is called once per frame
    void Update() {
        // When enemy HP is 0 or below, set destroy target
        if(currHP <= 0) {
            Instantiate(explosion, transform.position, transform.rotation);
            GameController.Instance.AnnotateScore(score_destroy);
            Destroy(gameObject);
        }
    }

    // Actions when taking damage
    void TakeDamage(int damage) {
        currHP -= damage;;
        if(meshRenderer) {
            meshRenderer.material = null;
            meshRenderer.material.color = flashColor_damage;
            Invoke("ResetColor", flashTime);
        }
    }

    // Reset color after impact
    void ResetColor() {
        meshRenderer.material       = originalMaterial;
        meshRenderer.material.color = originalColor;
    }
}
