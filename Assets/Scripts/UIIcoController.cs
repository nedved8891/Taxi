using System.Collections;
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
        CameraController.OnPassengerSpawn += Change;
    }

    private void OnDisable()
    {
        CameraController.OnPassengerSpawn -= Change;
    }

    private void Change()
    {
        ico.sprite = sprites[PlayerPrefs.GetInt("DialogID", 0)];
    }
}
