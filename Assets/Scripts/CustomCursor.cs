using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    [Header("Click Effect Settings")]
    public GameObject clickEffectPrefab;
    public AudioClip clickSound;
    public float soundVolume = 0.5f;

    private Camera mainCamera;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleClickEffect();
    }

    void HandleClickEffect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            if (clickEffectPrefab != null)
            {
                GameObject spawned = Instantiate(clickEffectPrefab, mousePos, Quaternion.identity);
                if (spawned != null)
                {
                    spawned.SetActive(true);

                    ParticleSystem ps = spawned.GetComponent<ParticleSystem>();
                    if (ps == null)
                        ps = spawned.GetComponentInChildren<ParticleSystem>();

                    if (ps != null)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        ps.Play(true);

                        var psr = ps.GetComponent<ParticleSystemRenderer>();
                        if (psr != null)
                        {
                            psr.sortingOrder = 5000;
                        }
                    }

                    Destroy(spawned, 1f);
                }
            }

            if (clickSound != null)
            {
                AudioSource.PlayClipAtPoint(clickSound, mousePos, soundVolume);
            }
        }
    }
}
