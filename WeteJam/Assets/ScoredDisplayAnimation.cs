using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoredDisplayAnimation : MonoBehaviour
{
    Vector3 iniScale;
   

    void Start()
    {
        iniScale = transform.localScale;
   
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = iniScale * (1 + Mathf.Sin(Time.time * 9f) * 0.05f);
    }
}

