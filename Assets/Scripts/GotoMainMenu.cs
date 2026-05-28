using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMainMenu : MonoBehaviour
{
    public void GoMainMenu()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        // Menggunakan Realtime agar tetap berjalan meskipun game sedang di-pause
        yield return new WaitForSecondsRealtime(1.5f);
        
        // Kembalikan waktu game ke normal sebelum pindah scene
        Time.timeScale = 1f; 
        
        // Reset data permainan
        Score.carrot = 0;
        HealthSystem.health = 3;
        Score.score = 0;
        Enemy.killcounter = 0;
        Death.deathcounter = 0;
        
        SceneManager.LoadScene("StartScene");
    }
}