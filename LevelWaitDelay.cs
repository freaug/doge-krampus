using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelWaitDelay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("TimeToWait");
	}
	
	IEnumerator TimeToWait()
	{
		yield return new WaitForSeconds(15f);
		SceneManager.LoadScene("WinScreen");
	}
}
