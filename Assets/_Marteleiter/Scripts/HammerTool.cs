using UnityEngine;
using UnityEngine.Networking;

public class HammerTool : NetworkBehaviour
{
    private float totalForce = 0f;

    private readonly float forceThreshold = 1f;

    private readonly float forceMultiplier = 1f;

    private readonly int vibrationDuration = 200;

    private RotationPlane rotationPlane;

    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        rotationPlane = FindObjectOfType<RotationPlane>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetHammerMovement();
    }

    void GetHammerMovement()
    {
        var currentForce = Input.gyro.userAcceleration.x * forceMultiplier;

        var isMoving = currentForce > forceThreshold;

        if (isMoving)
        {
            totalForce += currentForce;
        } else if (!isMoving && totalForce > 0)
        {
            CmdHammerSwing(totalForce);

            // TODO: Force exponential not linear
            Vibration.CreateOneShot(vibrationDuration, (int) totalForce);
            totalForce = 0f;
        }
    }

    [Command]
    void CmdHammerSwing(float swingForce)
    {
        rotationPlane.SpawnHammer(swingForce);
    }
}
