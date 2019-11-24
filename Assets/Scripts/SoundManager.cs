using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: This SoundManager is only intended for BGMs and SFXs which are universal across the entire game and are meant to be played without potential overlap.
// Therefore, any in-game sound effects such as shots or explosions need to be handled by their respective game objects.
public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

	// Audio source (players)
	public AudioSource audioSource_BGM;		// Background music		
	public AudioSource audioSource_SFX;		// Menu/Primary sound effects

	// Plays when starting game from title menu.
	public AudioClip SFX_GameStart;     
    public void PlaySFX_GameStart() {
		audioSource_SFX.clip = SFX_GameStart;
		audioSource_SFX.Play();
	}
	
    // public AudioClip SFX_MenuSelect;
    // public void PlaySFX_MenuSelect() {
	// 	audioSource.clip = SFX_MenuSelect;
	// 	audioSource.Play();
	// }

    // public AudioClip SFX_MenuConfirm;
	// public void PlaySFX_MenuConfirm() {
	// 	audioSource.clip = SFX_MenuConfirm;
	// 	audioSource.Play();
	// }

    // public AudioClip SFX_MenuCancel;
	// public void PlaySFX_MenuCancel() {
	// 	audioSource.clip = SFX_MenuCancel;
	// 	audioSource.Play();
	// }

	void Awake() {
        // Set SoundManager as Instance
        if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if (Instance != null) { Destroy(gameObject); }
    }
}
