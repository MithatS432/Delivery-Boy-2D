using UnityEngine;
using UnityEngine.UI;

public class DayNightCycleController : MonoBehaviour
{
    [Header("Sky Textures")]
    public Image skyImage;
    public Sprite[] skySprites = new Sprite[8];

    [Header("Settings")]
    public float timePerSprite = 30f;

    private float timer = 0f;
    private int currentSpriteIndex = 0;

    void Start()
    {
        if (skySprites.Length > 0)
            skyImage.sprite = skySprites[0];
    }

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
        if (currentSpriteIndex >= skySprites.Length)
            currentSpriteIndex = 0;

        skyImage.sprite = skySprites[currentSpriteIndex];
    }
}
