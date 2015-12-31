using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour 
{
	public void Awake ()
	{
		Cursor.visible = true;
	}


	public void OnMouseDown()
	{
		SceneManager.LoadScene("Start Menu");
	}
}
