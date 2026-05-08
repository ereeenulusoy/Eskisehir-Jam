using UnityEngine;
using UnityEngine.SceneManagement; // Sahneler arasý geçiþ için þart!

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // 1 numaralý sahneyi (Asýl oyun sahneni) yükler
        SceneManager.LoadScene(1);
    }

    public void QuitApplication()
    {
        // Oyunu kapatýr (Unity Editor'de çalýþmaz, sadece Build alýnca çalýþýr)
        Application.Quit();
        Debug.Log("Oyun kapatýldý!");
    }
}