using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sp;

    [Header("Audios")]
    public AudioClip runSound, pizzaSound, checkPointSound;

    [Header("Effects")]
    public GameObject runEffect;

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

    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(x, y).normalized;
        rb.linearVelocity = moveDir * speed;

        if (moveDir.magnitude > 0)
        {
            GameObject run = Instantiate(
     runEffect,
     new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z - 0.01f),
     Quaternion.identity
 );

            Destroy(run, 1f);
        }
    }
}
