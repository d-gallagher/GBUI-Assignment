using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject flashHolder;
    public float flashTime;

    public Sprite[] flashSprites;
    public SpriteRenderer[] flashSpriteRenderers;


    private void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        flashHolder.SetActive(true);

        int flashSpriteIndex = UnityEngine.Random.Range(0, flashSprites.Length);
        for(int i =0; i < flashSprites.Length; i++)
        {
            flashSpriteRenderers[i].sprite = flashSprites[flashSpriteIndex];
        }

        Invoke("Deactivate", flashTime);
    }

    public void Deactivate() => flashHolder.SetActive(false);
}
