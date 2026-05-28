using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public static int health = 3;
    [SerializeField] GameObject live3;
    [SerializeField] GameObject live2;
    [SerializeField] GameObject live1;
    [SerializeField] GameObject live0;
    [SerializeField] GameObject pauseScene;
    
    private void Update() 
    {
        switch (health)
        {
            case 3:
                live3.SetActive(true);
                live2.SetActive(false);
                live1.SetActive(false);
                live0.SetActive(false);
            break;
            case 2:
                live3.SetActive(false);
                live2.SetActive(true);
                live1.SetActive(false);
                live0.SetActive(false);
            break;
            case 1:
                live3.SetActive(false);
                live2.SetActive(false);
                live1.SetActive(true);
                live0.SetActive(false);
            break;
            case 0:
                live3.SetActive(false);
                live2.SetActive(false);
                live1.SetActive(false);
                live0.SetActive(true);
                StartCoroutine(DeathPanel());
                StartCoroutine(DeathTime());
            break;
        }
    }

    // ==================================================
    // SISTEM HEALING (AMBIL CHERRY)
    // ==================================================
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Cek apakah player menyentuh item dengan tag "Cherry"
        if (other.gameObject.CompareTag("Cherry"))
        {
            // Cek apakah nyawa saat ini masih kurang dari 3
            if (health < 3)
            {
                health++; // Tambah 1 nyawa
            }
            
            // Hancurkan item Cherry agar hilang dari level setelah diambil
            Destroy(other.gameObject);
        }
    }

    IEnumerator DeathTime(){
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }
    
    IEnumerator DeathPanel(){
        yield return new WaitForSeconds(0.9f);
        pauseScene.SetActive(true);
    }
}