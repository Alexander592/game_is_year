using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 cameraChange;
    public Vector3 playerChange;
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main.GetComponent<Camera>();        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            cam.transform.position += cameraChange;
            other.transform.position += playerChange;
        }
    }
}
