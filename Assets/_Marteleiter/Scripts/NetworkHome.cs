using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class NetworkHome : NetworkManager
{
    [SerializeField] private TMP_InputField ipField;

    [SerializeField] private AudioClip buttonSound;

    private AudioSource audioSource;

    private bool isServer = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        networkAddress = GetLocalIPAddress();
        ipField.text = networkAddress;
    }

    public void Host()
    {
        audioSource.PlayOneShot(buttonSound);
        StartHost();
        Debug.Log("Hosting at: " + networkAddress);
    }

    public void Join()
    {
        audioSource.PlayOneShot(buttonSound);
        networkAddress = ipField.text;
        StartClient();
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
