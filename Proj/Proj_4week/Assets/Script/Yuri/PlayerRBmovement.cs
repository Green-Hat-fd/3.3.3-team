using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerRBmovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam, groundCheck;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public LayerMask groundMask;
    public bool isGrounded;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    //private bool isJumping = false; // Nuova variabile per gestire il salto
    [SerializeField] private float jumpCharge; // Per vedere dall'Inspector se sta caricando, non va modificato
    [SerializeField] private float maxJumpCharge = 2f; // Modifica questo valore per aumentare la carica del salto
    private bool hasDoubleJumped = false;

    Vector3 velocity;
    private bool inAir;
    private bool isCharging;
    private bool inWater;
    private bool isDashing = false;
    private float dashDuration = 0.5f;
    private float dashTimer;
    [SerializeField] private float dashDistance = 3.0f;
    private Vector3 dashDirection;
    private bool isMoving;
    private bool isRunning = false;
    [SerializeField] private float speedBonus = 9f;
    [SerializeField] private float speedStandard = 6f;
    [SerializeField] private float speedDecrease = 3f;

    //[SerializeField] private float interactionRange = 2.0f;
    [SerializeField] private LayerMask grabbableLayer;
    //private bool isGrabbed = false;

    void Update()
    {
        jumpCharge = Mathf.Clamp(jumpCharge, 0f, maxJumpCharge);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        float horizontal = GameManager.inst.inputManager.Giocatore.Movimento.ReadValue<Vector2>().x;
        float vertical = GameManager.inst.inputManager.Giocatore.Movimento.ReadValue<Vector2>().y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            rb.velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);

            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (!isMoving)
        {
            speed = speedStandard;
        }

        if (GameManager.inst.inputManager.Giocatore.Corsa.WasPressedThisFrame() && isMoving && isGrounded && !isRunning)
        {
            isRunning = true;
            speed = speedBonus;
        }

        velocity.y += gravity * Time.deltaTime;
        rb.velocity += velocity * Time.deltaTime;

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (1.5f - 1) * Time.deltaTime;
        }

        if (isGrounded)
        {
            hasDoubleJumped = false;
            inAir = false;
        }
        else
        {
            inAir = true;
        }

        if (GameManager.inst.inputManager.Giocatore.Salto.ReadValue<float>() > 0f)
        {
            //if (!isJumping)
            //{
            //isJumping = true;
            //Debug.Log("sto caricando il salto");
            transform.parent = null;
            isCharging = true;
            jumpCharge += Time.deltaTime * 11;
            //rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * -2 * gravity), rb.velocity.z);
            //}
        }

        if (GameManager.inst.inputManager.Giocatore.Salto.WasReleasedThisFrame())
        {
            if (isGrounded && isCharging)
            {
                //isGrabbed = false;
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpCharge * -6 * gravity), rb.velocity.z);
                isCharging = false;
                jumpCharge = 0f;
                //isJumping = false;
            }
            else if (!hasDoubleJumped && inAir)
            {
                //isGrabbed = false;
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * -2 * gravity), rb.velocity.z);
                hasDoubleJumped = true;
                //isJumping = false;
            }
        }

        if (GameManager.inst.inputManager.Giocatore.Nuoto.WasPressedThisFrame() && inWater)
        {
            Vector3 camForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
            dashDirection = camForward;
            Vector3 dashDestination = transform.position + dashDirection * dashDistance;

            if (!Physics.Raycast(transform.position, dashDirection, dashDistance))
            {
                isDashing = true;
                dashTimer = 0f;
            }
        }

        if (isDashing)
        {
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
            }
            else
            {
                rb.velocity = dashDirection * dashDistance / dashDuration;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isRunning = false;
        if (other.tag == "Water")
        {
            inWater = true;
            speed = speedDecrease;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            inWater = false;
            speed = speedStandard;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DamageDash();
        }
    }

    private void DamageDash()
    {
        Vector3 camForward = Vector3.Scale(-transform.forward, new Vector3(1, 0, 1)).normalized;
        dashDirection = camForward;
        Vector3 dashDestination = transform.position + dashDirection * dashDistance;

        if (!Physics.Raycast(transform.position, dashDirection, dashDistance))
        {
            isDashing = true;
            dashTimer = 0f;
        }
    }
}
