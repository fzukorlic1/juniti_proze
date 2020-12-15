using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    static string currentPackId;
    static string currentLevelId;
    public static string CurrentPackId { get => currentPackId; set => currentPackId = value; }
    public static string CurrentLevelId { get => currentLevelId; set => currentLevelId = value; }

    static float blockDim;
    static float playerY;

    public GameObject helpPanel;
    public GameObject nextLevelPanel;
    public GameObject finishPackPanel;
    public GameObject nextLevelButton;
    public GameObject finishPackButton;
    public GameObject pausePanel;
    public GameObject goToMainMenuButton;

    public GameObject block;
    public GameObject wallBlock;
    public GameObject player;
    public GameObject blockPlace;
    public GameObject floorTile;
    public GameObject negativeIon;

    public Camera minimapCamera;

    static char[,] playingGrid;
    static char[,] referenceGrid;
    static int blocksPlaced;
    static List<GameObject> playingBlocks;

    static bool blockMoving = false;
    static Transform blockToMove;
    static Vector3 whereTo;
    static Vector3 positingOffset;

    static bool winningMovePlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.disableMovement = false;
        goToMainMenuButton.GetComponent<Button>().onClick.AddListener(delegate {
            gameObject.GetComponent<SceneChanger>().MainMenuScene();
        });
        MouseLook.disableCursor();
        Debug.Log("fakaro");
        Debug.Log("packid " + CurrentPackId);
        Debug.Log("levelid " + CurrentLevelId);
        blockDim = wallBlock.transform.localScale.x;
        playerY = player.transform.localPosition.y;
        for (int i = 0; i < GameData.packs.Count; i++)
        {
            if (currentPackId == GameData.packs[i].PackId)
            {
                PackData pack = GameData.packs[i];
                for (int j = 0; j < pack.Levels.Length; j++)
                {
                    if (currentLevelId == pack.Levels[j].LevelId)
                    {
                        renderGrid(pack.Levels[j].Grid);
                        renderNegativeIons(pack.Levels[j].NegativeIons);
                        break;
                    }
                }
                break;
            }
        }
        winningMovePlayed = false;
        Timer.restart();
    }

    void Update()
    {
        if (blockMoving)
        {
            if (Mathf.Abs(blockToMove.position.x - whereTo.x) < 0.1 && Mathf.Abs(blockToMove.position.z - whereTo.z) < 0.1)
            {
                blockToMove.position = whereTo;
                blockMoving = false;
                if (winningMovePlayed)
                {
                    bool packComplete = GameData.levelPassed();
                    winningMovePlayed = false;
                    Timer.pause();
                    PlayerMovement.disableMovement = true;
                    if (packComplete)
                    {
                        Button button = finishPackButton.GetComponent<Button>();
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(delegate {
                            gameObject.GetComponent<SceneChanger>().MainMenuScene();
                        });
                        finishPackPanel.SetActive(true);
                    } else
                    {
                        Button button = nextLevelButton.GetComponent<Button>();
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(delegate {
                            gameObject.GetComponent<SceneChanger>().GameScene(GameData.saveFiles[GameData.currentSaveFileIndex].PackId, GameData.saveFiles[GameData.currentSaveFileIndex].LevelId);
                        });
                        nextLevelPanel.SetActive(true);
                    }
                    MouseLook.enableCursor();
                }
            } else
            {
                blockToMove.position += positingOffset * Time.deltaTime;
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                restartLevel();
            }
        } 
        if (Input.GetKeyDown(KeyCode.H))
        {
            helpPanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            helpPanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                MouseLook.disableCursor();
                Timer.resume();
                PlayerMovement.disableMovement = false;
            } else
            {
                pausePanel.SetActive(true);
                MouseLook.enableCursor();
                Timer.pause();
                PlayerMovement.disableMovement = true;
            }
        }
    }

    void renderGrid(char[,] grid)
    {
        int n = grid.GetLength(0);
        int m = grid.GetLength(1);

        playingGrid = new char[n, m];
        playingBlocks = new List<GameObject>();

        minimapCamera.orthographicSize = Mathf.Max(n * blockDim /2, m * blockDim /2);
        minimapCamera.transform.position = new Vector3((n-1) * blockDim / 2, minimapCamera.transform.position.y, (m-1) * blockDim / 2);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                playingGrid[i, j] = '0';
                switch (grid[i, j])
                {
                    case '1':
                        playingGrid[i,j] = '1';
                        Instantiate(wallBlock, new Vector3(i * blockDim, wallBlock.transform.position.y, j * blockDim), new Quaternion(0, 0, 0, 0));
                        break;
                    case 'P':
                        Instantiate(floorTile, new Vector3(i * blockDim, blockPlace.transform.position.y, j * blockDim), new Quaternion(0, 0, 0, 0));
                        player.transform.position = new Vector3(i * blockDim, playerY, j * blockDim);
                        break;
                    case 'B':
                        playingGrid[i, j] = 'B';
                        Instantiate(floorTile, new Vector3(i * blockDim, blockPlace.transform.position.y, j * blockDim), new Quaternion(0, 0, 0, 0));
                        GameObject tempBlock = Instantiate(block, new Vector3(i * blockDim, wallBlock.transform.position.y, j * blockDim), new Quaternion(0, 0, 0, 0)) as GameObject;
                        playingBlocks.Add(tempBlock);
                        break;
                    case 'X':
                        Instantiate(blockPlace, new Vector3(i * blockDim, blockPlace.transform.position.y, j * blockDim), new Quaternion(0, 0, 0, 0));
                        break;
                    case '0':
                        Instantiate(floorTile, new Vector3(i * blockDim, blockPlace.transform.position.y, j * blockDim), new Quaternion(0, 0, 0, 0));
                        break;
                    default:
                        break;
                }
            }
        }
        referenceGrid = grid;
        blocksPlaced = 0;
    }

    void renderNegativeIons(NegativeIonData[] negativeIons)
    {
        int[] helpX = new int[4] { 1, 0, -1, 0 };
        int[] helpY = new int[4] { 0, -1, 0, 1 };

        for (int i = 0; i < negativeIons.Length; i++)
        {
            float x = (negativeIons[i].X + 0.5f * helpX[negativeIons[i].Side] + (negativeIons[i].PositionX) * helpY[negativeIons[i].Side]) * blockDim;
            float y = (negativeIons[i].PositionY + 0.5f) * blockDim;
            float z = (negativeIons[i].Y + 0.5f * helpY[negativeIons[i].Side] + (negativeIons[i].PositionX) * helpX[negativeIons[i].Side]) * blockDim;
            Instantiate(negativeIon, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0));
        }
    }

    void restartLevel()
    {
        int n = referenceGrid.GetLength(0);
        int m = referenceGrid.GetLength(1);

        int index = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                playingGrid[i, j] = '0';
                switch (referenceGrid[i, j])
                {
                    case '1':
                        playingGrid[i, j] = '1';
                        break;
                    case 'P':
                        player.transform.position = new Vector3(i * blockDim, playerY, j * blockDim);
                        break;
                    case 'B':
                        playingGrid[i, j] = 'B';
                        playingBlocks[index].transform.position = new Vector3(i * blockDim, wallBlock.transform.position.y, j * blockDim);
                        index++;
                        break;
                    case 'X':
                        break;
                    case '0':
                        break;
                    default:
                        break;
                }
            }
        }
        blocksPlaced = 0;
        blockMoving = false;
        winningMovePlayed = false;
        Timer.restart();
    }

    public static bool checkMove(GameObject movingBlockCollider, string direction)
    {
        Transform movingBlock = movingBlockCollider.transform.parent.transform.parent;

        int x = (int)(movingBlock.position.x / blockDim);
        int y = (int)(movingBlock.position.z / blockDim);

        int[] helpX = new int[4] { 1, 0, -1, 0 };
        int[] helpY = new int[4] { 0, -1, 0, 1 };
        int helpDirection = 0;
        switch (direction)
        {
            case "Down":
                helpDirection = 0;
                break;
            case "Left":
                helpDirection = 1;
                break;
            case "Up":
                helpDirection = 2;
                break;
            case "Right":
                helpDirection = 3;
                break;
            default:
                break;
        }

        int newX = x + helpX[helpDirection];
        int newY = y + helpY[helpDirection];

        if (playingGrid[newX, newY] == '0')
        {
            return true;
        } else
        {
            return false;
        }
    }

    public static void moveBlock(GameObject movingBlockCollider, string direction)
    {

        Transform movingBlock = movingBlockCollider.transform.parent.transform.parent;

        int x = (int)(movingBlock.position.x / blockDim);
        int y = (int)(movingBlock.position.z / blockDim);

        int[] helpX = new int[4] { 1, 0, -1, 0 };
        int[] helpY = new int[4] { 0, -1, 0, 1 };
        int helpDirection = 0;
        switch (direction)
        {
            case "Down":
                helpDirection = 0;
                break;
            case "Left":
                helpDirection = 1;
                break;
            case "Up":
                helpDirection = 2;
                break;
            case "Right":
                helpDirection = 3;
                break;
            default:
                break;
        }


        for (int i=0;i<playingBlocks.Count;i++)
        {
            if(movingBlock.transform.position == playingBlocks[i].transform.position)
            {
                int newX = x + helpX[helpDirection];
                int newY = y + helpY[helpDirection];
                if (playingGrid[newX, newY] == '0')
                {
                    playingGrid[newX, newY] = 'B';
                    playingGrid[x, y] = '0';
                    if (referenceGrid[x, y] == 'X') blocksPlaced--;
                    if (referenceGrid[newX, newY] == 'X')
                    {
                        blocksPlaced++;
                        if (blocksPlaced == playingBlocks.Count)
                        {
                            winningMovePlayed = true;
                        }
                    }
                    whereTo = new Vector3(newX * blockDim, movingBlock.position.y, newY * blockDim);
                    blockToMove = movingBlock;
                    positingOffset = whereTo - blockToMove.position;
                    blockMoving = true;
                    break;
                } else
                {
                    Debug.Log("cant move");
                }
            }
        }
    }

}
