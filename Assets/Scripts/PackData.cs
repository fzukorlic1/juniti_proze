using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PackData
{
    string packId;
    string packName;
    LevelData[] levels;

    public PackData(string packId, string packName, LevelData[] levels)
    {
        this.PackId = packId;
        this.PackName = packName;
        this.Levels = levels;
    }

    public string PackId { get => packId; set => packId = value; }
    public string PackName { get => packName; set => packName = value; }
    public LevelData[] Levels { get => levels; set => levels = value; }
}
