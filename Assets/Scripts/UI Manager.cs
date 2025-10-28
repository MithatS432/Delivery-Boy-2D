using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject[] images;
    public void StartGame()
    {
        SceneManager.LoadScene("Town");
    }
    public void Options()
    {
        foreach (GameObject img in images)
        {
            img.SetActive(!img.activeSelf);
        }
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
