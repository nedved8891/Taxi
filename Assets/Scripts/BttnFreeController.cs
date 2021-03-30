﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BttnFreeController : MonoBehaviour
{
    public static event Action<int> OnStopCar;
    
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
        Activate(false);
    }

    public void DialogStart()
    {
        OnStopCar?.Invoke(1);

        Activate(true);
    }

    private void Activate(bool value)
    {
        DOVirtual.DelayedCall(0, () =>
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
        });
    }
}
