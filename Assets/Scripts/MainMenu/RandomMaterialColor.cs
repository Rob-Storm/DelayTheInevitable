using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialColor : MonoBehaviour
{

    public Material material;

    public Color startColor;

    public Color endColor;
    
    float startTime;

    public float speed = 1.0f;

    public bool repeatable = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {

        if(!repeatable)
        {
            float t = (Time.time - startTime) * speed;

            material.color = Color.Lerp(startColor, endColor, t);
        }

        else
        {
            float t = (Mathf.Sin(Time.time - startTime) * speed);

            material.color = Color.Lerp(startColor, endColor, t);
        }

    }
}
