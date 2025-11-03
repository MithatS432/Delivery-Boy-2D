using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Inventory")]
    public GameObject inventoryPanel;
    private bool isInventoryOpen = false;
    public TMP_Text margheritaText;
    public TMP_Text pepperoniText;
    public TMP_Text hawaiianText;
    public TMP_Text bbqChickenText;
    public TMP_Text buffaloChickenText;
    public TMP_Text totalText;

    public int margheritaCount = 29;
    public int pepperoniCount = 17;
    public int hawaiianCount = 14;
    public int bbqChickenCount = 30;
    public int buffaloChickenCount = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? 8f : 5f;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }
    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
            rb.linearVelocity = Vector2.zero;
        if (isInventoryOpen)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateInventoryUI();
        }
    }
    void UpdateInventoryUI()
    {
        margheritaText.text = "Margherita  " + margheritaCount.ToString();
        pepperoniText.text = "Pepperoni " + pepperoniCount.ToString();
        hawaiianText.text = "Hawaiian " + hawaiianCount.ToString();
        bbqChickenText.text = "BBQ Chicken " + bbqChickenCount.ToString();
        buffaloChickenText.text = "Buffalo Chicken " + buffaloChickenCount.ToString();
        totalText.text = "Total: " + TotalPizzaCount.ToString();
    }
    public int TotalPizzaCount
    {
        get
        {
            return margheritaCount + pepperoniCount + hawaiianCount + bbqChickenCount + buffaloChickenCount;
        }
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
