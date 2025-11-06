using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sp;

    [Header("Audios")]
    public AudioClip runSound, pizzaSound;
    private float soundCooldown = 0.3f;
    private float soundTimer;
    public AudioClip dogBarkSound;

    [Header("Effects")]
    public GameObject runEffect;
    private float effectCooldown = 0.1f;
    private float effectTimer;

    [Header("Character Settings")]
    public float speed = 5f;
    public float loadSpeed = 20f;
    public Image pizzaLoader;
    public Image[] healthImages;
    public int health = 2;
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button quitButton;
    public bool isInMud = false;

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

    private bool isAtHouse = false;
    private bool isLoading = false;
    private float loadProgress = 0f;

    [Header("Houses")]
    public GameObject[] houses;
    public int currentHouseIndex = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        HandleInventory();

        if (isAtHouse)
        {
            if (Input.GetKey(KeyCode.Space))
                StartLoading();
            else
                StopLoading();
        }

        if (isLoading)
        {
            loadProgress += loadSpeed * Time.deltaTime;
            pizzaLoader.fillAmount = loadProgress / 100f;

            if (loadProgress >= 100f)
                CompleteLoading();
        }
    }

    void FixedUpdate()
    {
        soundTimer -= Time.fixedDeltaTime;
        effectTimer -= Time.fixedDeltaTime;
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        float baseSpeed = isInMud ? 2f : 5f;
        speed = Input.GetKey(KeyCode.LeftShift) ? baseSpeed * 2f : baseSpeed;

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
    }

    private void HandleInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
            if (isInventoryOpen)
            {
                rb.linearVelocity = Vector2.zero;
                UpdateInventoryUI();
            }
        }
    }

    private void UpdateInventoryUI()
    {
        margheritaText.text = "Margherita  " + margheritaCount.ToString();
        pepperoniText.text = "Pepperoni " + pepperoniCount.ToString();
        hawaiianText.text = "Hawaiian " + hawaiianCount.ToString();
        bbqChickenText.text = "BBQ Chicken " + bbqChickenCount.ToString();
        buffaloChickenText.text = "Buffalo Chicken " + buffaloChickenCount.ToString();
        totalText.text = "Total: " + TotalPizzaCount.ToString();
    }

    public int TotalPizzaCount =>
        margheritaCount + pepperoniCount + hawaiianCount + bbqChickenCount + buffaloChickenCount;

    private void StartLoading()
    {
        if (!isLoading)
        {
            isLoading = true;
            loadProgress = 0f;
            pizzaLoader.fillAmount = 0f;
            pizzaLoader.gameObject.SetActive(true);
        }
    }

    private void StopLoading()
    {
        isLoading = false;
        if (pizzaLoader != null && pizzaLoader.gameObject != null)
            pizzaLoader.gameObject.SetActive(false);
    }

    private void CompleteLoading()
    {
        isLoading = false;
        if (pizzaLoader != null && pizzaLoader.gameObject != null)
            pizzaLoader.gameObject.SetActive(false);

        AudioSource.PlayClipAtPoint(pizzaSound, transform.position, 1f);

        if (currentHouseIndex < houses.Length)
        {
            houses[currentHouseIndex].SetActive(false);
            currentHouseIndex++;
        }

        if (currentHouseIndex >= houses.Length)
        {
            Debug.Log("Tüm evler tamamlandı!");
            currentHouseIndex = houses.Length - 1;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("House"))
        {
            if (currentHouseIndex < houses.Length &&
                collision.gameObject == houses[currentHouseIndex])
            {
                isAtHouse = true;
            }
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            health--;
            if (health >= 0 && health < healthImages.Length)
            {
                healthImages[health].enabled = false;
            }
            if (health <= 0)
            {
                gameOverPanel.SetActive(true);
                Time.timeScale = 0f;
                restartButton.onClick.AddListener(() =>
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
});

                quitButton.onClick.AddListener(() =>
                {
                    Application.Quit();
                });
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("House"))
        {
            isAtHouse = false;
            StopLoading();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bark"))
        {
            AudioSource.PlayClipAtPoint(dogBarkSound, transform.position, 1f);
        }
        if (other.gameObject.CompareTag("Mud"))
        {
            isInMud = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mud"))
        {
            isInMud = false;
        }
    }
}
