using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject
	{
		public static Player instance = null;				//Allows other scripts to call functions from Player.				
	
		public float restartLevelDelay=0.1f;				//Delay time in seconds to restart level.
		public float ammoSpeed = 0.1f;						//How fast ammo goes
		public GameObject[] candyCaneAmmo;					//select an ammo
		public GameObject[] ornamentAmmo;					//select an ammo
		public GameObject[] boxAmmo;						//select an ammo

		public Sprite[] candyCane;
		public Sprite[] ornament;
		public Sprite[] box; 

		public Animator animator;							//Used to store a reference to the Player's animator component.
		private int currentLevel;

		private bool candySelected;
		private bool ornamentSelected;
		private bool boxSelected;

		private bool canFire = true;
		private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.

		//Start overrides the Start function of MovingObject
		protected override void Start ()
		{			
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();

			currentLevel = GameManager.instance.level;

			if(currentLevel > 0 && currentLevel < 5)
				{
				candySelected = true;

				GameObject.FindGameObjectWithTag("CandySelection").GetComponent<SpriteRenderer>().sprite = candyCane[1];
				}
			else if(currentLevel > 4 && currentLevel < 9)
			{
				ornamentSelected = true;

				GameObject.FindGameObjectWithTag("OrnSelection").GetComponent<SpriteRenderer>().sprite = ornament[1];
			}
			else if(currentLevel > 8 && currentLevel < 13)
			{
				boxSelected = true;

				GameObject.FindGameObjectWithTag("BoxSelection").GetComponent<SpriteRenderer>().sprite = box[1];
			}

			Cursor.visible = false;
			//Call the Start function of the MovingObject base class.
			base.Start ();		
		}
		
		private void Update ()
		{						
			//If it's not the player's turn, exit the function.
			if(!GameManager.instance.playersTurn) return;

			if(Input.GetKey(KeyCode.Alpha1))
			{
				candySelected = true;
				ornamentSelected = false;
				boxSelected = false;

				GameObject.FindGameObjectWithTag("CandySelection").GetComponent<SpriteRenderer>().sprite = candyCane[1];
				GameObject.FindGameObjectWithTag("OrnSelection").GetComponent<SpriteRenderer>().sprite = ornament[0];
				GameObject.FindGameObjectWithTag("BoxSelection").GetComponent<SpriteRenderer>().sprite = box[0];
			}
			else if(Input.GetKey(KeyCode.Alpha2) && currentLevel > 4)
			{
				candySelected = false;
				ornamentSelected = true;
				boxSelected = false;

				GameObject.FindGameObjectWithTag("CandySelection").GetComponent<SpriteRenderer>().sprite = candyCane[0];
				GameObject.FindGameObjectWithTag("OrnSelection").GetComponent<SpriteRenderer>().sprite = ornament[1];
				GameObject.FindGameObjectWithTag("BoxSelection").GetComponent<SpriteRenderer>().sprite = box[0];
			}
			else if(Input.GetKey(KeyCode.Alpha3) && currentLevel > 8)
			{
				candySelected = false;
				ornamentSelected = false;
				boxSelected = true;

				GameObject.FindGameObjectWithTag("CandySelection").GetComponent<SpriteRenderer>().sprite = candyCane[0];
				GameObject.FindGameObjectWithTag("OrnSelection").GetComponent<SpriteRenderer>().sprite = ornament[0];
				GameObject.FindGameObjectWithTag("BoxSelection").GetComponent<SpriteRenderer>().sprite = box[1];
			}
			
			//shot an object 
			if(Input.GetKeyDown(KeyCode.Space) && !IsInvoking("CallCandyCaneAmmo") && !IsInvoking("CallOrnamentAmmo") && !IsInvoking("CallBoxAmmo"))
			{
				GameManager.instance.playersTurn = false;
				StartCoroutine(WaitMove());
				if(candySelected)
				{
					animator.SetTrigger ("shoot");
					Invoke("CallCandyCaneAmmo", 0.4f);
				}
				else if(ornamentSelected && currentLevel > 4)
				{
					StartCoroutine("OrnFire");
				}
				else if(boxSelected && currentLevel > 8)
				{
					if(canFire)
					{
						StartCoroutine("BoxFire");
					}
				}
			}	
			
			int horizontal = 0;  	//Used to store the horizontal move direction.
			int vertical = 0;		//Used to store the vertical move direction.

			#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL

			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			
			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = (int) (Input.GetAxisRaw ("Vertical"));
			
			//Check if moving horizontally, if so set vertical to zero.
			if(horizontal != 0)
			{
				vertical = 0;
			}

			//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
			#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.GetTouch(0);

				if (myTouch.position.x < Screen.width/2)
    			{
					if(Input.touchCount > 0 && !IsInvoking("CallCandyCaneAmmo") && !IsInvoking("CallOrnamentAmmo") && !IsInvoking("CallBoxAmmo"))
					{
						GameManager.instance.playersTurn = false;
						StartCoroutine(WaitMove());
						if(candySelected)
						{
							animator.SetTrigger ("shoot");
							Invoke("CallCandyCaneAmmo", 0.4f);
						}
						else if(ornamentSelected && currentLevel > 4)
						{
							StartCoroutine("OrnFire");
						}
						else if(boxSelected && currentLevel > 8)
						{
							if(canFire)
							{
								StartCoroutine("BoxFire");
							}
						}
					}	
    			}	
				else if (myTouch.position.x > Screen.width/2)
    			{
					//Check if the phase of that touch equals Began
					if (myTouch.phase == TouchPhase.Began)
					{
						//If so, set touchOrigin to the position of that touch
						touchOrigin = myTouch.position;
					}
					
					//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
					else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
					{
						//Set touchEnd to equal the position of this touch
						Vector2 touchEnd = myTouch.position;
						
						//Calculate the difference between the beginning and end of the touch on the x axis.
						float x = touchEnd.x - touchOrigin.x;
						
						//Calculate the difference between the beginning and end of the touch on the y axis.
						float y = touchEnd.y - touchOrigin.y;
						
						//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
						touchOrigin.x = -1;
						
						//Check if the difference along the x axis is greater than the difference along the y axis.
						if (Mathf.Abs(x) > Mathf.Abs(y))
							//If x is greater than zero, set horizontal to 1, otherwise set it to -1
							horizontal = x > 0 ? 1 : -1;
						else
							//If y is greater than zero, set horizontal to 1, otherwise set it to -1
							vertical = y > 0 ? 1 : -1;
					}
				}
			}
			
			#endif //End of mobile platform dependendent compilation section started above with #elif
			//Check if we have a non-zero value for horizontal or vertical
			if(horizontal != 0 || vertical != 0)
			{
				//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
				//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
				AttemptMove<Wall> (horizontal, vertical);
			}
			
			if(transform.position == new Vector3(8f, 6f, 0f))
			{
				if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
					transform.position = new Vector3(8f, 1f, 0f);
			
			}
			
			if(transform.position == new Vector3(8f, 1f, 0f))
			{
				if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
					transform.position = new Vector3(8f, 6f, 0f);
				
			}
		}
		
		void LateUpdate()
		{			
			currentLevel = GameManager.instance.level;

			if(KrampsSpawn.instance.totalEnemiesKilled >= KrampsSpawn.instance.totalEnemiesCreated)
			{				
				animator.SetTrigger("normal");
			}
		}
		
		
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
					
			//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
			base.AttemptMove <T> (xDir, yDir);
			
			//Set the playersTurn boolean of GameManager to false now that players turn is over.
			GameManager.instance.playersTurn = false;
			
			StartCoroutine(WaitMove());
			
			//delay between turns	
		}

		void CallCandyCaneAmmo ()
		{
			Vector3 offset = new Vector3(-1f,0f,0f);	
			GameObject toInstantiate = candyCaneAmmo[Random.Range (0,candyCaneAmmo.Length)];	
			GameObject instance = Instantiate (toInstantiate, transform.position + offset, Quaternion.identity) as GameObject;	
			instance.GetComponent<Rigidbody2D>().velocity = new Vector3(-ammoSpeed,0,0);	
			Destroy(instance, 2.1f);
		}

		void CallOrnamentAmmo ()
		{
			Vector3 offset = new Vector3(-1f,0f,0f);	
			GameObject toInstantiate = ornamentAmmo[Random.Range (0,ornamentAmmo.Length)];	
			GameObject instance = Instantiate (toInstantiate, transform.position + offset, Quaternion.identity) as GameObject;	
			instance.GetComponent<Rigidbody2D>().velocity = new Vector3(-ammoSpeed,0,0);	
			Destroy(instance, 2.1f);
		}

		void CallBoxAmmo ()
		{
			Vector3 offset = new Vector3(-1f,0f,0f);	
			GameObject toInstantiate = boxAmmo[Random.Range (0,boxAmmo.Length)];	
			GameObject instance = Instantiate (toInstantiate, transform.position + offset, Quaternion.identity) as GameObject;	
			instance.GetComponent<Rigidbody2D>().velocity = new Vector3(-ammoSpeed,0,0);	
			Destroy(instance, 2.1f);
		}

		
		//OnCantMove overrides the abstract function OnCantMove in MovingObject.
		//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
		protected override void OnCantMove <T> (T component)
		{
			//Set hitWall to equal the component passed in as a parameter.
			//Enemy hitEnemy = component as Enemy;	
			
		}
		
		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		private void OnTriggerEnter2D (Collider2D other)
		{			
								
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{
					if(currentLevel >= 12)
					{
						GameManager.instance.Winner ();				
					}
					else
					{
						//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
						Invoke ("Restart", restartLevelDelay);
						//Disable the player object since level is over.
						enabled = false;
					}
			}
		}
			
		//Restart reloads the scene when called.
		private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game.
			SceneManager.LoadScene("Main");
		}

		IEnumerator WaitMove()
		{
			yield return new WaitForSeconds(0.2f);
			GameManager.instance.playersTurn = true;
		}

		IEnumerator CanFire()
		{
			yield return new WaitForSeconds(0.4f);
			canFire = true;
		}

		IEnumerator OrnFire()
		{
			animator.SetTrigger ("shoot");
			Invoke("CallOrnamentAmmo", 0.3f);
			yield return new WaitForSeconds (0.3f);
			animator.SetTrigger ("shoot");
			Invoke("CallOrnamentAmmo", 0.3f);

		}

		IEnumerator BoxFire()
		{
			animator.SetTrigger ("shoot2");
			Invoke("CallBoxAmmo", 0.1f);
			yield return new WaitForSeconds (0.1f);
			Invoke("CallBoxAmmo", 0.1f);
			yield return new WaitForSeconds (0.1f);
			Invoke("CallBoxAmmo", 0.1f);
			canFire = false;
			StartCoroutine("CanFire");
		}
	}
}

