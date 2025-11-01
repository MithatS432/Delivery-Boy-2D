using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sp;

    [Header("Audios")]
    public AudioClip runSound, pizzaSound, checkPointSound;
    private float soundCooldown = 0.3f;
    private float soundTimer;


    [Header("Effects")]
    public GameObject runEffect;
    private float effectCooldown = 0.1f;
    private float effectTimer;

    [Header("Character Settings")]
    public float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? 8f : 5f;
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x < 0)
            sp.flipX = true;
        else if (x > 0)
            sp.flipX = false;

        Vector2 moveDir = new Vector2(x, y).normalized;
        rb.linearVelocity = moveDir * speed;
        anim.SetFloat("Speed", moveDir.magnitude);

        if (moveDir.magnitude > 0)
        {
            if (soundTimer <= 0f)
            {
                AudioSource.PlayClipAtPoint(runSound, transform.position, 1f);
                soundTimer = soundCooldown;
            }

            if (effectTimer <= 0f)
            {
                Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y - 0.1f, -1f);
                GameObject run = Instantiate(runEffect, spawnPos, Quaternion.identity);
                Destroy(run, 1f);
                effectTimer = effectCooldown;
            }
        }
        soundTimer -= Time.fixedDeltaTime;
        effectTimer -= Time.fixedDeltaTime;


    }
}
