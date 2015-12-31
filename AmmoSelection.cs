using UnityEngine;
using System.Collections;

public class AmmoSelection : MonoBehaviour {

	public Sprite[] candyCane;
	public Sprite[] ornament;
	public Sprite[] box; 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Alpha1))
		{
			GameObject.FindGameObjectWithTag("CandySelection").GetComponent<SpriteRenderer>().sprite = candyCane[1];
			GameObject.FindGameObjectWithTag("OrnSelection").GetComponent<SpriteRenderer>().sprite = ornament[0];
			GameObject.FindGameObjectWithTag("BoxSelection").GetComponent<SpriteRenderer>().sprite = box[0];
		}
		else if(Input.GetKey(KeyCode.Alpha2))
		{
			GameObject.FindGameObjectWithTag("CandySelection").GetComponent<SpriteRenderer>().sprite = candyCane[0];
			GameObject.FindGameObjectWithTag("OrnSelection").GetComponent<SpriteRenderer>().sprite = ornament[1];
			GameObject.FindGameObjectWithTag("BoxSelection").GetComponent<SpriteRenderer>().sprite = box[0];
		}
		else if(Input.GetKey(KeyCode.Alpha3))
		{
			GameObject.FindGameObjectWithTag("CandySelection").GetComponent<SpriteRenderer>().sprite = candyCane[0];
			GameObject.FindGameObjectWithTag("OrnSelection").GetComponent<SpriteRenderer>().sprite = ornament[0];
			GameObject.FindGameObjectWithTag("BoxSelection").GetComponent<SpriteRenderer>().sprite = box[1];
		}
	}
}
