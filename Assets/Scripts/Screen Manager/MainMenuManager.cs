using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;

    private Button easyBtn;
    private Button hardBtn;
    private Button quitBtn;

    void Start()
    {
        var root = uiDoc.rootVisualElement;

        easyBtn = root.Q<Button>("EasyBtn");
        hardBtn = root.Q<Button>("HardBtn");
        quitBtn = root.Q<Button>("QuitBtn");

        if (easyBtn != null)
            easyBtn.clicked += () => LoadGameScene("EasyScene");

        if (hardBtn != null)
            hardBtn.clicked += () => LoadGameScene("HardScene");

        if (quitBtn != null)
            quitBtn.clicked += QuitGame;
    }

    private void LoadGameScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' belum ditambahkan ke Build Settings!");
        }
    }

    private void QuitGame()
    {
        Debug.Log("Keluar dari aplikasi.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
