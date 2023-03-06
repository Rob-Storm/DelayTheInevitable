using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    PlayerMain player;

    public Animation anim;

    public AnimationClip cameraPan;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isAlive)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + player.camDistance, player.transform.position.z);
        }
    }
}
