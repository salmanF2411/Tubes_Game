using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    // Fungsi untuk tombol bahasa Inggris
    public void PilihBahasaInggris()
    {
        PlayerPrefs.SetInt("GameLanguage", 0);
        PlayerPrefs.Save();
        RefreshSemuaTeks();
    }

    // Fungsi untuk tombol bahasa Indonesia
    public void PilihBahasaIndonesia()
    {
        PlayerPrefs.SetInt("GameLanguage", 1);
        PlayerPrefs.Save();
        RefreshSemuaTeks();
    }

    // Memperbarui semua teks yang sedang tampil di layar secara instan
    private void RefreshSemuaTeks()
    {
        AutoTranslate[] semuaTeks = FindObjectsOfType<AutoTranslate>();
        foreach (AutoTranslate teks in semuaTeks)
        {
            teks.UpdateText();
        }
    }
}