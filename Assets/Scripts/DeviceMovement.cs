using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceMovement : MonoBehaviour
{
    private AccelerometerUtil accelerometerUtil;
    private Rigidbody rb;
    private float forceMultiplier = 10f;

    void Start()
    {
        accelerometerUtil = new AccelerometerUtil();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Move object
        Vector3 accel = accelerometerUtil.GetAcceleration();
        rb.velocity = forceMultiplier*accel;
    }
}