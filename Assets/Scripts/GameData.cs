using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;

[InitializeOnLoad]
public class GameData
{
    public static List<PackData> packs;
    public static bool dataLoaded = false;

    public static List<SaveFileData> saveFiles;

    public static int currentSaveFileIndex;

    static string path;

    static GameData()
    {
        Debug.Log("Up and running");

        path = Application.persistentDataPath;

        Directory.CreateDirectory(path + "/packs");

        if (!File.Exists(path + "/packs.dat"))
        {
            Debug.Log("No packs found. Creating default pack...");
            try
            {
                saveDefaultPack();
                Debug.Log("Creation successfull.");
            }
            catch (Exception e)
            {
                Debug.Log("Error creating default pack!" + e.Message);
            }
        }


        FileStream stream = new FileStream(path + "/packs.dat", FileMode.Open, FileAccess.Read);

        BinaryFormatter formatter = new BinaryFormatter();

        string[] packIds = formatter.Deserialize(stream) as string[];
        stream.Close();

        packs = new List<PackData>();

        for(int i = 0; i < packIds.Length; i++)
        {
            stream = new FileStream(path + "/packs/" + packIds[i] + ".pack", FileMode.Open, FileAccess.Read);

            formatter = new BinaryFormatter();

            PackData pack = formatter.Deserialize(stream) as PackData;
            stream.Close();

            packs.Add(pack);
            Debug.Log("PackId: " + packIds[i]);
            Debug.Log("PackName: " + pack.PackName);
        }

        if (!File.Exists(path + "/saveFiles.dat"))
        {
            Debug.Log("No saveFiles file found. Creating file...");
            try
            {
                saveFiles = new List<SaveFileData> {};

                stream = new FileStream(path + "/saveFiles.dat", FileMode.Create);

                formatter = new BinaryFormatter();

                formatter.Serialize(stream, saveFiles);
                stream.Close();

                Debug.Log("Creation successfull.");
            }
            catch (Exception e)
            {
                Debug.Log("Error creating saveFiles file!" + e.Message);
            }
        }

        stream = new FileStream(path + "/saveFiles.dat", FileMode.Open, FileAccess.Read);

        formatter = new BinaryFormatter();

        saveFiles = formatter.Deserialize(stream) as List<SaveFileData>;
        stream.Close();

        dataLoaded = true;
    }

    static void saveDefaultPack()
    {
        string packId = "1";
        string packName = "Default pack";
        LevelData[] levels = new LevelData[] { generateDummyLevel(), generateDummyLevel2() };

        PackData pack = new PackData(packId, packName, levels);

        FileStream stream = new FileStream(path + "/packs/" + packId + ".pack", FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, pack);
        stream.Close();


        string[] packIds = new string[1] { packId };

        stream = new FileStream(path + "/packs.dat", FileMode.Create);

        formatter = new BinaryFormatter();

        formatter.Serialize(stream, packIds);
        stream.Close();

    }

    static LevelData generateLevel1()
    {
        string levelId;
        string levelName;
        char[,] grid;

        levelId = "1";
        levelName = "Level 1";
        grid = new char[11, 22] {
            {'0','0','0','0','1','1','1','1','1','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','1','0','0','0','1','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','1','B','0','0','1','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','1','1','1','0','0','B','1','1','1','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','1','0','0','B','0','0','B','0','1','0','0','0','0','0','0','0','0','0','0','0'},
            {'1','1','1','0','1','0','1','1','1','0','1','0','0','0','0','0','1','1','1','1','1','1'},
            {'1','0','0','0','1','0','1','1','1','0','1','1','1','1','1','1','1','0','0','X','X','1'},
            {'1','0','B','0','0','B','0','0','0','0','0','0','0','0','0','0','0','0','0','X','X','1'},
            {'1','1','1','1','1','0','1','1','1','1','0','1','P','1','1','1','1','0','0','X','X','1'},
            {'0','0','0','0','1','0','0','0','0','0','0','1','1','1','0','0','1','1','1','1','1','1'},
            {'0','0','0','0','1','1','1','1','1','1','1','1','0','0','0','0','0','0','0','0','0','0'}
        };

        NegativeIonData[] negativeIons = new NegativeIonData[1]  { new NegativeIonData(7, 21, 0.4f, 0.1f, (int)NegativeIonData.SideT.Left) };

        return new LevelData(levelId, levelName, grid, negativeIons);
    }

