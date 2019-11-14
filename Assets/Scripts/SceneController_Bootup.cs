using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController_Bootup : MonoBehaviour
{
    public GameObject initialSequence;
    public GameObject lastSequence;
    
    public TextMeshProUGUI txt_countdown;

    private bool countdownOver = false;
    private int countup = 0;
    private int countVal = 15;

    // Update is called once per frame
    void Update()
    {
        if(!countdownOver) {
            if(countup >= GlobalController.Instance.targetFrameRate) {
                if(countVal != 0) {
                    countVal--;
                    txt_countdown.text = "" + countVal.ToString("00");
                    countup = 0;
                } else {
                    countdownOver = true;
                    // Play end sequence to bootup.
                    initialSequence.SetActive(false);
                    lastSequence.SetActive(true);
                }
            } else {
                countup++;
            }
        }

        if (Input.GetButton("Fire1") || Input.GetButton("Fire2")) {
            SceneManager.LoadScene("01 Title");
        }
    }
}
