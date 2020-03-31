using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator animateCam;

    public void CamShake()
    {
        int allShakes = Random.Range(0,3);
        switch (allShakes)
        {
            case 0:
                animateCam.SetTrigger("Shake_X-axis");
                break;
            case 1:
                animateCam.SetTrigger("Shake_Y-axis");
                break;
            case 2:
                animateCam.SetTrigger("Shake_Z-axis");
                break;
            default:
                break;
        }
        
    }
}
