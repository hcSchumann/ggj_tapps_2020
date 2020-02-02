using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class NetworkHome : MonoBehaviour
{
    NetworkManager networkManager;

    [SerializeField] private TMP_InputField ipField;

    [SerializeField] private AudioClip buttonSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        networkManager = GetComponent<NetworkManager>();
        networkManager.networkAddress = GetLocalIPAddress();
        ipField.text = networkManager.networkAddress;
    }

    public void Host()
    {
        audioSource.PlayOneShot(buttonSound);
        networkManager.StartHost();
        Debug.Log("Hosting at: " + networkManager.networkAddress);
    }

    public void Join()
    {
        audioSource.PlayOneShot(buttonSound);
        networkManager.networkAddress = ipField.text;
        networkManager.StartClient();
    }

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
