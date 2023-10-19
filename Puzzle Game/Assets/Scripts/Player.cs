using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject pauseMenu; 
    public Camera playerCamera;
    public Transform holdPoint;
    public float maxReach = 5.0f;
    public float mouseSensitivity = 100f;
    public float moveSpeed = 10f;
    public Rigidbody heldObject;

    private CharacterController controller;
    private Light playerLight;
    private float cameraPitch = 0f;
    private bool paused = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerLight = GetComponentInChildren<Light>();
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
                    heldObject.gameObject.layer = LayerMask.NameToLayer("Interactable");
                    heldObject.useGravity = true;
                    heldObject = null;
				}
				else if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, maxReach * Mathf.Abs(transform.localScale.x), LayerMask.GetMask("Interactable")))
				{
                    heldObject = hit.rigidbody;
                    heldObject.useGravity = false;
                    heldObject.gameObject.layer = LayerMask.NameToLayer("Duplicates");
				}
			}
		}

		{   // Escape Menu
            if (Input.GetKeyDown("escape"))
            {
                paused = !paused;

                pauseMenu.SetActive(paused);
                Time.timeScale = paused ? 0f : 1f;
                Cursor.lockState = paused ? CursorLockMode.Confined : CursorLockMode.Locked;
            }
        }

		{   // Prevent Player from floating
            controller.Move(new(0f, -1f, 0f));
		}

		{
            playerLight.range = Mathf.Abs(transform.localScale.x) * 15f;
            //zplayerLight.intensity = Mathf.Abs(transform.localScale.x) * 6f;
        }
    }

	private void FixedUpdate()
	{
        if (heldObject)
		{
            heldObject.velocity = (holdPoint.position - heldObject.position) * 15.0f;
		}

	}

    public void ExitGame()
    {
        Application.Quit();
    }
}
