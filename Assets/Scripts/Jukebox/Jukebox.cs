using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip backMusic;

    public AudioClip deathMusic;

    public float fadeTime;

    bool hasChanged = false;

    PlayerMain player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();


        audioSource.clip = backMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.isAlive)
        {
            audioSource.clip = backMusic;
        }

        else
        {
            if (!hasChanged)
            {
                StartCoroutine(FadeAndSwitch(fadeTime));
            }
        }
    }

    IEnumerator FadeAndSwitch(float fadeTime)
    {
        hasChanged = true;

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) 
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = deathMusic;
        audioSource.volume = startVolume;
        audioSource.Play();
    }
}