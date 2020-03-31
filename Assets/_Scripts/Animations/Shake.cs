using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator animateCam;

    public void CamShake()
    {
        animateCam.SetTrigger("Shake_X-axis");
    }
}
