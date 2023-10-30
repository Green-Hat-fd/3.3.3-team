using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagnoIA : MonoBehaviour
{
    private float Distance;
    private Transform target;
    public float lookAtDistance = 15.0f;
    public float attackRange = 12.0f;
    public float patrolRadius = 10.0f; // Raggio in cui il ragno perlustra
    public float moveSpeed = 5.0f;
    public float Damping = 6.0f;
    public float obstacleCheckDistance = 1.5f;
    [SerializeField] private GameObject player;
    private bool isAttacking = false;
    private bool isStopped = false;
    private Vector3 randomPatrolPoint; // Il punto casuale di perlustrazione

    private void Awake()
    {
        target = player.transform;
        SetRandomPatrolPoint(); // Imposta il primo punto di perlustrazione casuale
    }

    void LookAt()
    {
        var rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
    }

    void Update()
    {
      if (!PauseMenu.gameIsPaused)
       {
        if (!isStopped)
        {
            Distance = Vector3.Distance(target.position, transform.position);
            if (Distance < lookAtDistance)
            {
                LookAt();
            }

            if (Distance < attackRange)
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }

            if (isAttacking)
            {
                moveSpeed = 5.0f;
                Attack();
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            // Se il giocatore esce dal range di attacco, riprendi la perlustrazione.
            if (Distance > attackRange)
            {
                isAttacking = false;
                Patrol();
            }
        }
      }
        
    }


    void Patrol()
    {
        // Muovi il ragno verso il punto casuale di perlustrazione.
        Vector3 moveDirection = randomPatrolPoint - transform.position;
        transform.rotation = Quaternion.LookRotation(moveDirection);

        if (!IsBlocked())
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Se bloccato, seleziona una nuova direzione casuale.
            SetRandomPatrolPoint();
        }
    }

    bool IsBlocked()
    {
        // Controlla se ci sono ostacoli davanti al ragno.
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.Raycast(ray, obstacleCheckDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isStopped = true;
            StartCoroutine(StopForSeconds(2.0f));
        }
    }

    void Attack()
    {
        // Logica di attacco del ragno
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void SetRandomPatrolPoint()
    {
        // Genera un punto casuale all'interno del raggio di perlustrazione.
        randomPatrolPoint = transform.position + Random.insideUnitSphere * patrolRadius;
        randomPatrolPoint.y = transform.position.y; // Mantieni la stessa altezza.
    }

    IEnumerator StopForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isStopped = false;
    }
}
