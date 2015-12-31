using UnityEngine;						//unity engine
using System;							//serializable 
using System.Collections;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.
using UnityEngine.UI;					//Allows us to use UI.

namespace Completed 
{

	public class KrampsSpawn : MonoBehaviour 
	{
	
		[Serializable]
		public class Count
		{
			public int minimum; 			//Minimum value for our Count class.
			public int maximum; 			//Maximum value for our Count class.
			
			
			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
	
		public static KrampsSpawn instance = null;						//Allows other scripts to call functions from KrampsSpawn.				

		public GameObject[] enemyTiles;									//Array of enemy prefabs.
		public GameManager gameScript;									//Get access to GameManager
		public Text foodText;											//UI Text to display current level complete.
		
		public float startWaitOne=4;									//time before the first wave is spawned
		public float startWaitTwo=6;									//time before the first wave is spawned
		public float startWaitThree=7;									//time before the first wave is spawned
		public float startWaitFour=8;									//time before the first wave is spawned
		
		public float enemySpawnOneWait=2;								// time between each enemy spawned 
		public float enemySpawnTwoWait=3;								// time between each enemy spawned 
		public float enemySpawnThreeWait=4;								// time between each enemy spawned 
		public float enemySpawnFourWait=5;								// time between each enemy spawned 

		private int enemyCountOne;										//stores generated number for enemy type one
		private int enemyCountTwo;										//stores generated number for enemy type two
		private int enemyCountThree;									//stores generated number for enemy type three
		private int enemyCountFour;										//stores generated number for enemy type four

		private float waveWaitDelay=12f;

		public int totalEnemiesCreated=0;								//sum of all enemies created
		private int currentLevel;										//current level
		private int multiplier;											//enemy multiplier

		public int totalEnemiesKilled=0;								//sum of all enemies killed
		private bool enemyState = true;									//should enemies be spawned, true = yes false = no 
		private List <Vector3> gridPositions = new List <Vector3> ();	//A list of possible locations to place enemies.

		void Awake ()
		{
			//Check if there is already an instance of KrampsSpawn
			if (instance == null)
				//if not, set it to this.
				instance = this;
			//If instance already exists:
			else if (instance != this)
				//Destroy this, this enforces our singleton pattern so there can only be one instance of KrampsSpawn.
				Destroy (gameObject);
			
			//Set KrampsSpawn to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
			DontDestroyOnLoad (gameObject);
		}

		// this is called when a level is loaded but not the first time a script is called need to have a start coroutine in start and here
		void OnLevelWasLoaded() 
		{	
			

			gameScript = GetComponent<GameManager>();

			foodText = GameObject.Find("FoodText").GetComponent<Text>();
	
			currentLevel = GameManager.instance.level;

			//destroy game script on winning
			if(currentLevel > 12)
			{
				DestroyImmediate(this);
			}

			// hide level clear text
			foodText.enabled = false;
			
			//spawn random number of enemies for spawn1
			RandomEnemySpawn();	

			//get total enemies created
			LevelSpecificEnemySum();

			//set spawn state to true so the spawner will run
			enemyState = true;
			
			//clear enemy positions
			gridPositions.Clear();
			
			//start the coroutine for spawning 

			if(currentLevel <= 13)
			{
				StartCoroutine(Spawn1());
			}
			if(currentLevel > 3 && currentLevel < 13)
			{
				StartCoroutine(Spawn2());
			}
			if(currentLevel > 7 && currentLevel < 13)
			{
				StartCoroutine(Spawn3());
			}
			if(currentLevel > 11 && currentLevel < 13)
			{
				StartCoroutine(Spawn4());
			}

			if(currentLevel > 0 && currentLevel < 4)
			{
				waveWaitDelay = 12f;
			}
			if(currentLevel > 3 && currentLevel < 9)
			{
				waveWaitDelay = 14f;
			}
			else if(currentLevel > 8 && currentLevel < 13)
			{
				waveWaitDelay = 16f;
			}
		}

		void Update()
		{	
			//total number of enemies that you have killed. taken from the gamemanager where all enemy data is stored
			totalEnemiesKilled = gameScript.eneimesKilled;		
		}
		
		void LateUpdate()
		{	
			if(totalEnemiesKilled >= totalEnemiesCreated)
			{				
				enemyState = false;
				StartCoroutine(WaveComplete());
				ChangeTag();
			}
			
			if(gameScript.gameOver == true)
				enabled = false;
		}
		
