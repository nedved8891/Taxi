using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISmilesSpawner : MonoBehaviour
{
   public GameObject prefab;

   public Transform parent;
   
   public Transform pos;

   private AnswerStatus isGoodAnswer;
   
   private void OnEnable()
   {
        DialogueManager.OnReceivedAnswer += CheckAnswer;  

        DriverController.OnPassengerReaction += ReceivedAnswer;
   }

   private void OnDisable()
   {
       DialogueManager.OnReceivedAnswer -= CheckAnswer; 
       
       DriverController.OnPassengerReaction -= ReceivedAnswer;
   }

   private void CheckAnswer(AnswerStatus value)
   {
       isGoodAnswer = value;
   }

   private void ReceivedAnswer(AnswerStatus value)
   {
       var sm =  Instantiate (prefab, pos.position, Quaternion.identity, parent);

       sm.GetComponent<UISmilesController>().Init(value, isGoodAnswer);
   }

}
