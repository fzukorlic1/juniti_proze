using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public AudioSource runSound;

    public float speed = 8f;

    public static bool disableMovement = false;
    public static bool isMoving = false;
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(!disableMovement && (x != 0 || z != 0))
        {
            if (!runSound.isPlaying) {
                runSound.Play();
            }
            controller.Move(move * speed * Time.deltaTime);
            isMoving = true;
        }
        else {
            isMoving = false;
        }
        
    }
}
