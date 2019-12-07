using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using UnityEngine.SceneManagement;

public enum Axis        { X,Y,Z }                           // Axis settings for individual object controls
public enum Language    { English = 0, Japanese = 1 };      // Language settings reflected across entire game

public enum MissilePattern { TwoWay, TwoWay_Back, SpreadBomb,  }
public enum OptionPattern { Follow, Formation, Rotate,  }   // Option formation type

public class GlobalController : MonoBehaviour {
    // Establishing instance of GlobalController makes it accessible everywhere else.
    public static GlobalController Instance;

    // Set target frame rate to ensure consistent target performance. 
    // Keep as modifiable variable in event of being run by low-spec systems.
    // Setttings: 30fps, 60fps
    // Comments: Set 30fps for menus and 60fps during gameplay to improve battery efficiency?
    public int targetFrameRate = 60;

    // Start is called before the first frame update
    void Awake() {
        Screen.SetResolution(1080,1920,true);
        //savePath = Application.persistentDataPath + "/" + "gameData.json";

        Application.targetFrameRate = targetFrameRate;  // Set target frame rate of game (ideal: 60fps)
        Time.captureDeltaTime       = 1.0f/targetFrameRate;

        // Set GlobalController as Instance
        if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if (Instance != null) { Destroy(gameObject); }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
