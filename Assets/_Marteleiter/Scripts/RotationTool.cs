using UnityEngine.Networking;

public class RotationTool : NetworkBehaviour
{
    private RotationPlane rotationPlane;

    private void Start()
    {
        rotationPlane = FindObjectOfType<RotationPlane>();
    }
    public void SetRotationEvent(int rotation)
    {
        CmdSetRotation(rotation);
    }

    [Command]
    public void CmdSetRotation(int rotation)
    {
        rotationPlane.rotationDirection = rotation;
    }
}
