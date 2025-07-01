using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include this for TextMeshPro

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;          // Prefab enemy yang akan di-spawn
    public float spawnAreaWidth = 10f;      // Lebar area spawn (X)
    public float spawnAreaLength = 10f;     // Panjang area spawn (Z)
    public float spawnY = 1.74f;            // Ketinggian spawn (Y)
    public int totalEnemiesToSpawn = 10;    // Total musuh yang akan di-spawn
    public TextMeshProUGUI enemyCountText;  // UI Text untuk menampilkan jumlah musuh yang tersisa

    // --- New variables for time limit ---
    public float gameDuration = 60f;        // Durasi total permainan dalam detik (1 menit)
    public TextMeshProUGUI gameTimerText;   // UI Text untuk menampilkan sisa waktu
    public GameObject gameOverPanel;        // Panel UI untuk kondisi kalah (Time Over)
    public GameObject gameWonPanel;         // Panel UI untuk kondisi menang (Enemies Defeated)
    // --- End new variables ---

    private int remainingEnemies;           // Menyimpan jumlah musuh yang tersisa
    private float timeRemaining;            // Menyimpan sisa waktu
    private bool gameEnded = false;         // Flag untuk menandakan apakah game sudah berakhir

    void Start()
    {
        remainingEnemies = totalEnemiesToSpawn;    // Set jumlah musuh yang tersisa
        UpdateEnemyCountUI();                      // Update tampilan UI

        timeRemaining = gameDuration;              // Inisialisasi waktu
        UpdateGameTimerUI();                       // Update tampilan UI waktu

        // Pastikan panel game over dan game won nonaktif di awal
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
        if (!gameEnded) // Hanya jalankan timer jika game belum berakhir
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Kurangi waktu setiap frame
                UpdateGameTimerUI();             // Update tampilan UI waktu

                if (timeRemaining <= 0)
                {
                    timeRemaining = 0; // Pastikan waktu tidak negatif
                    UpdateGameTimerUI();
                    GameOver(); // Panggil fungsi game over jika waktu habis
                }
            }
        }
    }

    // Fungsi untuk spawn semua musuh
    private void SpawnAllEnemies()
    {
        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            SpawnEnemy(); // Spawn musuh
        }
    }

    // Fungsi untuk spawn musuh di posisi acak
    private void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            // Generate posisi acak dalam area yang ditentukan
            float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
            float randomZ = Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2);

            // Set posisi spawn dengan Y tetap 1.74
            Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);

            // Spawn musuh pada posisi yang telah ditentukan
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("EnemyPrefab belum diatur di Inspector!");
        }
    }

    // Fungsi untuk mengupdate UI jumlah musuh yang tersisa
    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies : " + remainingEnemies.ToString();
        }
    }

    // Fungsi untuk mengupdate UI sisa waktu
    private void UpdateGameTimerUI()
    {
        if (gameTimerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            gameTimerText.text = string.Format("Time : {0:00}:{1:00}", minutes, seconds);
        }
    }

    // Fungsi untuk mengurangi jumlah musuh ketika musuh dihancurkan
    public void OnEnemyDestroyed()
    {
        if (gameEnded) return; // Jangan lakukan apa-apa jika game sudah berakhir

        remainingEnemies--;
        UpdateEnemyCountUI();

        // Jika semua musuh dihancurkan, tampilkan kondisi menang
        if (remainingEnemies <= 0)
        {
            WinGame();
        }
    }

    // Fungsi ketika pemain menang
    private void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("You Win! All enemies defeated!");
        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(true);
        }
        // Opsional: Pause game, tampilkan menu kemenangan, dll.
        Time.timeScale = 0f; // Menghentikan semua pergerakan dalam game
    }

    // Fungsi ketika waktu habis (pemain kalah)
    private void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("Game Over! Time's up!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // Opsional: Pause game, tampilkan menu kekalahan, dll.
        Time.timeScale = 0f; // Menghentikan semua pergerakan dalam game
    }

    // Tambahkan ini jika Anda ingin tombol restart atau kembali ke menu
    public void RestartGame()
    {
        Time.timeScale = 1f; // Pastikan waktu kembali normal
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}