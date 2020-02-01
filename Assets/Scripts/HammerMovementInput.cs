using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMovementInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mainCamera = Camera.main;
        var mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - mainCamera.transform.position.z;

        transform.position = mainCamera.ScreenToWorldPoint(mousePos);
    }
}