		IEnumerator Spawn1()
		{	
			
			yield return new WaitForSeconds(startWaitOne);		
			
			if(enemyState)		
			{
				if(currentLevel == 5 || currentLevel == 9 || currentLevel == 12)
				{
					yield return new WaitForSeconds(15f);
				}

				for(int i = 0; i < (enemyCountOne); i++)
				{	
					Vector3 randomPosition = RandomPosition();
					
					GameObject enemyChoice = enemyTiles[0];					
					
					Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
					
					yield return new WaitForSeconds(enemySpawnOneWait);
					
				}	

				yield return new WaitForSeconds (waveWaitDelay);

				for(int i = 0; i < (enemyCountOne); i++)
				{	
					Vector3 randomPosition = RandomPosition();
					
					GameObject enemyChoice = enemyTiles[0];					
					
					Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
					
					yield return new WaitForSeconds(enemySpawnOneWait);
					
				}	

				yield return new WaitForSeconds (waveWaitDelay);

				for(int i = 0; i < (enemyCountOne); i++)
				{	
					Vector3 randomPosition = RandomPosition();
					
					GameObject enemyChoice = enemyTiles[0];					
					
					Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
					
					yield return new WaitForSeconds(enemySpawnOneWait);
					
				}

				yield return new WaitForSeconds (waveWaitDelay);

				if(currentLevel > 3)
				{
					for(int i = 0; i < (enemyCountOne); i++)
					{	
						Vector3 randomPosition = RandomPosition();
						
						GameObject enemyChoice = enemyTiles[0];					
						
						Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
						
						yield return new WaitForSeconds(enemySpawnOneWait);
						
					}
				}

				yield return new WaitForSeconds (waveWaitDelay);

				if(currentLevel > 8)
				{
					for(int i = 0; i < (enemyCountOne); i++)
					{	
						Vector3 randomPosition = RandomPosition();
						
						GameObject enemyChoice = enemyTiles[0];					
						
						Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
						
						yield return new WaitForSeconds(enemySpawnOneWait);
						
					}
				}								
			}
		}
		
		
		IEnumerator Spawn2()
		{					
			yield return new WaitForSeconds(startWaitTwo);		
			
			if(enemyState)
			{
				if(currentLevel == 5 || currentLevel == 9 || currentLevel == 12)
				{
					yield return new WaitForSeconds(15f);
				}
					for(int i = 0; i < (enemyCountTwo); i++)
					{	
						
						Vector3 randomPosition = RandomPosition();
						
						GameObject enemyChoice = enemyTiles[1];					
						
						Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
						
						yield return new WaitForSeconds(enemySpawnTwoWait);		
					}

					yield return new WaitForSeconds (waveWaitDelay);

					for(int i = 0; i < (enemyCountTwo); i++)
					{	
						
						Vector3 randomPosition = RandomPosition();
						
						GameObject enemyChoice = enemyTiles[1];					
						
						Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
						
						yield return new WaitForSeconds(enemySpawnTwoWait);		
					}	

					yield return new WaitForSeconds (waveWaitDelay);

					if(currentLevel > 4)
					{
						for(int i = 0; i < (enemyCountTwo); i++)
						{	
							
							Vector3 randomPosition = RandomPosition();
							
							GameObject enemyChoice = enemyTiles[1];					
							
							Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
							
							yield return new WaitForSeconds(enemySpawnTwoWait);		
						}
					}

					yield return new WaitForSeconds (waveWaitDelay);

					if(currentLevel > 8)
					{
						for(int i = 0; i < (enemyCountTwo); i++)
						{	
							
							Vector3 randomPosition = RandomPosition();
							
							GameObject enemyChoice = enemyTiles[1];					
							
							Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
							
							yield return new WaitForSeconds(enemySpawnTwoWait);		
						}
					}		
				}
		}
		
