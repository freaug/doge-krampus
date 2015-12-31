using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PrologueWait : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		StartCoroutine("TimeToWait");
	}
	
	IEnumerator TimeToWait()
	{
		yield return new WaitForSeconds(15f);
		SceneManager.LoadScene("Main");
	}
}
