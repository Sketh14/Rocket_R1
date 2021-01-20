using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will go for both Player And Enemy And Ufo Sounds if possible, otherwise for animation purposes only

public class SoundEffectsScript : MonoBehaviour
{
    public AudioSource objectSource;
    public AudioClip[] soundEffects;

    private void PowerUpSE()
    {
        objectSource.Stop();
        objectSource.PlayOneShot(soundEffects[0]);
    }

    private void PowerDownSE()
    {
        objectSource.Stop();
        objectSource.PlayOneShot(soundEffects[1]);
    }

    private void AtPlace()
    {
        objectSource.PlayOneShot(soundEffects[Random.Range(2,4)]);
    }
}
