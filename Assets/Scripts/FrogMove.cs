using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMove : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float jumpForceX = 3f;
    [SerializeField] private float jumpForceY = 6f;
    [SerializeField] private float jumpDelay = 1.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private float timer;
    private bool isGrounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = jumpDelay;
    }

    private void Update()
    {
        // 1. Cek jarak dengan waypoint saat ini (hanya saat sedang di tanah)
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 1.5f && isGrounded)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        // 2. Putar arah hadap katak sesuai posisi waypoint
        if (waypoints[currentWaypointIndex].transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Hadap Kanan
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Hadap Kiri
        }

        // 3. Sistem Jeda Lompatan (hanya menghitung mundur saat di tanah)
        if (isGrounded)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Jump();
                timer = jumpDelay; // Reset timer
            }
        }

        // 4. Update status ke Animator
        // Jika isGrounded false (sedang di udara), maka isJumping akan true
        if (anim != null)
        {
            anim.SetBool("isJumping", !isGrounded);
        }
    }

    private void Jump()
    {
        isGrounded = false;
        
        // Tentukan arah dorongan X (1 untuk kanan, -1 untuk kiri)
        float moveDirection = (waypoints[currentWaypointIndex].transform.position.x > transform.position.x) ? 1f : -1f;

        // Berikan dorongan tenaga melompat
        rb.velocity = new Vector2(jumpForceX * moveDirection, jumpForceY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Deteksi pendaratan di tanah
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.velocity = Vector2.zero; // Rem mendadak agar tidak licin seperti es
        }
    }
}