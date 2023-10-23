using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovevent : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam, groundCheck;
    public float groundDistance = 0.4f;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    bool hasDoubleJumped = false;
    Vector3 velocity;
    bool isGrounded;
    bool inAir;
    public LayerMask groundMask;
    public float jumpCharge;
    private bool isCharging;

    void Update()
    {
        jumpCharge = Mathf.Clamp(jumpCharge, 0f, 2f);
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

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

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
            jumpCharge += Time.deltaTime;
            isCharging = true;
        }
        if (GameManager.inst.inputManager.Giocatore.Salto.WasReleasedThisFrame())
        {
            if (isGrounded && isCharging)
            {
                velocity.y = Mathf.Sqrt(jumpCharge * -6 * gravity);
                jumpCharge = 0f;
                isCharging = false;
            }
            else if (!hasDoubleJumped && inAir)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                hasDoubleJumped = true;
            }
        }
    }
}

