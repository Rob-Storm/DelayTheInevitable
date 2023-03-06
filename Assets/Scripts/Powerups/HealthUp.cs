using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    PlayerMain player;

    public bool safeToDelete = false;

    public bool hasBeenPickedUp = false;

    public BoxCollider meshCollider;

    public MeshRenderer meshRenderer;

    public ParticleSystem partSystem;

    public int amount;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();


    }

    void Update()
    {
        safeToDelete = partSystem.isPlaying;

        DeleteSafety();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(player.health < 100)
        {
            player.AddHealth(amount);

            partSystem.Play();

            hasBeenPickedUp = true;

            StartCoroutine(InvisDelay());
        }

        else
        { 
            return;
        }
    }

    void DeleteSafety()
    {
        if (!safeToDelete && hasBeenPickedUp)
        {
            GameObject.Destroy(this.gameObject);
        }

        else
        {
            return;
        }
        
    }

    IEnumerator InvisDelay()
    {
        yield return new WaitForSeconds(0.1f);

        meshCollider.enabled = false;
        meshRenderer.enabled = false;
    }
}
