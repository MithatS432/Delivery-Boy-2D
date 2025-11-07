using UnityEngine;

public class WinEffect : MonoBehaviour
{
    public GameObject winEffectPrefab;

    public void TriggerWinEffect()
    {
        Camera cam = Camera.main;
        if (cam != null && winEffectPrefab != null)
        {
            Instantiate(winEffectPrefab, cam.transform.position, Quaternion.identity);
        }
    }
}

