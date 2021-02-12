using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSensitivity : MonoBehaviour
{
    public static float mouseSens = 0.65f;

    public void setSensitivity(float value) {
        mouseSens = value;
    }
}
