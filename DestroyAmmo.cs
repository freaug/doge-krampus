using UnityEngine;
using System.Collections;

namespace Completed
{

public class DestroyAmmo : MonoBehaviour {

	void OnCollisionEnter2D()
	{
		if(gameObject.tag == "Ammo")
			Destroy(gameObject);
		}
	}
}