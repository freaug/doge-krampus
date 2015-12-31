using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


	public class Credits : MonoBehaviour 
	{		
		public void OnMouseDown()
		{
			SceneManager.LoadScene("Credits");
		}
	}

