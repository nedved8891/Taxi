﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public enum NDialogs
{
    Taxi_1,
    Taxi_2,
    Taxi_3,
}

public class BttnFreeController : MonoBehaviour
{
    public NDialogs dialogName;

    public GameObject bttn;
    
    private Tween twn;
    
    private void OnEnable()
    {
        DialogueManager.OnVisibleDialog += Activate;
        
        UIGameOverController.OnGoNext += Next;
    }

    private void OnDisable()
    {
        DialogueManager.OnVisibleDialog -= Activate;
        
        UIGameOverController.OnGoNext -= Next;
    }

    private void Start()
    {
        Activate(false);
    }

    private void Next()
    {
        dialogName = NDialogs.Taxi_1;

        Activate(false);
    }

    public void DialogStart()
    {
        DialogueManager._internal.DialogueStart(dialogName.ToString());
    }

    private void Activate(bool value)
    {
        bttn.SetActive(!value);
        
        bttn.transform.localScale = Vector3.one;

        if (bttn.activeSelf)
        {
            twn = bttn.transform.DOScale(bttn.transform.localScale + new Vector3(0.15f, 0.15f, 0.15f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            if (twn != null)
            {
                twn.Kill();
                twn = null;
            }
        }
    }
}