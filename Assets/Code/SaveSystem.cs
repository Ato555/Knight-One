using UnityEngine;
using System.IO;

public static class SaveSystem
{
    // Thư mục Save nằm cạnh file .exe
    static string SaveDir
    {
        get
        {
            string exeDir = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(exeDir, "Save");
        }
    }

    public static void Save(GameData data, string fileName)
    {
        // TẠO THƯ MỤC SAVE NẾU CHƯA CÓ
        if (!Directory.Exists(SaveDir))
            Directory.CreateDirectory(SaveDir);

        string savePath = Path.Combine(SaveDir, fileName + ".json");

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved: " + savePath);
    }
}

[System.Serializable]
public class GameData
{
    public float playerX;
    public float playerY;
    public float playerZ;
}
