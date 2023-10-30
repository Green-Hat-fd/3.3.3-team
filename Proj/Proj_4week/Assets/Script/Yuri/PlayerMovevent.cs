using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovevent : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam, groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f; //il movimento e' gestito per far girare il player in base alla camera, cambia questo valore per aumentare la rotazione
    private float turnSmoothVelocity;
    [SerializeField] private float gravity = -9.81f;
    
    [SerializeField] private float jumpHeight = 3f; //serve preincipalmente per l'altezza del doppio salto
    private bool hasDoubleJumped = false;
    Vector3 velocity;
    private bool isGrounded;
    private bool inAir;
    public LayerMask groundMask;
    [SerializeField] private float jumpCharge; //per vedere dall'inspector se sta caricando, non va modificato
    [SerializeField] private float maxJumpCharge = 2f; //modifica questo valore per aumentare la carica del salto
    private bool isCharging;
    private bool inWater;

    private bool isDashing = false;
    private float dashDuration = 0.5f;
    private float dashTimer;
    [SerializeField] private float dashDistance = 3.0f; //serve per la spinta in acqua soltanto
    private Vector3 dashDirection;

    private bool isMoving;
    private bool isRunning = false;
    [SerializeField] private float speedBonus = 9f;
    [SerializeField] private float speedStandard = 6f;
    [SerializeField] private float speedDecreese = 3f;

    void Update()
    {
        if (!PauseMenu.gameIsPaused || !DialogoScript.dialogueActive)
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

                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                isMoving = true;

            }

            else { isMoving = false; }

            if (!isMoving)
            {
                speed = speedStandard;
            }

            if (GameManager.inst.inputManager.Giocatore.Corsa.WasPressedThisFrame() && isMoving && isGrounded && !isRunning) //dopo esser stato premuto una volta aumenta la velocita' finche' non ci si ferma. Ovviamente non funziona in acqua
            {
                isRunning = true;
                speed = speedBonus;
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
            if (GameManager.inst.inputManager.Giocatore.Salto.ReadValue<float>() > 0f) //a ogni frame aumenta il valore del salto che e' limitato dal clamp a inizio Update. Cambia il valore di maxJump se vuoi che salti piu' in alto
            {
                transform.parent = null;
                jumpCharge += Time.deltaTime;
                isCharging = true;
            }

            if (GameManager.inst.inputManager.Giocatore.Salto.WasReleasedThisFrame())
            {
                if (isGrounded && isCharging)
                {
                    //isGrabbed = false;
                    transform.parent = null;
                    velocity.y = Mathf.Sqrt(jumpCharge * -6 * gravity);
                    jumpCharge = 0f;
                    isCharging = false;
                }
                else if (!hasDoubleJumped && inAir)
                {
                    transform.parent = null;
                    //isGrabbed = false;
                    velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                    hasDoubleJumped = true;
                }
            }

            if (GameManager.inst.inputManager.Giocatore.Nuoto.WasPressedThisFrame() && inWater) //a fare da trigger non e' il terreno sotto ma l'acqua in trigger che sta di mezzo tra i due
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
                    controller.Move(dashDirection * Time.deltaTime * dashDistance / dashDuration);
                }
            }

        }
        else { }
    }

    private void OnTriggerStay(Collider other)
    {
        isRunning = false;
        if (other.tag == "Water") //ho messo un valore fisso perche' continuava a ridurre la velocita' a ogni frame, cosi' ho dato un valore fisso per andare sul sicuro (sia in stay che exit che lo riporta al suo valore iniziale)
        {
            inWater = true;
            speed = speedDecreese;
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
        Vector3 camForward = Vector3.Scale(- transform.forward, new Vector3(1, 0, 1)).normalized;
        dashDirection = camForward;
        Vector3 dashDestination = transform.position + dashDirection * dashDistance;

        if (!Physics.Raycast(transform.position, dashDirection, dashDistance))
        {
            isDashing = true;
            dashTimer = 0f;
        }
        
    }

    /*private void GrabHinge()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange, grabbableLayer);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Grab"))
            {
                transform.parent = collider.transform;
                isGrabbed = true;
                Debug.Log("grabbato");
            }
        }
    }*/
}