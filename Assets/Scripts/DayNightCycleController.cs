using UnityEngine;
using UnityEngine.UI;

public class DayNightCycleController : MonoBehaviour
{
    [Header("Sky Textures")]
    public Image skyImage; 
    public Sprite[] daySprites; 
    public Sprite[] nightSprites;
    
    [Header("Sun/Moon Prefabs")]
    public GameObject[] sunPrefabs; 
    public GameObject[] moonPrefabs; 
    
    [Header("Settings")]
    public float timePerSprite = 30f; 
    public RectTransform skyObjectContainer; 
    
    [Tooltip("Güneş ve Ay prefablarının görüneceği ölçek (Örn: 2 kat büyütmek için 2)")]
    public float skyObjectScale = 2f; // Boyut sorununu çözmek için eklendi.

    // --- Private Variables ---
    private float timer = 0f;
    private int currentSpriteIndex = 0;
    private GameObject currentSkyObject;
    // Hangi tam gün döngüsünde (1. gün, 2. gün, 3. gün...) olduğumuzu tutar.
    private int currentDayCycle = 0; 

    void Start()
    {
        InitializeCycle();
    }

    void InitializeCycle()
    {
        if (daySprites.Length > 0)
        {
            skyImage.sprite = daySprites[0];
            // Başlangıçta 0. gün (1. gün) için 0 indexli prefabı oluştur.
            SpawnSkyObject(currentDayCycle, true);
        }
    }

    // Her frame çalışacak fonksiyon
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timePerSprite)
        {
            timer = 0f;
            AdvanceCycle();
        }
    }

    void AdvanceCycle()
    {
        currentSpriteIndex++;
        
        int totalSprites = daySprites.Length + nightSprites.Length;
        if (currentSpriteIndex >= totalSprites)
        {
            currentSpriteIndex = 0;
            
            // Tam bir gün (Gündüz+Gece) döngüsü bitti, gün sayacını artır.
            currentDayCycle++; 
        }

        UpdateSkyAndObjects();
    }

    void UpdateSkyAndObjects()
    {
        bool isDaytime = currentSpriteIndex < daySprites.Length;
        int textureIndex = currentSpriteIndex;

        if (isDaytime)
        {
            // Gündüz sprite'ını ayarla
            skyImage.sprite = daySprites[textureIndex];
            
            // SADECE GÜNDÜZ BAŞLANGICINDA (ilk sprite'ta) Güneşi oluştur/değiştir.
            if (textureIndex == 0) 
            {
                SpawnSkyObject(currentDayCycle, true); 
            }
        }
        else
        {
            // Gece sprite'ını ayarla
            textureIndex = currentSpriteIndex - daySprites.Length;
            skyImage.sprite = nightSprites[textureIndex];
            
            // SADECE GECE BAŞLANGICINDA (gecenin ilk sprite'ında) Ay'ı oluştur/değiştir.
            if (textureIndex == 0) 
            {
                SpawnSkyObject(currentDayCycle, false);
            }
        }
    }

    void SpawnSkyObject(int cycleIndex, bool isSun)
    {
        // Önceki gök cismini yok et.
        if (currentSkyObject != null)
        {
            Destroy(currentSkyObject);
        }

        GameObject[] prefabArray = isSun ? sunPrefabs : moonPrefabs;

        // Prefab dizisi boşsa ya da atama yapılmamışsa devam etme.
        if (prefabArray.Length == 0) return;
        
        // Mod (%) operatörünü kullanarak dizinin dışına çıkmasını engelle.
        int indexToUse = cycleIndex % prefabArray.Length; 
        
        GameObject prefabToSpawn = prefabArray[indexToUse]; 
        
        if (prefabToSpawn != null && skyObjectContainer != null)
        {
            // Yeni prefabı oluştur ve currentSkyObject olarak ata.
            currentSkyObject = Instantiate(prefabToSpawn, skyObjectContainer);

            // UI elemanı olduğu için RectTransform ayarları.
            RectTransform rt = currentSkyObject.GetComponent<RectTransform>();
            if (rt != null)
            {
                // POZİSYON AYARLARI
                rt.anchorMin = new Vector2(0.5f, 0.8f); 
                rt.anchorMax = new Vector2(0.5f, 0.8f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = Vector2.zero; // X/Y merkezde
                
                // Z POZİSYONUNU SIFIRLA (Nadiren görünürlük sorununu çözebilir)
                rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0f);
                
                // ÖLÇEK AYARI (Boyut sorununu çözmek için)
                rt.localScale = new Vector3(skyObjectScale, skyObjectScale, 1f); 
                
                // ÇİZİM SIRASI (Görünürlük sorununu çözmek için)
                // Objenin, ebeveyni (skyObjectContainer) içindeki en son (en ön) obje olmasını sağla.
                rt.SetAsLastSibling(); 
            }
        }
    }
}