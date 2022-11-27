using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GameCore.SaveSystem.Data;
using UnityEngine;

namespace GameCore.SaveSystem.Reader
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
        
        public static string GetPathFromSaveFile(string saveFile) => 
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");

        public static IEnumerable<string> SavesList() =>
            from save in Directory.EnumerateFiles(Application.persistentDataPath) 
            where Path.GetExtension(save) == ".sav" select Path.GetFileNameWithoutExtension(save);
        
        public static string GetLastSave => Path.GetFileNameWithoutExtension(Directory.GetFiles(Path.Combine(Application.persistentDataPath))
            .Select(x => new FileInfo(x))
            .OrderByDescending(x => x.LastWriteTime)
            .FirstOrDefault()
            ?.ToString());
    }
}