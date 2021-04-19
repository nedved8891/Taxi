using System;
using DG.Tweening;
using UnityEngine;

public class BttnFreeController : MonoBehaviour
{
    public static event Action OnPressBttnFree;

    public GameObject bttn;
    
    private Tween twn;
    
    private void OnEnable()
    {
        GameController.OnRestart += Restart;
    }

    private void OnDisable()
    {
        GameController.OnRestart -= Restart;
    }

    private void Start()
    {
        Visible(true);
    }

    private void Restart()
    {
        Visible(true);
    }
    
    public void DialogStart()
    {
        if (PlayerPrefs.HasKey("DialogID"))
        {
            PlayerPrefs.SetInt("DialogID", PlayerPrefs.GetInt("DialogID") == 3? 0 : PlayerPrefs.GetInt("DialogID") + 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("DialogID", 0);
            PlayerPrefs.Save();   
        }
        
        OnPressBttnFree?.Invoke();
        Debug.Log("### OnPressBttnFree ");

        Visible(false);
    }

    private void Visible(bool value)
    {
        DOVirtual.DelayedCall(0, () =>
        {
            bttn.SetActive(value);
            
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
