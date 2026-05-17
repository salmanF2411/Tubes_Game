using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SlugMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2.0f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip slugAudioClip;
    [SerializeField] private float maxAudioDistance = 15f;

    private Transform player;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (slugAudioClip != null)
        {
            audioSource.clip = slugAudioClip;
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

        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                currentWaypointIndex = 0;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }

    private void UpdateAudioVolume()
    {
        if (audioSource == null || player == null || !audioSource.isPlaying) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float targetVolume = 1f - (distanceToPlayer / maxAudioDistance);
        
        audioSource.volume = Mathf.Clamp01(targetVolume);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, maxAudioDistance);
    }
}