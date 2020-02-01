using UnityEngine;

public class AccelerometerUtil
{
    public float alpha = 0.8f;
    public float[] gravity = new float[3];

    public AccelerometerUtil()
    {
        Vector3 currentAcc = Input.acceleration;
        gravity[0] = currentAcc.x;
        gravity[1] = currentAcc.y;
        gravity[2] = currentAcc.z;
    }

    public Vector3 GetAcceleration()
    {
        Vector3 currentAcc = Input.acceleration;
        gravity[0] = alpha * gravity[0] + (1 - alpha) * currentAcc.x;
        gravity[1] = alpha * gravity[1] + (1 - alpha) * currentAcc.y;
        gravity[2] = alpha * gravity[2] + (1 - alpha) * currentAcc.z;

        Vector3 linearAcceleration =
            new Vector3(currentAcc.x - gravity[0],
                currentAcc.y - gravity[1],
                currentAcc.z - gravity[2]);

        return linearAcceleration;
    }
}