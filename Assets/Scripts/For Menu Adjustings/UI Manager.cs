using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject[] images;
    private bool isPaused = false;
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
    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ContinueGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}
