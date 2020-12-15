using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListUpdater : MonoBehaviour
{

    public GameObject scrollViewPacks;
    public GameObject buttonPrefab;

    public GameObject scrollViewSaveFiles;

    bool listed = false;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        while (!listed && GameData.dataLoaded)
        {
            renderPacks();
            renderSaveFiles();
            listed = true;
        }
    }

    void renderPacks()
    {
        for (int i = 0; i < GameData.packs.Count; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            TextMeshProUGUI text = button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = GameData.packs[i].PackName;
            int x = new int();
            x = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { packClicked(x); });
            button.transform.SetParent(scrollViewPacks.transform, false);
        }
    }

    void packClicked(int packIndex)
    {
        GameData.currentSaveFileIndex = GameData.saveFiles.Count;
        GameData.saveFiles.Add(new SaveFileData(GameData.packs[packIndex].Levels[0].LevelId, GameData.packs[packIndex].PackId, 0));
        GameData.saveSaveFile();

        gameObject.GetComponent<SceneChanger>().GameScene(GameData.packs[packIndex].PackId, GameData.packs[packIndex].Levels[0].LevelId);
    }

    void renderSaveFiles()
    {
        for (int i = 0; i < GameData.saveFiles.Count; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            TextMeshProUGUI text = button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            foreach(PackData packData in GameData.packs)
            {
                if(packData.PackId == GameData.saveFiles[i].PackId)
                {
                    for(int j = 0; j < packData.Levels.Length; j++)
                    {
                        if(packData.Levels[j].LevelId == GameData.saveFiles[i].LevelId)
                        {
                            text.text = packData.PackName + " " + j + "/" + packData.Levels.Length;
                            break;
                        }
                    }
                    break;
                }
            }
            int x = new int();
            x = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { saveFileClicked(x); });
            button.transform.SetParent(scrollViewSaveFiles.transform, false);
        }
    }

    void saveFileClicked(int saveFileIndex)
    {
        GameData.currentSaveFileIndex = saveFileIndex;
        gameObject.GetComponent<SceneChanger>().GameScene(GameData.saveFiles[saveFileIndex].PackId, GameData.saveFiles[saveFileIndex].LevelId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
