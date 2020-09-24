using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine.SocialPlatforms;

/// BinaryFormatter is used to serialize an object (meaning it converts it to one long stream of 1s and 0s), 
/// and deserialize it (converting that stream back to its usual form with all data intact), and is typically used with to used with to 
/// save data to the hard disk so it can be loaded again after the game is closed and started up again.
/// FileStream provides asynchronous read and write operations.

public class SerializationManager
{ 
    public static bool Save(string saveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        string path = Application.persistentDataPath + "/Saves/" + saveName + ".save";
        FileStream file = File.Create(path);
        formatter.Serialize(file, saveData);
        file.Close();
        return true;
    }

    public static object Load(string path)
    {
        if (!File.Exists(path))
        {
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
            Debug.LogErrorFormat("Failed to load save file at {0}", path);
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        QuarternionSerializationSurrogate quaternionSurrogate = new QuarternionSerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
        formatter.SurrogateSelector = selector;

        return formatter;
    }
}
