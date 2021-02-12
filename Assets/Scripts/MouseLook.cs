using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 50f;

    public Transform playerBody;

    float xRotation = 0f;

    static bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = MouseSensitivity.mouseSens * 100;
        disableCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public static void enableCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isEnabled = true;
    }

    public static void disableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isEnabled = false;
    }
}
