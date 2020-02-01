using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SwingTrack : MonoBehaviour
{
    bool isMoving = false;

    float threshold = 1f;

    float totalForce = 0f;

    float lastForce = 0f;

    public TMP_Text textMesh;

    public Rigidbody hammer;

    // Start is called before the first frame update
    void Start()
    {
        //textMesh = FindObjectOfType<TMP_Text>();
        Input.gyro.enabled = true;
        Handheld.Vibrate();
        Debug.Log(hammer.maxAngularVelocity);
        hammer.maxAngularVelocity = 100f;
    }

    float speed = 10f;
    float smooth = 1f;

    Vector2 rotation;

    void OnMouseDrag()
    {
        rotation.y += Input.GetAxis("Mouse X") * speed;
        rotation.x += -Input.GetAxis("Mouse Y") * speed;

        var baseRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, baseRotation, smooth);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if(Input.touchCount >= 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true;
            totalForce = 10f;
        }
        var currentForce = Input.gyro.userAcceleration.x * 1f;

        if (isMoving)
        {
            if (currentForce < threshold)
            {
                isMoving = false;
                lastForce = totalForce;
                hammer.AddRelativeTorque(0f, 0f, totalForce * 100f);

                Debug.Log("Last Force: " + totalForce);
                Vibration.CreateOneShot(200, Mathf.Min((int)totalForce * 10, 255));
            }
            else
            {
                totalForce += currentForce;
            }
        }
        else
        {
            isMoving = currentForce > threshold;
            totalForce = currentForce;
        }

        textMesh.text = "Last Force: " + lastForce;
    }
}