		IEnumerator Spawn3()
		{	
			yield return new WaitForSeconds(startWaitThree);		
			
			if(enemyState)
			{

				if(currentLevel == 5 || currentLevel == 9 || currentLevel == 12)
				{
					yield return new WaitForSeconds(15f);
				}

				for(int i = 0; i < (enemyCountThree); i++)
				{	
					
					Vector3 randomPosition = RandomPosition();
					
					GameObject enemyChoice = enemyTiles[2];					
					
					Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
					
					yield return new WaitForSeconds(enemySpawnThreeWait);		
				}	

				yield return new WaitForSeconds (waveWaitDelay);

				for(int i = 0; i < (enemyCountThree); i++)
				{	
					
					Vector3 randomPosition = RandomPosition();
					
					GameObject enemyChoice = enemyTiles[2];					
					
					Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
					
					yield return new WaitForSeconds(enemySpawnThreeWait);		
				}	

				yield return new WaitForSeconds (waveWaitDelay);

				if(currentLevel > 8)
				{
					for(int i = 0; i < (enemyCountThree); i++)
					{	
						
						Vector3 randomPosition = RandomPosition();
						
						GameObject enemyChoice = enemyTiles[2];					
						
						Instantiate(enemyChoice, randomPosition, Quaternion.identity);	
						
						yield return new WaitForSeconds(enemySpawnThreeWait);		
					}
				}			
			}			
		}
		
		IEnumerator Spawn4()
		{				
			yield return new WaitForSeconds(startWaitFour);		
			
			if(enemyState)
			{
				if(currentLevel == 5 || currentLevel == 9 || currentLevel == 12)
				{
					yield return new WaitForSeconds(15f);
				}

				for(int i = 0; i < (enemyCountFour); i++)
				{	
					
					GameObject enemyChoice = enemyTiles[3];					
					
					Instantiate(enemyChoice, new Vector3(-3f, 4f, 0f), Quaternion.identity);	
					
					yield return new WaitForSeconds(enemySpawnFourWait);		
				}				
			}			
		}
		
		void ChangeTag()
		{
			var exit = GameObject.FindWithTag("Exit");
			exit.layer = 0;
			exit.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
		}	
		
		void RandomEnemySpawn()
		{
			if(currentLevel > 0 && currentLevel < 4)
			{
				enemyCountOne = Random.Range(8,10);
				enemyCountTwo = Random.Range(4,5);
			}
			else if(currentLevel > 4 && currentLevel < 8)
			{
				enemyCountOne = Random.Range(10, 12);
				enemyCountTwo = Random.Range(7,8);
				enemyCountThree = Random.Range(3,4);
			}
			else if (currentLevel > 8 && currentLevel < 12)
			{
				enemyCountOne = Random.Range(12, 14);
				enemyCountTwo = Random.Range(9,11);
				enemyCountThree = Random.Range(5,6);
			}
			else if (currentLevel > 11)
			{
				enemyCountOne = Random.Range(14, 16);
				enemyCountTwo = Random.Range(13,15);
				enemyCountThree = Random.Range(7,8);
				enemyCountFour = 1;
			}
		}
		
		void LevelSpecificEnemySum()
		{
			if(currentLevel == 1 || currentLevel == 2 || currentLevel == 3)
			{
				totalEnemiesCreated = (enemyCountOne * 3);
			}
			else if (currentLevel == 4)
			{
				totalEnemiesCreated = ((enemyCountOne * 4 ) + (enemyCountTwo * 2));
			}
			else if (currentLevel == 5 || currentLevel == 6 || currentLevel == 7)
			{
				totalEnemiesCreated = ((enemyCountOne * 4 ) + (enemyCountTwo * 3));
			} 
			else if(currentLevel == 8)
			{
				totalEnemiesCreated = ((enemyCountOne * 4) + (enemyCountTwo * 3) + (enemyCountThree * 2));
			}
			else if ( currentLevel == 9 || currentLevel == 10 || currentLevel == 11)
			{
				totalEnemiesCreated = ((enemyCountOne * 5) + (enemyCountTwo * 4) + (enemyCountThree * 3));
			}
			else if(currentLevel == 12)
			{
					totalEnemiesCreated = (((enemyCountOne * 5) + (enemyCountTwo * 4) + (enemyCountThree * 3) + enemyCountFour));
			}
		}
		
		void RandomPos()
		{
			//Loop through x axis (columns).
			for(int x = -3; x < -1; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 1; y < 7; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			RandomPos();
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0, gridPositions.Count);
			
			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPosition = gridPositions[randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPosition;
		}
		
		IEnumerator WaveComplete()
		{
			foodText.enabled = true;
			foodText.text = "Night " + currentLevel + " success!";
			yield return new WaitForSeconds (4f);
			foodText.enabled = false;
		}	
	}
}