    static LevelData generateLevel2()
    {
        string levelId;
        string levelName;
        char[,] grid;

        levelId = "2";
        levelName = "Level 2";
        grid = new char[18, 17] {
            {'0','0','0','0','0','0','0','0','0','0','1','1','1','1','1','1','1'},
            {'0','0','0','0','0','0','0','0','0','0','1','0','0','X','X','X','1'},
            {'0','0','0','0','0','0','1','1','1','1','1','0','0','X','X','X','1'},
            {'0','0','0','0','0','0','1','0','0','0','0','0','0','X','X','X','1'},
            {'0','0','0','0','0','0','1','0','0','1','1','0','0','X','X','X','1'},
            {'0','0','0','0','0','0','1','1','0','1','1','0','0','X','X','X','1'},
            {'0','0','0','0','0','1','1','1','0','1','1','1','1','1','1','1','1'},
            {'0','0','0','0','0','1','0','B','B','B','0','1','1','0','0','0','0'},
            {'0','1','1','1','1','1','0','0','B','0','B','0','1','1','1','1','1'},
            {'1','1','0','0','0','1','B','0','B','0','0','0','1','0','0','0','1'},
            {'1','P','0','B','0','0','B','0','0','0','0','B','0','0','B','0','1'},
            {'1','1','1','1','1','1','0','B','B','0','B','0','1','1','1','1','1'},
            {'0','0','0','0','0','1','0','B','0','0','0','0','1','0','0','0','0'},
            {'0','0','0','0','0','1','1','1','1','0','1','1','1','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','1','0','0','1','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','1','0','0','1','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','1','0','0','1','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','1','1','1','1','0','0','0','0','0'},
        };

        NegativeIonData[] negativeIons = new NegativeIonData[1] { new NegativeIonData(13, 10, 0.5f, 0.5f, (int)NegativeIonData.SideT.Down) };

        return new LevelData(levelId, levelName, grid, negativeIons);
    }

    static LevelData generateDummyLevel()
    {
        string levelId;
        string levelName;
        char[,] grid;

        levelId = "d1";
        levelName = "Dummy level 1";
        grid = new char[18, 17] {
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','B','X','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','P','B','X','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
        };

        NegativeIonData[] negativeIons = new NegativeIonData[1] { new NegativeIonData(13, 10, 0.5f, 0.5f, (int)NegativeIonData.SideT.Down) };

        return new LevelData(levelId, levelName, grid, negativeIons);
    }

    static LevelData generateDummyLevel2()
    {
        string levelId;
        string levelName;
        char[,] grid;

        levelId = "d2";
        levelName = "Dummy level 2";
        grid = new char[18, 17] {
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','P','B','X','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'},
        };

        NegativeIonData[] negativeIons = new NegativeIonData[1] { new NegativeIonData(13, 10, 0.5f, 0.5f, (int)NegativeIonData.SideT.Down) };

        return new LevelData(levelId, levelName, grid, negativeIons);
    }

    public static bool saveSaveFile()
    {
        try
        {

            FileStream stream = new FileStream(path + "/saveFiles.dat", FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, saveFiles);
            stream.Close();

            Debug.Log("Update successfull.");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Error updating saveFiles file!" + e.Message);
            return false;
        }
    }

    public static bool levelPassed()//vraca true ako je preso sve levele
    {
        SaveFileData saveFile = saveFiles[currentSaveFileIndex];
        foreach(PackData packData in GameData.packs)
        {
            if(packData.PackId == saveFile.PackId)
            {
                for(int i = 0; i < packData.Levels.Length; i++)
                {
                    if(packData.Levels[i].LevelId == saveFile.LevelId)
                    {
                        if(i == packData.Levels.Length - 1)
                        {
                            saveFiles.RemoveAt(currentSaveFileIndex);
                            return true;
                        } else
                        {
                            saveFile.LevelId = packData.Levels[i+1].LevelId;
                            saveFile.Time += (int)Timer.timeElapsed;
                            saveSaveFile();
                            return false;
                        }
                    }
                }
            }
        }
        return false;
    }
}