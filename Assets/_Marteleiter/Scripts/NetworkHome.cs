using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class NetworkHome : MonoBehaviour
{
    NetworkManager networkManager;

    [SerializeField] TMP_InputField ipField;

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    public void Host()
    {
        networkManager.StartHost();
    }

    public void Join()
    {
        networkManager.networkAddress = ipField.text;
        networkManager.StartClient();
    }
}
