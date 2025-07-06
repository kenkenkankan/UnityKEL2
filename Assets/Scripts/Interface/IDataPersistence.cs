using System;
using UnityEngine;

/// <summary>
/// For items, puzzle keys, npc dialgue line sectionss,
/// </summary>
public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(GameData data);

    string Id { get; set; }
}
