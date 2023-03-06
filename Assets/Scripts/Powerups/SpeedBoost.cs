using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    PlayerMain player;

    public bool safeToDelete = false;

    public bool hasBeenPickedUp = false;

    public BoxCollider meshCollider;

    public MeshRenderer meshRenderer;

    public ParticleSystem partSystem;

    public int boostFactor;

    public int duration;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(!player.hasSpeed)
        {
            player.TempSpeed(duration, boostFactor);

            partSystem.Play();

            hasBeenPickedUp = true;

            StartCoroutine(InvisDelay());
        }

        else
        {
            partSystem.Play();

            hasBeenPickedUp = true;

            StartCoroutine(InvisDelay());
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
