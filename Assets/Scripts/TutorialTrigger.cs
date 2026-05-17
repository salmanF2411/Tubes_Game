using System.Collections;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial UI Settings")]
    [Tooltip("Masukkan objek gambar/animasi tutorial ke sini")]
    [SerializeField] private GameObject tutorialPopUp; 
    
    [Tooltip("Posisi pop-up di atas kepala player (X, Y, Z)")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f); 
    
    [Tooltip("Berapa lama pop-up bertahan SETELAH player menekan A/D? (dalam detik)")]
    [SerializeField] private float timeToHideAfterPractice = 2.5f; 

    private Transform playerTransform;
    private bool isTriggered = false;
    private bool hasPracticed = false;

    private void Start()
    {
        // Pastikan pop-up mati saat game mulai
        if (tutorialPopUp != null)
        {
            tutorialPopUp.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mengecek apakah yang masuk area adalah Player dan tutorial belum pernah aktif
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            playerTransform = other.transform; // Simpan data posisi player untuk diikuti
            
            if (tutorialPopUp != null)
            {
                tutorialPopUp.SetActive(true); // Munculkan tutorial
            }
        }
    }

    private void Update()
    {
        // Jika tutorial sedang aktif
        if (isTriggered && tutorialPopUp != null && tutorialPopUp.activeSelf)
        {
            // 1. Buat Pop-up selalu mengikuti posisi Player ditambah jarak Offset (di atas kepala)
            tutorialPopUp.transform.position = playerTransform.position + offset;

            // 2. Cek apakah Player sudah menekan tombol A atau D (mencoba praktek)
            if (!hasPracticed && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            {
                hasPracticed = true; // Tandai bahwa player sudah paham/mencoba
                
                // Mulai hitung mundur untuk menghilangkan pop-up
                StartCoroutine(HideTutorialRoutine()); 
            }
        }
    }

    // Fungsi untuk memberi jeda waktu sebelum pop-up hilang
    private IEnumerator HideTutorialRoutine()
    {
        // Tunggu selama beberapa detik (sesuai settingan timeToHideAfterPractice)
        yield return new WaitForSeconds(timeToHideAfterPractice);

        // Matikan pop-up
        if (tutorialPopUp != null)
        {
            tutorialPopUp.SetActive(false);
        }

        // Hancurkan kotak trigger ini agar tidak membebani game, karena tutorialnya sudah selesai
        Destroy(gameObject); 
    }
}