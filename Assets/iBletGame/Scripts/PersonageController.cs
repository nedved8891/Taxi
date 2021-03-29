using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonageController : MonoBehaviour
{
    public static event Action<bool> ChangeMouseState; 
    
    public bool isOpenMouse;

    public float speed = 6;
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter " + other.gameObject.name);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!isOpenMouse)
        {
            isOpenMouse = true;
            
            ChangeMouseState?.Invoke(isOpenMouse);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (isOpenMouse)
        {
            isOpenMouse = false;
            
            ChangeMouseState?.Invoke(isOpenMouse);
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }
}
