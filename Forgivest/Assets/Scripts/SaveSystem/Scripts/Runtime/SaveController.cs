using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem.Scripts.Runtime
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private SaveDataChannel _saveDataChannel;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save("NEW_SAVE_FILE");
            }
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> capturedStates = LoadFile(saveFile);
            CaptureState(capturedStates);
            SaveFile(saveFile, capturedStates);
        }

        private void CaptureState(Dictionary<string, object> capturedStates)
        {
            foreach (var savableEntity in _saveDataChannel.SavableEntities)
            {
                if(savableEntity == null) continue;
                
                capturedStates[savableEntity.GetUniqueIdentifier()] = savableEntity.CaptureState();
            }

            capturedStates["SceneIndexToLoad"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void SaveFile(string saveFile, object captureState)
        {
            string path = GetPathFromSaveFile(saveFile);

            Type type = captureState.GetType();
            Debug.Log("Saving to " + path + " " + saveFile);

            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (!type.IsSerializable) return;

                formatter.Serialize(fileStream, captureState);
            }
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log("Laoding from " + path + " " + saveFile);
            
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using FileStream fileStream = File.Open(path, FileMode.Open);
            fileStream.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>) formatter.Deserialize(fileStream);
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var savableEntity in _saveDataChannel.SavableEntities)
            {
                if(savableEntity == null) continue;
                string uniqueID = savableEntity.GetUniqueIdentifier();

                if (state.ContainsKey(uniqueID))
                {
                    savableEntity.RestoreState(state[uniqueID]);
                }
            }
        }

        public IEnumerator LoadScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
           
            yield return null;

            RestoreState(state);
        }


        public IEnumerable<string> SavesList()
        {
            foreach (var save in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(save) == ".sav")
                    yield return Path.GetFileNameWithoutExtension(save);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
        
        public int GetSceneIndex(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if (state.ContainsKey("SceneIndexToLoad"))
            {
                int buildIndex = (int) state["SceneIndexToLoad"];

                return buildIndex;
            }

            return -1;
        }

        /*public void SaveSettings(SettingsSaveData settingsSaveData)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, settingsSaveData.MusicVolume);
            PlayerPrefs.SetFloat(EFFECTS_VOLUME, settingsSaveData.EffectsVolume);
            PlayerPrefs.SetInt(IS_FULLSCREEN, Convert.ToInt32(settingsSaveData.Fullscreen));
            PlayerPrefs.SetInt(RESOLUTION, settingsSaveData.ResolutionIndex);
            PlayerPrefs.SetInt(GRAPHICS, settingsSaveData.GraphicsIndex);
        }
        
        public SettingsSaveData LoadSettings()
        {
            var settingsSaveData = new SettingsSaveData
            {
                EffectsVolume = PlayerPrefs.GetFloat(EFFECTS_VOLUME, 0),
                MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0),
                Fullscreen = Convert.ToBoolean(PlayerPrefs.GetInt(IS_FULLSCREEN, 0)),
                ResolutionIndex = PlayerPrefs.GetInt(RESOLUTION, 0),
                GraphicsIndex = PlayerPrefs.GetInt(GRAPHICS, 0)
            };
            
            return settingsSaveData;
        }*/
        
        
    }
}