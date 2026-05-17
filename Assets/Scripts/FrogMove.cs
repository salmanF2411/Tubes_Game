using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FrogMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float jumpForceX = 3f;
    [SerializeField] private float jumpForceY = 6f;
    [SerializeField] private float jumpDelay = 1.5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip frogAudioClip;
    [SerializeField] private float maxAudioDistance = 15f;

    private Rigidbody2D rb;
    private Animator anim;
    private float timer;
    private bool isGrounded = true;

    private Transform player;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = jumpDelay;

        audioSource = GetComponent<AudioSource>();

        if (frogAudioClip != null)
        {
            audioSource.clip = frogAudioClip;
            audioSource.loop = true;
            audioSource.spatialBlend = 0f;
            audioSource.Play();
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        UpdateAudioVolume();

        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 1.5f && isGrounded)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        if (waypoints[currentWaypointIndex].transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Hadap Kanan
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Hadap Kiri
        }

        if (isGrounded)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Jump();
                timer = jumpDelay;
            }
        }

        if (anim != null)
        {
            anim.SetBool("isJumping", !isGrounded);
        }
    }

    private void UpdateAudioVolume()
    {
        if (audioSource == null || player == null || !audioSource.isPlaying) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float targetVolume = 1f - (distanceToPlayer / maxAudioDistance);
        
        audioSource.volume = Mathf.Clamp01(targetVolume);
    }

    private void Jump()
    {
        isGrounded = false;
        
        float moveDirection = (waypoints[currentWaypointIndex].transform.position.x > transform.position.x) ? 1f : -1f;
        rb.velocity = new Vector2(jumpForceX * moveDirection, jumpForceY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, maxAudioDistance);
    }
}