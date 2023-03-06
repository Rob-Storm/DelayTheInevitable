using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{

    public ParticleSystem pfx;

    // Update is called once per frame
    void Update()
    {
        DeleteObject();
    }

    void DeleteObject()
    {
        if(!pfx.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
