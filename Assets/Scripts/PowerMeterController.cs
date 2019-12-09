using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerMeterController : MonoBehaviour {
    public Sprite meterTexture_inactive;
    public Sprite meterTexture_active;

    public List<SpriteRenderer>     powerMeterSprites   = new List<SpriteRenderer>();       // List of all power meter sprite renderer objects
    public List<TextMeshProUGUI>    powerMeterText      = new List<TextMeshProUGUI>();      // List of text for each power meter entry
    public List<string>             powerMeterText_ref  = new List<string>();               // Reference text for initial contents

    public Color32    powerMeterTextColor_inactive    =   new Color32(100,100,100,255);
    public Color32    powerMeterTextColor_active      =   new Color32(255,255,255,255);

    public int currentLitMeter = -1;        // -1 = Nothing lit, 0 - 5 for each corresponding entry's index

    // Max. limits of each power up. Make power up unselectable when maxed out
    public int powerMax_0_speedUp       = 6;
    public int powerMax_1_missile       = 2;
    public int powerMax_2_attack_laser  = 2;
    public int powerMax_3_attack_charge = 1;
    public int powerMax_4_optionCount   = 6;    // Should be 4 or 6
    public int powerMax_5_shield        = 1;    



    void Awake() {
        // Auto-set Power Up Items from Power Meter GameObject
        // SpriteRenderer for changing bar highlights
        // TextMeshProUGUI for changing text highlights
        foreach (Transform child in transform) {
            powerMeterSprites.Add(child.GetComponent<SpriteRenderer>());

            // Get child object of each current power bar
            foreach(Transform textObject in child) {
                //Debug.Log(textObject.name);
                powerMeterText.Add(textObject.GetComponent<TextMeshProUGUI>());
            }
        }

        // [TEMP] Set names of each power meter item
        // Needs method to auto-set the contents i player loadout is modified
        powerMeterText[0].text = "SPEED UP";
        powerMeterText[1].text = "MISSILE";
        powerMeterText[2].text = "LASER";
        powerMeterText[3].text = "CHARGE";
        powerMeterText[4].text = "OPTION";
        powerMeterText[5].text = "SHIELD";

        for(int i = 0; i < powerMeterText.Count; i++) {
            powerMeterText_ref.Add(powerMeterText[i].text);
        }
    }

    public void IncrementPowerMeter() {
        if((currentLitMeter + 1) < powerMeterSprites.Count) {
            currentLitMeter++;
        } else {
            currentLitMeter = 0;
        }
        UpdatePowerMeterLight();
    }

    // Reset power meter when power up used by player
    public void ResetMeter() {
        currentLitMeter = -1;
        UpdatePowerMeterLight();
    }

    // Reset power meter when player dies
    // If at least one power meter is lit, reset to beginning
    // Otherwise, reset to nothing (-1)
    public void ResetMeter_Die() {
        Debug.Log("Power meter reset executed.");
        if(currentLitMeter > -1) {
            currentLitMeter = 0;
        }
        UpdatePowerMeterLight();

        // Update power meter text contents (reset all since powers all resetted)
        for(int i = 0; i < powerMeterSprites.Count; i++) {
            UpdatePowerMeterText(i, 0);
        }
    }

    // Update power meter light status according to currentLitMeter
    public void UpdatePowerMeterLight() {
        for(int i = 0; i < powerMeterSprites.Count; i++) {
            powerMeterSprites[i].sprite = meterTexture_inactive;
            powerMeterText[i].color     = powerMeterTextColor_inactive;
        }
        if(currentLitMeter != -1) {
            powerMeterSprites[currentLitMeter].sprite   = meterTexture_active;
            powerMeterText[currentLitMeter].color       = powerMeterTextColor_active;
        }
    }

    // Update power meter text (index only) based its current level
    public void UpdatePowerMeterText(int meterIndex, int currentLevel) {
        bool blankOut_isMaxed = false;
        
        switch(meterIndex) {
            case 0: 
                if(currentLevel >= powerMax_0_speedUp) { 
                    blankOut_isMaxed = true; 
                }
                break;
            case 1: 
                if(currentLevel >= powerMax_1_missile) { 
                    blankOut_isMaxed = true; 
                }
                break;
            case 2: 
                if(currentLevel >= powerMax_2_attack_laser) { 
                    blankOut_isMaxed = true; 
                }
                break;
            case 3: 
                if(currentLevel >= powerMax_3_attack_charge) { 
                    blankOut_isMaxed = true; 
                }
                break;
            case 4: 
                if(currentLevel >= powerMax_4_optionCount) { 
                    blankOut_isMaxed = true; 
                }
                break;
            case 5: 
                if(currentLevel >= powerMax_5_shield) { 
                    blankOut_isMaxed = true; 
                }
                break;
        }

        if(blankOut_isMaxed) { 
            powerMeterText[meterIndex].text = "";
        } else {
            powerMeterText[meterIndex].text = powerMeterText_ref[meterIndex];
        }

        /*
        powerMeterText[0].text = "SPEED UP";
        powerMeterText[1].text = "MISSILE";
        powerMeterText[2].text = "LASER";
        powerMeterText[3].text = "CHARGE";
        powerMeterText[4].text = "OPTION";
        powerMeterText[5].text = "SHIELD";
        */
    }



    // DEPRECATED METHOD: Too much manual setup. Replaced with automated generation of Lists for each relevant part instead of arrays.
    /*
    public Sprite meterTexture_inactive;
    public Sprite meterTexture_active;

    public SpriteRenderer[] powerMeterSprites;
    public TextMesh[]       powerMeterText;

    public Color32    powerMeterTextColor_inactive    =   new Color32(100,100,100,255);
    public Color32    powerMeterTextColor_active      =   new Color32(255,255,255,255);

    public int currentLitMeter = -1;        // -1 = Nothing lit, 0 - 5 for each corresponding entry's index

    void Awake() {
        for(int i = 0; i < powerMeterSprites.Length; i++) {
            
            foreach(Transform child in powerMeterSprites[i].transform) {
                powerMeterText[i] = child.GetComponent<TextMesh>();
            }
        }
    }

    public void IncrementPowerMeter() {
        if((currentLitMeter + 1) < powerMeterSprites.Length) {
            currentLitMeter++;
        } else {
            currentLitMeter = 0;
        }
        UpdatePowerMeterLight();
    }

    public void ResetMeter() {
        currentLitMeter = -1;
        UpdatePowerMeterLight();
    }

    // Update power meter light status according to currentLitMeter
    public void UpdatePowerMeterLight() {
        for(int i = 0; i < powerMeterSprites.Length; i++) {
            powerMeterSprites[i].sprite = meterTexture_inactive;
            powerMeterText[i].color     = powerMeterTextColor_inactive;
        }
        if(currentLitMeter != -1) {
            powerMeterSprites[currentLitMeter].sprite   = meterTexture_active;
            powerMeterText[currentLitMeter].color       = powerMeterTextColor_active;
        }
    }
    */
}

// Contains all text variables for power ups
// No idea if this would work
public struct PowerUpText {
    public string test;
    
    //string[] txt_0_0_speedup = new string[];

    // SAMPLES: Power Up Text List
    /*
        00_0 - Speed Up
        "SPEED UP", "INIT. SPEED"

        01_0_0 - Missile
        "MISSILE", "MISSILE II"
        
        01_1 - Two-Way Missile
        "TWO-WAY", "TWO-WAY II"
        
        01_2 - Two-Way Reverse
        "TWO-WAY R.", "TWO-WAY R. II"
        

        02_0 - Laser
        "LASER", "LASER II"


        03_0 - Charge


        04_0 - Option
        "OPTION"


        05_0 - Force Shield
        "F.SHIELD"

        05_1 - Phase Out (Temporary invincibility)
        "PHASEOUT"


    */
}