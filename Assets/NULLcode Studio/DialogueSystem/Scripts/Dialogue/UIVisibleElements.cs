using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisibleElements : MonoBehaviour
{

    public List<GameObject> objs;
    
    private void OnEnable()
    {
        SliderController.OnGameOver += GameOver;
        
        UIGameOverController.OnGoNext += Next;
    }

    private void OnDisable()
    {
        SliderController.OnGameOver -= GameOver;
        
        UIGameOverController.OnGoNext -= Next;
    }
    
    private void Next()
    {
        foreach (var obj in objs)
        {
            obj.SetActive(true);
        }
    }

    private void GameOver(bool value)
    {
        foreach (var obj in objs)
        {
            obj.SetActive(false);
        }
    }
}
