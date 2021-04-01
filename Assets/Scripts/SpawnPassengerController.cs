using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPassengerController : MonoBehaviour
{
    public List<GameObject> passengers;

    public Transform spawnPoint;
    
    private void OnEnable()
    {
        CameraController.OnPassengerSpawn += Spawn;
    }

    private void OnDisable()
    {
        CameraController.OnPassengerSpawn -= Spawn;
    }

    private void Spawn()
    {
        var go = Instantiate(passengers[PlayerPrefs.GetInt("DialogID", 0)], spawnPoint.position, Quaternion.identity, spawnPoint);
        
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
    }
}
