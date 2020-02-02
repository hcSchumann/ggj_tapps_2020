using UnityEngine;
using UnityEngine.Networking;

public class RotationPlane : NetworkBehaviour
{
    [SerializeField] public int rotationDirection = 0;

    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private GameObject hammerPrefab;

    [SerializeField] private Transform hammerPosition;

    private void FixedUpdate()
    {
        if (!isServer)
        {
            return;
        }

        transform.Rotate(new Vector3(0, 0, rotationDirection), rotationSpeed);
    }

    public void SpawnHammer(float swingForce)
    {
        var hammerSwing = (GameObject.Instantiate(hammerPrefab, hammerPosition) as GameObject).GetComponent<HammerSwing>();
        hammerSwing.swingForce = swingForce;
    }
}
