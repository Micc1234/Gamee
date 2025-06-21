using UnityEngine;

public class BulletFiring : MonoBehaviour
{
    public GameObject bulletPrefab; // Masih untuk visual peluru
    public Transform bulletPoint;
    public float bulletForce = 50f; // Kecepatan peluru visual
    public AudioClip fireSound;
    public float fireRate = 0.1f;
    public float raycastRange = 100f; // Jarak maksimum deteksi raycast

    // <<< BARU: LayerMask untuk mendefinisikan layer yang bisa di-hit raycast
    public LayerMask hitDetectionLayers; // Di Inspector, atur ini hanya untuk layer "Enemy"

    private AudioSource audioSource;
    private float nextFireTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        Vector3 bulletDirection = bulletPoint.forward;

        // --- Instansiasi Peluru Visual (Hanya untuk Tampilan) ---
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.LookRotation(bulletDirection));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = bulletDirection * bulletForce;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        // Hancurkan peluru visual setelah beberapa saat
        Destroy(bullet, 2f); // Hancurkan visual peluru setelah 2 detik

        // --- Logika Damage Utama (Raycasting) ---
        RaycastHit hit;
        // Lakukan raycast, hanya mengenai layer yang ditentukan di hitDetectionLayers
        if (Physics.Raycast(bulletPoint.position, bulletDirection, out hit, raycastRange, hitDetectionLayers))
        {
            Debug.DrawRay(bulletPoint.position, bulletDirection * raycastRange, Color.red, 1f); // Garis MERAH jika kena
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
                Debug.Log("Enemy hit by raycast! Hit: " + hit.collider.name); // Menampilkan nama objek yang kena
            }
            else
            {
                Debug.Log("Raycast hit something, but it's NOT an Enemy: " + hit.collider.name + " on layer " + LayerMask.LayerToName(hit.collider.gameObject.layer)); // Lebih detail
            }
        }
        else
        {
            Debug.DrawRay(bulletPoint.position, bulletDirection * raycastRange, Color.blue, 1f); // Garis BIRU jika meleset
            Debug.Log("Raycast missed!");
        }


        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }
}