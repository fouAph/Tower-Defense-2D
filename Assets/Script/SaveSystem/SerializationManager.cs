using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class SerializationManager
{
    public static bool Save(string saveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        string path = Application.persistentDataPath + "/saves/" + saveName + ".save";

        FileStream file = File.Create(path);

        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }

    public static object Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("file not xist");
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        }

        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return null;
        }
    }

    // public static object DeleteSaveData(string saveName)

    // {
    //     string path = Application.persistentDataPath + "/saves/" + saveName + ".save";
    //     if (!File.Exists(path))
    //     {
    //         Debug.Log("file not xist");
    //         return null;
    //     }

    //     BinaryFormatter formatter = GetBinaryFormatter();
    //     FileStream file =File.Delete("abc");

    // }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter;
    }
}
