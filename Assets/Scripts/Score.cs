using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip audioStar;
    [SerializeField] private AudioClip audioCarrot; // Tambahan variabel untuk suara Carrot
    [SerializeField] private AudioSource _audioSource;
    
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _carrotsImage0;
    [SerializeField] private Image _carrotsImage1;
    [SerializeField] private Image _carrotsImage2;
    
    public static int score = 0;
    public static int carrot = 0;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 1. Logika mengambil Star
        if (other.gameObject.CompareTag("Star"))
        {
            score++;
            _scoreText.text = score.ToString();
            
            // Mainkan suara Star
            _audioSource.PlayOneShot(audioStar, 0.05f);
            Destroy(other.gameObject);
        }
        // 2. Logika mengambil Carrot
        else if (other.gameObject.CompareTag("Carrot"))
        {
            switch (carrot)
            {
                case 0:
                    var tempColor0 = _carrotsImage0.color;
                    tempColor0.a = 1f;
                    _carrotsImage0.color = tempColor0;
                break;
                case 1:
                    var tempColor1 = _carrotsImage1.color;
                    tempColor1.a = 1f;
                    _carrotsImage1.color = tempColor1;
                break;
                case 2:
                    var tempColor2 = _carrotsImage2.color;
                    tempColor2.a = 1f;
                    _carrotsImage2.color = tempColor2;
                break;
            }
            carrot++;
            
            // Mainkan suara khusus Carrot
            _audioSource.PlayOneShot(audioCarrot, 0.05f);
            Destroy(other.gameObject);
        }
    }
}