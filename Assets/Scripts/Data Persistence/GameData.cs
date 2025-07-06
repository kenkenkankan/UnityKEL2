using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public string sceneName;
    public string chapterName;
    public long lastSaveTime;

    public SerializableDictionary<string, bool> keyItemsCollected;
    [Tooltip("Charactes id as key, and list of their stored dilogue line keys as values")]
    public SerializableDictionaryValueList<string, string> dialoguesKeyPoints;

    public GameData()
    {
        playerPosition = new(3.6f, -2.6f, -1.5f);
        sceneName = "level0";
        chapterName = "The Existence";
        lastSaveTime = DateTime.Now.ToBinary();
        keyItemsCollected = new();
        dialoguesKeyPoints = new();
    }

    public int GetProgressPercentage()
    {
        return default;
    }
}
