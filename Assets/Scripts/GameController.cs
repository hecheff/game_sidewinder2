using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    // Establishing instance of GameController makes it accessible everywhere else.
        public static GameController Instance;

    public int currentScore = 0;
    public TextMeshProUGUI txt_currentScore;

    // Start is called before the first frame update
    void Awake()
    {
        // Set GameController as Instance
        if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if (Instance != null) { Destroy(gameObject); }
    }

    void Start() {
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnnotateScore(int score) {
        currentScore += score;
        UpdateScoreText();
    }

    void UpdateScoreText() {
        txt_currentScore.text = "" + currentScore.ToString("0000000");
    }
}
