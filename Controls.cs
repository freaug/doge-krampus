using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Controls : MonoBehaviour {

	public void OnMouseDown()
		{
		SceneManager.LoadScene("Controls");
		}
}
