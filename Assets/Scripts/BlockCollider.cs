using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{

    public GameObject interactionCanvas;

    private Collider collisionBlock;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionBlock != null && Input.GetKeyDown(KeyCode.E))
        {
            LevelManager.moveBlock(collisionBlock.gameObject, collisionBlock.name);
            switch (collisionBlock.name)
            {
                case "Down":
                    break;
                case "Up":
                    break;
                case "Right":
                    break;
                case "Left":
                    break;
                default:
                    break;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(LevelManager.checkMove(other.gameObject, other.name))
        {
            interactionCanvas.SetActive(true);
            collisionBlock = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactionCanvas.SetActive(false);
        collisionBlock = null;
    }
}
