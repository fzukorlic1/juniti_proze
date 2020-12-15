using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NegativeIonData
{
    int x;
    int y;
    //offset from center -0.5 to 0.5
    float positionX;
    float positionY;
    int side;// 0 - down, 1 - left, 2 - up, 3 - right

    public enum SideT
    {
        Down,
        Left,
        Up,
        Right
    }

    public NegativeIonData(int x, int y, float positionX, float positionY, int side)
    {
        this.x = x;
        this.y = y;
        this.positionX = positionX;
        this.positionY = positionY;
        this.side = side;
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public int Side { get => side; set => side = value; }
}
