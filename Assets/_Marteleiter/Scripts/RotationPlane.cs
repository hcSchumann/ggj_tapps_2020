using UnityEngine;
using UnityEngine.Networking;

public class RotationPlane : NetworkBehaviour
{
    [SerializeField] public int rotationDirection = 0;

    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private Transform hammerPosition;

    [SerializeField] private GameObject goalIndicator;

    private bool ShouldAcceptInputs = false;

    private void FixedUpdate()
    {
        if (!isServer || !ShouldAcceptInputs)
        {
            return;
        }

        transform.Rotate(new Vector3(0, 0, rotationDirection), rotationSpeed);
        goalIndicator.transform.Rotate(new Vector3(0, 0, -rotationDirection), rotationSpeed);
    }

    public void SetInputStatus(bool inputStatus)
    {
        ShouldAcceptInputs = inputStatus;
    }

    public void SpawnHammer(float swingForce)
    {
        if (!ShouldAcceptInputs) return;
        var hammerSwing = (GameObject.Instantiate(hammerPrefab, hammerPosition) as GameObject).GetComponent<HammerSwing>();
        hammerSwing.swingForce = swingForce;
    }
}
