using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // Gunakan GameObject kosong sebagai titik spawn
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

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWonPanel != null) gameWonPanel.SetActive(false);

        SpawnAllEnemies();
    }

    void Update()
    {
        if (!gameEnded && timeRemaining > 0)
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

    private void SpawnAllEnemies()
    {
        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Enemy prefab atau spawn point belum diatur!");
            return;
        }

        // Pilih spawn point acak
        Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Tambahkan offset acak agar tidak terlalu menumpuk
        float offsetX = Random.Range(-4f, 4f);
        float offsetZ = Random.Range(-4f, 4f);

        Vector3 spawnPosition = new Vector3(
            chosenSpawnPoint.position.x + offsetX,
            spawnY,
            chosenSpawnPoint.position.z + offsetZ
        );

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
            gameTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("WinScene");
    }

    private void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("Game Over! Time's up!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoseScene");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
