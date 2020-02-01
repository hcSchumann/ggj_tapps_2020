using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    private Vector3 initialPosition;
    public Rigidbody target;

    private void Awake()
    {
        initialPosition = target.position;
    }

    public void OnPress()
    {
        Debug.Log($"Reseting to position: {initialPosition}");
        target.velocity = Vector3.zero;
        target.position = initialPosition;
    }
}
