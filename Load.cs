using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

	public class Load : MonoBehaviour 
	{		
		public void OnMouseDown()
		{
			SceneManager.LoadScene("Prologue");
		}
	}

