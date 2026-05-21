using UnityEngine;
using TMPro; // Wajib dipanggil karena kita pakai TextMeshPro

[RequireComponent(typeof(TextMeshProUGUI))]
public class AutoTranslate : MonoBehaviour
{
    [Header("Kamus Bahasa")]
    [TextArea] public string textEnglish;
    [TextArea] public string textIndonesia;

    private TextMeshProUGUI myText;

    private void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    // OnEnable dipanggil setiap kali teks ini dimunculkan di layar
    private void OnEnable()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (myText == null) return;

        // Cek bahasa yang sedang dipilih (0 = English, 1 = Indonesia)
        // Default kita set 0 (English)
        int currentLanguage = PlayerPrefs.GetInt("GameLanguage", 0);

        if (currentLanguage == 0)
        {
            myText.text = textEnglish;
        }
        else if (currentLanguage == 1)
        {
            myText.text = textIndonesia;
        }
    }
}