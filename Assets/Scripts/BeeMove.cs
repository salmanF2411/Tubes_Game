using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Memastikan komponen AudioSource selalu ada
public class BeeMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Patrol Area")]
    [SerializeField] private float patrolRangeX = 3f;
    [SerializeField] private float patrolRangeY = 2f;
    [SerializeField] private float patrolWaitTime = 1.5f;

    [Header("Chase")]
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float maxChaseDistance = 7f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip beeAudioClip; // Slot untuk memasukkan audio lebah
    [SerializeField] private float maxAudioDistance = 15f; // Jarak maksimal suara terdengar

    private Transform player;
    private AudioSource audioSource;

    private Vector2 startPosition;
    private Vector2 patrolTarget;

    private bool isChasing = false;
    private bool isWaiting = false;

    private void Start()
    {
        // Ambil komponen AudioSource dari Lebah
        audioSource = GetComponent<AudioSource>();

        // Pasang dan mainkan audio jika ada
        if (beeAudioClip != null)
        {
            audioSource.clip = beeAudioClip;
            audioSource.loop = true;
            audioSource.spatialBlend = 0f; // Memastikan tetap 2D agar script yang mengatur volume
            audioSource.Play();
        }

        // Simpan posisi awal
        startPosition = transform.position;

        // Cari player
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Ambil titik patrol pertama
        PickNewPatrolPoint();
    }

    private void Update()
    {
        // Update volume suara setiap frame berdasarkan jarak
        UpdateAudioVolume();

        if (Enemy.isEnemyDeath) return;

        if (player == null || Death.isDeath)
        {
            isChasing = false;
            PatrolMovement();
            return;
        }

        // =========================
        // HITUNG JARAK
        // =========================

        // Jarak player ke SARANG bee
        float distancePlayerToHome =
            Vector2.Distance(player.position, startPosition);

        // Jarak bee ke SARANG
        float distanceBeeToHome =
            Vector2.Distance(transform.position, startPosition);

        // =========================
        // KONDISI CHASE
        // =========================

        // Player masuk range
        if (distancePlayerToHome <= chaseRange &&
            distanceBeeToHome <= maxChaseDistance)
        {
            isChasing = true;
        }

        // Player keluar range
        if (distancePlayerToHome > chaseRange ||
            distanceBeeToHome >= maxChaseDistance)
        {
            isChasing = false;
        }

        // =========================
        // MODE CHASE
        // =========================
        if (isChasing)
        {
            MoveTo(player.position);
        }

        // =========================
        // MODE PATROL
        // =========================
        else
        {
            PatrolMovement();
        }
    }

    // ===================================
    // AUDIO VOLUME SYSTEM
    // ===================================
    private void UpdateAudioVolume()
    {
        if (audioSource == null || player == null || !audioSource.isPlaying) return;

        // Hitung jarak posisi lebah saat ini dengan posisi player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Atur volume: 1 saat sangat dekat, perlahan menjadi 0 saat mencapai maxAudioDistance
        float targetVolume = 1f - (distanceToPlayer / maxAudioDistance);
        
        // Pastikan volume tidak minus atau lebih dari 1
        audioSource.volume = Mathf.Clamp01(targetVolume);
    }

    // ===================================
    // PATROL SYSTEM
    // ===================================
    private void PatrolMovement()
    {
        if (isWaiting) return;

        MoveTo(patrolTarget);

        float distanceToPatrolPoint =
            Vector2.Distance(transform.position, patrolTarget);

        // Sampai titik patrol
        if (distanceToPatrolPoint < 0.2f)
        {
            StartCoroutine(WaitAndPickNewPoint());
        }
    }

    private IEnumerator WaitAndPickNewPoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(patrolWaitTime);

        PickNewPatrolPoint();

        isWaiting = false;
    }

    private void PickNewPatrolPoint()
    {
        float randomX =
            Random.Range(-patrolRangeX, patrolRangeX);

        float randomY =
            Random.Range(-patrolRangeY, patrolRangeY);

        patrolTarget = new Vector2(
            startPosition.x + randomX,
            startPosition.y + randomY
        );
    }

    // ===================================
    // MOVE
    // ===================================
    private void MoveTo(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        FlipSprite(target.x);
    }

    // ===================================
    // FLIP SPRITE
    // ===================================
    private void FlipSprite(float targetX)
    {
        if (targetX > transform.position.x)
        {
            transform.rotation =
                Quaternion.Euler(0, 180, 0);
        }
        else if (targetX < transform.position.x)
        {
            transform.rotation =
                Quaternion.Euler(0, 0, 0);
        }
    }

    // ===================================
    // GIZMOS
    // ===================================
    private void OnDrawGizmosSelected()
    {
        Vector3 center =
            Application.isPlaying
            ? startPosition
            : transform.position;

        // Patrol Area
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(
            center,
            new Vector3(
                patrolRangeX * 2,
                patrolRangeY * 2,
                1
            )
        );

        // Chase Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, chaseRange);

        // Max Chase Distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, maxChaseDistance);

        // Audio Range (Garis putih putus-putus)
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, maxAudioDistance);
    }
}