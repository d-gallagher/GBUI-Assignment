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
                
            //// DB2C36 red
            //if( ColorUtility.TryParseHtmlString("#DB2C36", out col)){
            //    imageFireReady.color = col;
            //}
            imageCooldown.fillAmount += 1 / _radialBeltScript.cooldownTime * Time.deltaTime;
            if (imageCooldown.fillAmount >=1)
            {
                imageCooldown.fillAmount = 0;

                //if (ColorUtility.TryParseHtmlString("#95E01C", out col))
                //{
                //    imageFireReady.color = col;
                //}
            }
            // 95E01C green
        }
    }
}
