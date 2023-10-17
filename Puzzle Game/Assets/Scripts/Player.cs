using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera playerCamera;
    public Transform holdPoint;
    public float maxReach = 5.0f;
    public float mouseSensitivity = 100f;
    public float moveSpeed = 10f;

    private CharacterController controller;
    private float cameraPitch = 0f;
    private Rigidbody heldObject;

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

            controller.Move((transform.forward * moveY + transform.right * moveX) * transform.localScale.magnitude);
		}

		{   // Rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            cameraPitch = Mathf.Clamp(cameraPitch - mouseY, -90.0f, 90.0f);

            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0.0f, 0.0f);
            transform.Rotate(0.0f, mouseX, 0.0f);
		}

		{   // Pickup
            if (Input.GetKeyDown(KeyCode.E))
			{
                if (heldObject)
				{
                    heldObject.useGravity = true;
                    heldObject = null;
				}
				else if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, maxReach, LayerMask.GetMask("Interactable")))
				{
                    heldObject = hit.rigidbody;
                    heldObject.useGravity = false;
				}
			}
		}
    }

	private void FixedUpdate()
	{
        if (heldObject)
		{
            heldObject.velocity = (holdPoint.position - heldObject.position) * 10.0f;
		}

	}
}
