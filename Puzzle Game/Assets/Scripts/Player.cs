using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera camera;
    public float speed = 1.0f;
    public float angularSpeed = 1.0f;

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new();
        if (Input.GetKey(KeyCode.W)) movement += transform.forward * speed;
        else if (Input.GetKey(KeyCode.S)) movement += transform.forward * -speed;
        if (Input.GetKey(KeyCode.D)) movement += transform.right * speed;
        else if (Input.GetKey(KeyCode.A)) movement += transform.right * -speed;

        movement.x *= transform.localScale.x;
        movement.y *= transform.localScale.y;
        movement.z *= transform.localScale.z;

        rigidbody.AddForce(movement * Time.deltaTime);

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * angularSpeed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * angularSpeed;

        float xRotation = camera.transform.localEulerAngles.x;
		float yRotation = transform.localRotation.eulerAngles.y;

		yRotation += mouseX;

        xRotation -= mouseY;
        

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
