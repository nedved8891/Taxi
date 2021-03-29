using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISmilesSpawner : MonoBehaviour
{
   public GameObject prefab;

   public Transform parent;
   
   public Transform pos;

   private bool isGoodAnswer;
   
   private void OnEnable()
   {
        DialogueManager.OnReceivedAnswer += CheckAnswer;  

        PersonagesAnimator.OnPassengerReaction += ReceivedAnswer;
   }

   private void OnDisable()
   {
       DialogueManager.OnReceivedAnswer -= CheckAnswer; 
       
       PersonagesAnimator.OnPassengerReaction -= ReceivedAnswer;
   }

   private void CheckAnswer(bool value)
   {
       isGoodAnswer = value;
   }

   private void ReceivedAnswer(bool value)
   {
       var sm =  Instantiate (prefab, pos.position, Quaternion.identity, parent);

       sm.GetComponent<UISmilesController>().Init(value, isGoodAnswer);
   }

}
