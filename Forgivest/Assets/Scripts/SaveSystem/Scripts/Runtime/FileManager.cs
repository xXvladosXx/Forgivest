using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem.Scripts.Runtime
{
    public class FileManager
    {
        public static void SaveToBinaryFile(string path, Dictionary<string, object> data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Create);

            try
            {
                formatter.Serialize(file, data);
            }
            catch (Exception)
            {
                Debug.LogWarning($"Failed to save file at {path}");
            }
            finally
            {
                file.Close();
            }
        }

        public static Dictionary<string, object> LoadFromBinaryFile(string path)
        {
            string saveFile = GetPathFromSaveFile(path);

            var formatter = new BinaryFormatter();
            var file = File.Open(path, FileMode.Open);

            try
            {
                return formatter.Deserialize(file) as Dictionary<string, object>;
            }
            catch (Exception)
            {
                Debug.LogError($"Failed to load file at {path}");
                return new Dictionary<string, object>();
            }
            finally
            {
                file.Close();
            }
        }
        
        public static string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}