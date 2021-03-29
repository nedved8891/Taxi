using UnityEngine;
using System.Collections;

public class TestScene : MonoBehaviour {

	void Update()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			if(Input.GetMouseButtonDown(0))
			{
				DialogueTrigger tr = hit.transform.GetComponent<DialogueTrigger>();
				if(tr != null && tr.fileName != string.Empty)
				{
					DialogueManager.Internal.DialogueStart(tr.fileName);
				}
			}

			if(Input.GetMouseButtonDown(1) && !DialogueManager.isActive)
			{
				hit.transform.gameObject.SetActive(false);
			}
		}
	}
}
