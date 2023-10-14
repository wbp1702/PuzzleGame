using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public Camera playerCamera;
    public float mouseSensitivity = 100f;
    public float moveSpeed = 10f;

    private CharacterController controller;
    private float cameraPitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
		{   // Translation
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            controller.Move(transform.forward * moveY + transform.right * moveX);
		}

		{   // Rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            cameraPitch = Mathf.Clamp(cameraPitch - mouseY, -90.0f, 90.0f);

            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0.0f, 0.0f);
            transform.Rotate(0.0f, mouseX, 0.0f);
		}
    }
}
