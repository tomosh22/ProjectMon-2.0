using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public int mouseXSensitivity;
    [SerializeField]
    public int mouseYSensitivity;
    public bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * mouseYSensitivity * Time.deltaTime);
        if (isPlayer)
        {
            transform.parent.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime);
        }
        else {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime);
        }
        
    }
}
