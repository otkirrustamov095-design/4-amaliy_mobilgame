using System.IO;
using UnityEngine;

// O'yinchi ma'lumotlari. [System.Serializable] bo'lishi shart,
// aks holda JsonUtility uni JSON ga aylantira olmaydi.
[System.Serializable]
public class PlayerData
{
    public int bestScore;    // rekord (eng ko'p yig'ilgan ball)
    public int totalCoins;   // umumiy yig'ilgan tangalar
    public int gamesPlayed;  // necha marta o'ynalgan
}

// Ma'lumotni JSON faylga saqlash va qayta yuklash
public static class SaveSystem
{
    // Fayl manzili: Windows da odatda
    // C:/Users/<ism>/AppData/LocalLow/<kompaniya>/<loyiha>/player_data.json
    static string FilePath => Path.Combine(Application.persistentDataPath, "player_data.json");

    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true); // true = chiroyli formatda
        File.WriteAllText(FilePath, json);
        Debug.Log("Saqlandi (JSON): " + JsonUtility.ToJson(data) + "\n" + FilePath);
    }

    public static PlayerData Load()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            Debug.Log("Yuklandi (JSON): " + json.Replace("\n", "").Replace("    ", " ") + "\n" + FilePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }

        Debug.Log("Saqlangan fayl yo'q, yangi ma'lumot yaratildi");
        return new PlayerData();
    }
}
