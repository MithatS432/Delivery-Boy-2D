using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target;
    private Animator anim;
    private SpriteRenderer sp;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float viewRange = 5f;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float waitTime = 2f;
    private float waitTimer;

    public bool isChasing = false;
    public bool isPatrolling = true;
    public bool isInView = false;
    public bool canChase = true;

    [Header("Audio")]
    public AudioClip chaseSound;
    private bool hasPlayedChaseSound = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        waitTimer = waitTime;

        GameObject patrolParent = GameObject.Find("PatrolPoints");
        if (patrolParent != null)
        {
            patrolPoints = new Transform[patrolParent.transform.childCount];
            for (int i = 0; i < patrolParent.transform.childCount; i++)
            {
                patrolPoints[i] = patrolParent.transform.GetChild(i);
            }

            ShufflePatrolPoints();
        }
        else
        {
            Debug.LogWarning("PatrolPoints parent objesi bulunamadı!");
        }
    }

    void ShufflePatrolPoints()
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            int randIndex = Random.Range(i, patrolPoints.Length);
            Transform temp = patrolPoints[i];
            patrolPoints[i] = patrolPoints[randIndex];
            patrolPoints[randIndex] = temp;
        }
    }


    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        isInView = distance < viewRange;

        if (canChase && isInView)
        {
            isChasing = true;
            isPatrolling = false;
            Vector2 movementDelta = ChasePlayer();
            UpdateAnimation(movementDelta);
        }
        else
        {
            isChasing = false;
            if (!isPatrolling)
            {
                isPatrolling = true;
                waitTimer = waitTime;
            }
            Vector2 movementDelta = Patrol();
            UpdateAnimation(movementDelta);
        }
    }

    Vector2 ChasePlayer()
    {
        if (!hasPlayedChaseSound && chaseSound != null)
        {
            AudioSource.PlayClipAtPoint(chaseSound, transform.position, 1f);
            hasPlayedChaseSound = true;
        }

        Vector2 oldPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Vector2 movementDelta = (Vector2)transform.position - oldPos;

        if (movementDelta.x != 0)
            sp.flipX = movementDelta.x < 0;

        return movementDelta;
    }

    Vector2 Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return Vector2.zero;
        hasPlayedChaseSound = false;

        Transform patrolTarget = patrolPoints[currentPatrolIndex];
        Vector2 oldPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget.position, speed * Time.deltaTime);
        Vector2 movementDelta = (Vector2)transform.position - oldPos;

        if (movementDelta.x != 0)
            sp.flipX = movementDelta.x < 0;

        if (Vector2.Distance(transform.position, patrolTarget.position) < 0.2f)
        {
            if (waitTimer <= 0f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                waitTimer = waitTime;
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
        }

        return movementDelta;
    }

    void UpdateAnimation(Vector2 movementDelta)
    {
        float speedValue = movementDelta.magnitude / Time.deltaTime;

        anim.SetFloat("Speed", speedValue);

        bool moving = speedValue > 0.01f;
        anim.SetBool("IsMoving", moving);
    }

}
