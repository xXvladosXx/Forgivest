using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameCore.Data.SaveLoad
{
    public class FileManager
    {
        public static void SaveToBinaryFile(string saveFile, object data)
        {
            string path = GetPathFromSaveFile(saveFile);

            Type type = data.GetType();
            Debug.LogWarning("Saving to " + path + " " + saveFile);

            using FileStream fileStream = File.Open(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            if (!type.IsSerializable) return;

            formatter.Serialize(fileStream, data);
        }

        public static PlayerProgress LoadFromBinaryFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.LogWarning("Loading from " + path + " " + saveFile);
            
            if (!File.Exists(path))
            {
                return new PlayerProgress("");
            }

            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (PlayerProgress) formatter.Deserialize(fileStream);
            }
        }
        
        public static string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}