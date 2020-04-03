using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    public Image imageFireReady;
    public Image imageCooldown;

    private RadialBeltScript _radialBeltScript;
    private Color col;

    private void Start()
    {
        _radialBeltScript = FindObjectOfType<RadialBeltScript>();
        imageCooldown.fillAmount = 0;
    }
    private void Update()
    {
        if (_radialBeltScript.IsCoolingDown)
        {
            imageCooldown.fillAmount += 1 / _radialBeltScript.cooldownTime * Time.deltaTime;
            if (imageCooldown.fillAmount >= 0.9965995)
            {
                imageCooldown.fillAmount = 0;
            }
        }
    }
}
