using UnityEngine;
using UnityEngine.UI;
using TMPro; // Untuk TextMeshPro
using UnityEngine.SceneManagement; // Untuk berpindah scene

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnAreaWidth = 10f;
    public float spawnAreaLength = 10f;
    public float spawnY = 1.74f;
    public int totalEnemiesToSpawn = 10;
    public TextMeshProUGUI enemyCountText;

    public float gameDuration = 60f;
    public TextMeshProUGUI gameTimerText;
    public GameObject gameOverPanel;
    public GameObject gameWonPanel;

    private int remainingEnemies;
    private float timeRemaining;
    private bool gameEnded = false;

    void Start()
    {
        remainingEnemies = totalEnemiesToSpawn;
        UpdateEnemyCountUI();

        timeRemaining = gameDuration;
        UpdateGameTimerUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(false);
        }

        SpawnAllEnemies();
    }

    void Update()
    {
        if (!gameEnded)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateGameTimerUI();

                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    UpdateGameTimerUI();
                    GameOver();
                }
            }
        }
    }

    private void SpawnAllEnemies()
    {
        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
            float randomZ = Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2);
            Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("EnemyPrefab belum diatur di Inspector!");
        }
    }

    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies : " + remainingEnemies.ToString();
        }
    }

    private void UpdateGameTimerUI()
    {
        if (gameTimerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            gameTimerText.text = string.Format("Time : {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void OnEnemyDestroyed()
    {
        if (gameEnded) return;

        remainingEnemies--;
        UpdateEnemyCountUI();

        if (remainingEnemies <= 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("You Win! All enemies defeated!");
        Time.timeScale = 1f; // Pastikan waktu normal
        SceneManager.LoadScene("WinScene"); // Ganti scene ke WinScene
    }

    private void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("Game Over! Time's up!");
        Time.timeScale = 1f; // Pastikan waktu normal
        SceneManager.LoadScene("LoseScene"); // Ganti scene ke LoseScene
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
