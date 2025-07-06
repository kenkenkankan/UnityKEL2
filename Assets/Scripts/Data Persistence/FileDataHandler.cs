using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    public string dataDirPath = "";
    public string dataFileName = "";

    private bool useEncryption = false;
    private readonly string ecryptioCodenWord = "secret";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId)
    {
        if (profileId == null) return null;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                // Load serialized data fromm file
                using FileStream stream = new(fullPath, FileMode.Open);
                using StreamReader sr = new(stream);
                dataToLoad = sr.ReadToEnd();

                if (useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                // deserialize the data from Json back to C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + ex);
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        if (profileId == null) return;

        // Path combine for different OS have different path separator
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# game data object into Json 
            string dataToStore = JsonUtility.ToJson(data,true);
            
            if (useEncryption)
                dataToStore = EncryptDecrypt(dataToStore);

            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter sw = new(stream);
            sw.Write(dataToStore);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occured when try to save data to file: " + fullPath + "\n" + ex);
        }
    }

    public void DeleteData(string profileId)
    {
        if (profileId == null) return;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        Debug.Log("full path " + fullPath);
        try
        {   
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occured when try to save data to file: " + fullPath + "\n" + ex);
        }
    }

    public Dictionary<string, GameData> LoadAllProfile()
    {
        Dictionary<string, GameData> profileDictionaries = new();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (var dirinfo in dirInfos)
        {
            string profileId = dirinfo.Name;

            // if it doesn't, then this folder isn't a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            // Load the game data for this profile and put it in the dictionary
            GameData profileData = Load(profileId);
            // defensive programming ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if (profileData != null)
            {
                profileDictionaries.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError($"Tried to load ProfileID: '{profileId} but somethin went wrong");
            }
        }

        return profileDictionaries;
    }

    public string GetMostRecentUpdatedProfile()
    {
        string mostRecentProfileId = default;
        Dictionary<string, GameData> profileGameData = LoadAllProfile();
        foreach (var pair in profileGameData)
        {
            var profileId = pair.Key;
            var gameData = pair.Value;

            if (gameData == null) continue;

            if (mostRecentProfileId == default)
                mostRecentProfileId = profileId;
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profileGameData[mostRecentProfileId].lastSaveTime);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastSaveTime);

                if (newDateTime > mostRecentDateTime)
                    mostRecentProfileId = profileId;
            }
        }
        return mostRecentProfileId;
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += data[i] ^ ecryptioCodenWord[i % ecryptioCodenWord.Length];
        }
        return modifiedData;
    }
}
