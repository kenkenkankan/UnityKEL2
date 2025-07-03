using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string ecryptioCodenWord = "secret";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileName)
    {
        string fullpath = Path.Combine(dataDirPath, profileName, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";
                // Load serialized data fromm file
                using FileStream stream = new(fullpath, FileMode.Open);
                using StreamReader sr = new(stream);
                dataToLoad = sr.ReadToEnd();

                if (useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                // deserialize the data from Json back to C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullpath + "\n" + ex);
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileName)
    {
        // Path combine for different OS have different path separator
        string fullpath = Path.Combine(dataDirPath, profileName, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            // Serialize the C# game data object into Json 
            string dataToStore = JsonUtility.ToJson(data,true);
            
            if (useEncryption)
                dataToStore = EncryptDecrypt(dataToStore);

            using FileStream stream = new(fullpath, FileMode.Create);
            using StreamWriter sw = new(stream);
            sw.Write(dataToStore);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occured when try to save data to file: " + fullpath + "\n" + ex);
        }
    }

    public Dictionary<string, GameData> LoadAllProfile()
    {
        Dictionary<string, GameData> profileDictionaries = new();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (var dirinfo in dirInfos)
        {
            string profileId = dirinfo.Name;

            // defensive programming check if the data file exists
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
