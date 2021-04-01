using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Animator DoorAnimator;
    [SerializeField] Animator PeopleAnimator;
    private void OnEnable()
    {
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenDoor()
    {
        DoorAnimator.SetTrigger("OpenDoor");
    }
    public void CloseDoor()
    {
        DoorAnimator.SetTrigger("CloseDoor");
    }
}
