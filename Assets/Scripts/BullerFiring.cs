using UnityEngine;

public class BulletFiring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletPoint;
    public float bulletForce = 50f; // Sesuaikan dengan skala dunia kamu
    public AudioClip fireSound;
    public float fireRate = 0.1f;

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

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.LookRotation(bulletDirection));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = bulletDirection * bulletForce;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        // Raycast for instant damage
        RaycastHit hit;
        if (Physics.Raycast(bulletPoint.position, bulletDirection, out hit, 100f))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }

        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }
}
