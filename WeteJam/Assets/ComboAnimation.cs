using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAnimation : MonoBehaviour
{
    Vector3 iniScale;
    private Vector3 initialRotation;

    // Rotation offset amplitude (in degrees)
    public float rotationAmplitude = 5f;
    // Frequency of the rotation animation
    public float rotationFrequency = 10f;

    void Start()
    {
        iniScale = transform.localScale;
        initialRotation = transform.eulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = iniScale * (1 + Mathf.Sin(Time.time * 10f) * 0.05f);
        float angleOffset = Mathf.Sin(0.65f*transform.position.x+ Time.time * rotationFrequency) * rotationAmplitude;

        // Apply the offset (this example rotates around the Y axis)
        transform.eulerAngles = initialRotation + new Vector3(0, 0, angleOffset);
    }
}
