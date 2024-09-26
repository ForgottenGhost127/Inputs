using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementspeed = 5;
    public float rotationspeed = 200;
    public float dashSpeed = 15.0f; 
    public float dashDuration = 0.5f; 
    public float dashCooldown = 1.0f; 
    public float jumpForce = 7.0f; 
    public float gravity = -9.8f;
    public Transform cameraTrans;

    public float maxHoldTime = 1.5f;
    public float holdForce = 9.0f;

    private float verticalVelocity = 0f;
    private bool isGrounded = true;
    private int jumpCount = 0;
    private Vector3 moveDirection;

    private float groundPosY = -0.5f;
    private float cubeHeight = 0.5f;

    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    private bool canHoldJump = false;
    private float jumpHoldTime = 0f;

    private Renderer cubeRenderer;
    private Color originalColor = Color.white;
    public Color dashColor = Color.cyan;
    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }
    void Update()
    {
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * movementspeed);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * movementspeed);

        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * rotationspeed);

        HandleJumpAndGravity();

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
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
                canHoldJump = true;
                jumpHoldTime = 0f;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (canHoldJump && Input.GetKey(KeyCode.Space))
            {
                if (jumpHoldTime < maxHoldTime)
                {
                    verticalVelocity += holdForce * Time.deltaTime;
                    jumpHoldTime += Time.deltaTime;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                canHoldJump = false;
            }

            if (jumpCount < 2 && Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                jumpCount++;
                canHoldJump = true;
                jumpHoldTime = 0f;
            }
        }

        transform.Translate(Vector3.up * verticalVelocity * Time.deltaTime);

        if (transform.position.y - cubeHeight <= groundPosY)
        {
            transform.position = new Vector3(transform.position.x, groundPosY + cubeHeight, transform.position.z);
            isGrounded = true;
            jumpCount = 0;
            canHoldJump = false;
        }
    }

    //Ejercicio 2: Dash en dirección del movimiento
    IEnumerator PerformDash()
    {
        isDashing = true;
        cubeRenderer.material.color = dashColor;
        Vector3 dashDirection = transform.forward;
        float dashTime = 0f;

        while (dashTime < dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            dashTime += Time.deltaTime;
            yield return null;
        }

        cubeRenderer.material.color = originalColor;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
    }
}
