using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerMeterController : MonoBehaviour
{
    public Sprite meterTexture_inactive;
    public Sprite meterTexture_active;

    public List<SpriteRenderer>     powerMeterSprites   = new List<SpriteRenderer>();       // List of all objects in pool
    public List<TextMeshProUGUI>    powerMeterText      = new List<TextMeshProUGUI>();          // 

    public Color32    powerMeterTextColor_inactive    =   new Color32(100,100,100,255);
    public Color32    powerMeterTextColor_active      =   new Color32(255,255,255,255);

    public int currentLitMeter = -1;        // -1 = Nothing lit, 0 - 5 for each corresponding entry's index

    void Awake() {
        // Auto-set Power Up Items from Power Meter GameObject
        // SpriteRenderer for changing bar highlights
        // TextMeshProUGUI for changing text highlights
        foreach (Transform child in transform) {
            powerMeterSprites.Add(child.GetComponent<SpriteRenderer>());

            // Get child object of each current power bar
            foreach(Transform textObject in child) {
                Debug.Log(textObject.name);
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
    }

    public void IncrementPowerMeter() {
        if((currentLitMeter + 1) < powerMeterSprites.Count) {
            currentLitMeter++;
        } else {
            currentLitMeter = 0;
        }
        UpdatePowerMeterLight();
    }

    //
    public void ResetMeter() {
        currentLitMeter = -1;
        UpdatePowerMeterLight();
    }

    // 
    public void ResetMeter_Die() {
        if(currentLitMeter > -1) {
            currentLitMeter = 0;
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
