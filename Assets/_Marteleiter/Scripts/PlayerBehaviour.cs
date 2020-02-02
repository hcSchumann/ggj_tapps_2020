using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : NetworkBehaviour
{
    [SerializeField] private Canvas canvas;

    [SerializeField] private GameObject hammerObject;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = !isServer;
        hammerObject.SetActive(!isServer);
    }
}
