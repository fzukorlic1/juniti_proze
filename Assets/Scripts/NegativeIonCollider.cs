using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeIonCollider : MonoBehaviour
{
    public AudioSource ionSound;
    public AudioClip ionClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("kliknut");
        Timer.reduceTime(120);
        //ionSound.Play();
        AudioSource.PlayClipAtPoint(ionClip, ionSound.transform.position);
        gameObject.SetActive(false);
    }
}
