using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class LevelData
{
    string levelId;
    string levelName;
    char[,] grid;
    NegativeIonData[] negativeIons;

    public LevelData(string levelId, string levelName, char[,] grid, NegativeIonData[] negativeIons)
    {
        this.LevelId = levelId;
        this.LevelName = levelName;
        this.Grid = grid;
        this.NegativeIons = negativeIons;
    }

    public string LevelId { get => levelId; set => levelId = value; }
    public string LevelName { get => levelName; set => levelName = value; }
    public char[,] Grid { get => grid; set => grid = value; }
    public NegativeIonData[] NegativeIons { get => negativeIons; set => negativeIons = value; }
}
