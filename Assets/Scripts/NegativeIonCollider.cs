using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeIonCollider : MonoBehaviour
{
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
        Timer.reduceTime(5);
        gameObject.SetActive(false);
    }
}
