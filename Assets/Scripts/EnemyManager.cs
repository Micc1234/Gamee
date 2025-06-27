using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;           // Prefab enemy yang akan di-spawn
    public float spawnAreaWidth = 10f;       // Lebar area spawn (X)
    public float spawnAreaLength = 10f;      // Panjang area spawn (Z)
    public float spawnY = 1.74f;             // Ketinggian spawn (Y)
    public int totalEnemiesToSpawn = 10;     // Total musuh yang akan di-spawn
    public TextMeshProUGUI enemyCountText;              // UI Text untuk menampilkan jumlah musuh yang tersisa

    private int remainingEnemies;            // Menyimpan jumlah musuh yang tersisa

    void Start()
    {
        remainingEnemies = totalEnemiesToSpawn;    // Set jumlah musuh yang tersisa
        UpdateEnemyCountUI();                      // Update tampilan UI

        // Spawn semua musuh langsung
        SpawnAllEnemies();
    }

    // Fungsi untuk spawn semua musuh
    private void SpawnAllEnemies()
    {
        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            SpawnEnemy();  // Spawn musuh
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
            Debug.LogError("EnemyPrefab belum diatur!");
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

    // Fungsi untuk mengurangi jumlah musuh ketika musuh dihancurkan
    public void OnEnemyDestroyed()
    {
        remainingEnemies--;
        UpdateEnemyCountUI();

        // Jika semua musuh dihancurkan, tampilkan kondisi menang
        if (remainingEnemies <= 0)
        {
            Debug.Log("You Win!");
            // Bisa tambahkan logic untuk menang (misal UI Victory)
        }
    }
}
