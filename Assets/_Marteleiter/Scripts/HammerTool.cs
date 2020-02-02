using UnityEngine;
using UnityEngine.Networking;

public class HammerTool : NetworkBehaviour
{
    private float totalForce = 0f;

    private readonly float forceThreshold = 1f;

    private readonly float forceMultiplier = 1f;

    private readonly int vibrationDuration = 200;

    private RotationPlane rotationPlane;

    private readonly float cooldownTime = 0.5f;

    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        rotationPlane = FindObjectOfType<RotationPlane>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rotationPlane.SpawnHammer(15f);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            rotationPlane.SpawnHammer(5f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.fixedTime;
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
            if (cooldownTime > currentTime)
                return;

            currentTime = 0f;
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
