using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBlock : MonoBehaviour
{
    PlayerMain player;

    bool canHurt = false;

    public bool safeToDelete = false;

    public bool hasBeenPickedUp = false;

    public BoxCollider meshCollider;

    public MeshRenderer meshRenderer;

    public ParticleSystem partSystem;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();

        StartCoroutine(HurtDelay());

    }

    private void OnTriggerEnter(Collider other)
    {

        if (!player.invincible && canHurt)
        {
            player.AddHealth(player.health * -1);

            hasBeenPickedUp = true;

            GameObject.Destroy(this.gameObject);
        }

        else if (canHurt || player.invincible)
        {
            partSystem.Play();

            hasBeenPickedUp = true;

            StartCoroutine(InvisDelay());
        }

        else if (!canHurt)
        {
            return;
        }

    }

    IEnumerator HurtDelay()
    {
        yield return new WaitForSeconds(2);

        canHurt = true;
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
