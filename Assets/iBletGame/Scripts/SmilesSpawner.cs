using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmilesSpawner : MonoBehaviour
{
    public GameObject prefab;

    public Transform parent;
   
    private void OnEnable()
    {
        //AddScore.OnReation += Reation;
    }

    private void OnDisable()
    {
        //AddScore.OnReation -= Reation;
    }

    private void Reation(bool value, Vector3 pos)
    {
        var sm =  Instantiate (prefab, pos, Quaternion.identity, parent);
        
        sm.GetComponent<UISmilesController>().Init(!value, value);
    }

}
