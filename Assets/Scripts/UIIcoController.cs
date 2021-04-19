using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIcoController : MonoBehaviour
{
    [Header("Іконка")]
    public Image ico;

    [Header("Іконки")]
    public List<Sprite> sprites;
    
    private void OnEnable()
    {
        DialogueManager.OnDialogStarted += Change;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogStarted -= Change;
    }

    private void Change()
    {
        ico.sprite = sprites[PlayerPrefs.GetInt("DialogID", 0)];
    }
}
