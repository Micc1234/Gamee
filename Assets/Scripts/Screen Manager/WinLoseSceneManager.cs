using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class WinLoseSceneManager : MonoBehaviour
{
    private Button mainMenuButton;

    void OnEnable()
    {
        // Ambil root visual element dari UIDocument
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Ambil button berdasarkan nama (harus diawali dengan '#' di UXML: #MainMenuBtn)
        mainMenuButton = root.Q<Button>("MainMenuBtn");

        if (mainMenuButton != null)
        {
            mainMenuButton.clicked += () =>
            {
                // Load scene MainMenuScreen saat tombol ditekan
                SceneManager.LoadScene("MainMenuScreen");
            };
        }
        else
        {
            Debug.LogWarning("MainMenuBtn tidak ditemukan di UXML!");
        }
    }
}
