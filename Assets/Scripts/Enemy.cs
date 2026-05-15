using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private AudioClip audioDeadEnemy;
    [SerializeField] private AudioClip audioAttackEnemy;
    [SerializeField] private AudioSource audiosourceEnemy;
    public static bool isEnemyDeath = false;
    public static int killcounter = 0;
    private bool attack;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (Death.isAttack && attack)
        {
            anim.SetBool("enemyAttack", true);
        }
        else
        {
            anim.SetBool("enemyAttack", false);
            attack = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isEnemyDeath) 
        {
            isEnemyDeath = true; // Kunci status matinya di awal!
            
            audiosourceEnemy.PlayOneShot(audioDeadEnemy, 0.5f);
            anim.SetTrigger("enemyDeath");
            
            // JURUS PAMUNGKAS: Matikan semua collider di lebah ini
            // Agar tidak ada lagi deteksi sentuhan ganda dari kaki/badan player
            Collider2D[] semuaCollider = GetComponents<Collider2D>();
            foreach(Collider2D col in semuaCollider)
            {
                col.enabled = false;
            }

            Invoke("EnemyDestroy", 1f);
            killcounter += 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Tambahkan && !isEnemyDeath di sini juga!
        // Agar kalau lebahnya sudah diinjak (mati), dia tidak mengeluarkan suara serang
        if (other.gameObject.CompareTag("Player") && audioAttackEnemy != null && !isEnemyDeath)
        {
            audiosourceEnemy.PlayOneShot(audioAttackEnemy, 0.5f);
            attack = true;
        }
    }

    void EnemyDestroy()
    {
        Destroy(gameObject);
    }
}