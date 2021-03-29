using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    public static event Action<int> OnAddScore;
    
    public static event Action<bool> OnGameOver;
    
    public static event Action<bool> OnReation;

    public UISmilesController1 _smiles;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter " + other.gameObject.name);

        var go = other.GetComponentInChildren<ElementsController>();
        
        if(!go.isExit)
            OnAddScore?.Invoke(go.isGood? 100 : -100);
        else
        {
            OnGameOver?.Invoke(false);
        }
        
        _smiles.Init(!go.isGood, go.isExit);
        
        OnReation?.Invoke(go.isGood);
        
        Destroy(other.gameObject);
    }
}
