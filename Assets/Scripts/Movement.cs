using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementspeed = 5;
    public float rotationspeed = 200;
    public Transform cameraTrans;

    public float jumpForce = 7.0f; 
    public float gravity = -9.8f;

    private float verticalVelocity = 0f; 
    private bool isGrounded = true;
    private int jumpCount = 0;
    private Vector3 moveDirection;

    private float groundPosY = -0.5f;
    private float cubeHeight = 0.5f;
    void Update()
    {
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * movementspeed);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * movementspeed);

        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * rotationspeed);

        HandleJumpAndGravity();
    }

    //Ejercicio 1: Salto y Doble salto
    void HandleJumpAndGravity() 
    {
        if (isGrounded)
        {
            verticalVelocity = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                jumpCount = 1;
                isGrounded = false;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < gravity)
            {
                verticalVelocity = gravity;
            }

            if(jumpCount < 2 && Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                jumpCount++;
            }
        }

        transform.Translate(Vector3.up * verticalVelocity * Time.deltaTime);

        if (transform.position.y - cubeHeight <= groundPosY)
        {
            transform.position = new Vector3(transform.position.x, groundPosY + cubeHeight, transform.position.z);
            isGrounded = true;
            jumpCount = 0; 
        }
    }

    //Ejercicio 2: Dash en dirección del movimiento

}
