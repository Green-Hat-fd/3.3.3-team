using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VespaSparo : MonoBehaviour
{
    private float Distance;
    private Transform target;
    public float lookAtDistance = 15.0f;
    public float attackRange = 12.0f;
    public float patrolRadius = 10.0f;
    public float stopRadius = 5.0f;
    public float moveSpeed = 5.0f;
    public float Damping = 6.0f;
    public float obstacleCheckDistance = 1.5f;
    [SerializeField] private GameObject player;
    private bool isAttacking = false;
    private bool inSight = false;
    private Vector3 randomPatrolPoint;

    [SerializeField] private GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.4f; // Un colpo ogni 3 secondi
    private float lastFireTime;
    [SerializeField] private float bulletSpeed = 80;

    private bool isPatrolling = false;
    public float patrolInterval = 5.0f; // Intervallo tra le azioni di perlustrazione

    [SerializeField] private Animator anim;

    private void Awake()
    {
        target = player.transform;
        SetRandomPatrolPoint(); // Imposta il primo punto di perlustrazione casuale
    }

    void LookAt()
    {
        if (inSight) 
        { 
        var rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);            
        }

    }

    void Update()
    {
        if (!PauseMenu.gameIsPaused)
        {
            Distance = Vector3.Distance(target.position, transform.position);

            if (Distance < lookAtDistance)
            {
                inSight = true;
                LookAt();
            }

            if (Distance < attackRange)
            {
                inSight = true;
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }

            if (isAttacking)
            {
                Attack();
            }
            else
            {
                if (!isPatrolling)
                {
                    StartCoroutine(PatrolWithDelay(patrolInterval));
                }
            }
        }
        else
        {
            // Se il giocatore esce dal range di attacco, riprendi la perlustrazione.
            if (Distance > attackRange)
            {
                isAttacking = false;
                if (!isPatrolling)
                {
                    StartCoroutine(PatrolWithDelay(patrolInterval));
                }
            }
        }
    }

    IEnumerator PatrolWithDelay(float delay)
    {
        isPatrolling = true;
        yield return new WaitForSeconds(delay);
        Patrol();
        isPatrolling = false;
    }


    void Patrol()
    {
        // Calcola la direzione verso il punto casuale di perlustrazione.
        Vector3 moveDirection = randomPatrolPoint - transform.position;
        transform.rotation = Quaternion.LookRotation(moveDirection);

        // Verifica se il ragno è abbastanza vicino al punto di perlustrazione.
        if (Vector3.Distance(transform.position, randomPatrolPoint) < 1.0f)
        {
            // Se è vicino al punto, imposta un nuovo punto casuale di perlustrazione.
            SetRandomPatrolPoint();
        }
        else
        {
            // Sposta il ragno in avanti con una velocità maggiore.
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    bool IsBlocked()
    {
        // Controlla se ci sono ostacoli davanti al ragno.
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.Raycast(ray, obstacleCheckDistance);
    }

    void Attack()
    {
        inSight = false;
        if (Time.time - lastFireTime >= 1 / fireRate)
        {
            if (Distance > stopRadius)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = (target.position - firePoint.position).normalized * bulletSpeed;
            //anim.SetTrigger("onAttack");
            lastFireTime = Time.time; // Aggiorna il tempo dell'ultimo sparo

            anim.SetTrigger("attack");
        }
    }

    void SetRandomPatrolPoint()
    {
        // Genera un punto casuale all'interno del raggio di perlustrazione.
        randomPatrolPoint = transform.position + Random.insideUnitSphere * patrolRadius;
        randomPatrolPoint.y = transform.position.y; // Mantieni la stessa altezza.
    }
}
