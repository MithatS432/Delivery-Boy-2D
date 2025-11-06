using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.z);
    }
}
