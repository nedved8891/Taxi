using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
   public float delaySpawn = 1;

   public Transform spawnPosition;
   
   public List<GameObject> prefabs;
   
   public List<Transform> points;

   private IEnumerator coroutine;

   private bool isPaused;
   
   private void OnEnable()
   {
      AddScore.OnGameOver += GameOver;
      
      UIGameOverController1.OnGoNext += UIGameOverController1OnOnGoNext;
   }

   private void UIGameOverController1OnOnGoNext()
   {
      isPaused = false;
   }

   private void OnDisable()
   {
      AddScore.OnGameOver -= GameOver;
      
      UIGameOverController1.OnGoNext -= UIGameOverController1OnOnGoNext;
   }
   
   private void Start()
   {
      coroutine = Spawn();
      
      StartCoroutine(coroutine);
   }

   private void GameOver(bool value)
   {
      isPaused = true;
   }

   private IEnumerator Spawn()
   {
      foreach (var prefab in prefabs)
      {
         while (isPaused) 
         {
            yield return null;
         }
         
         var go = Instantiate(prefab, spawnPosition.position, Quaternion.identity);
         
         go.GetComponentInChildren<ElementsController>().Move(points[Random.Range(0, points.Count)].position);
         
         yield return new WaitForSeconds(delaySpawn);
      }
   }
}
