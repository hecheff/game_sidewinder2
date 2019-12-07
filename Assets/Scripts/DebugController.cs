using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script for handling all debug inputs
public class DebugController : MonoBehaviour {
    // Reset core gameplay scene
    public void ResetScene_CGP() {
        SceneManager.LoadScene("03 CGP");
    }
}
