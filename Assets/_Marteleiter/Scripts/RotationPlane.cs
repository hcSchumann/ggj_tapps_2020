using UnityEngine;
using UnityEngine.Networking;

public class RotationPlane : NetworkBehaviour
{
    [SerializeField] public int rotationDirection = 0;

    [SerializeField] private float rotationSpeed = 10f;

    private void FixedUpdate()
    {
        if (!isServer)
        {
            return;
        }

        transform.Rotate(new Vector3(0, 0, rotationDirection), rotationSpeed);
    }
}
