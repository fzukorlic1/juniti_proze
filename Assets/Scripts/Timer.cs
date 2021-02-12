using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textGUI;

    public static bool isPaused = true;
    public static float timeElapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            float numVal = Mathf.Abs(timeElapsed);
            timeElapsed += Time.deltaTime;
            textGUI.text = (timeElapsed > 0 ? "" : "-") + ((int)numVal / 60).ToString() + ":" + ((int)numVal % 60).ToString();
        }
    }

    public static void setTime(int time)
    {
        timeElapsed = time;
    }

    public static void pause()
    {
        isPaused = true;
    }

    public static void resume()
    {
        isPaused = false;
    }

    public static void restart()
    {
        timeElapsed = 0f;
        isPaused = false;
    }

    public static void reduceTime(int seconds)
    {
        timeElapsed -= seconds;
    }

    public static float getElapsedTime()
    {
        return timeElapsed;
    }
}
