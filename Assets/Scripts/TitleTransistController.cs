using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleTransistController : MonoBehaviour
{
    public Animator pressStartAnimator;

    private float transistDelay = 2.0f;     // Time before scene transition is executed
    private float timestamp_transist;       // Variable for defining transition time (current time + transition delay)

    // Update is called once per frame
    void Update()
    {
        if(pressStartAnimator.GetBool("StartPressed") == false) {
            if(Input.GetButton("Fire1") || Input.GetButton("Submit")) {
                pressStartAnimator.SetBool("StartPressed", true);
                SoundManager.Instance.PlaySFX_GameStart();
                timestamp_transist = Time.timeSinceLevelLoad + transistDelay;
            }
        } else {
            if(Time.timeSinceLevelLoad >= timestamp_transist) {
                SceneManager.LoadScene("03 CGP");
            }
        }
    }
}
