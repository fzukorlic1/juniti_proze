using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class SaveFileData
{
    string levelId;
    string packId;
    int time;

    public SaveFileData(string levelId, string packId, int time)
    {
        this.levelId = levelId;
        this.packId = packId;
        this.time = time;
    }

    public string LevelId { get => levelId; set => levelId = value; }
    public string PackId { get => packId; set => packId = value; }
    public int Time { get => time; set => time = value; }
}
