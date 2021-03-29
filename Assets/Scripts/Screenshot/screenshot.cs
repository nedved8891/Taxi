using UnityEngine;
using System.Collections;

public class screenshot : MonoBehaviour 
{
	private void Update () 
	{
		if(Input.GetKey(KeyCode.F1))
		{
			ScreenCapture.CaptureScreenshot("shot_" + Time.time + ".png", 2);
		}
	}
}
