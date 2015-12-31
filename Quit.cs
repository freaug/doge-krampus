using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour 
{
	public void Awake ()
	{
		Cursor.visible = true;
	}
	
	public void OnMouseDown()
	{
		Application.Quit();
	}
}
