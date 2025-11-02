using UnityEngine;
using UnityEngine.UI;

public class DayNightCycleTimed : MonoBehaviour
{
    public Image skyImage;
    public Sprite[] daySprites;

    [Tooltip("Her bir sprite'ın ekranda kalacağı süre (saniye).")]
    public float timePerSprite = 30f; 

    private float timer = 0f;
    private int currentSpriteIndex = 0;

    void Start()
    {
        if (daySprites.Length > 0)
        {
            skyImage.sprite = daySprites[0];
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timePerSprite)
        {
            timer -= timePerSprite; 

            currentSpriteIndex++;

            if (currentSpriteIndex >= daySprites.Length)
            {
                currentSpriteIndex = 0;
            }

            if (daySprites.Length > 0)
            {
                skyImage.sprite = daySprites[currentSpriteIndex];
            }
        }
    }
}