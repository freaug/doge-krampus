using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Completed
{
	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.1f;							//Delay between each Player turn.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		[HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
		public int level;										//Current level number, expressed in game as "Day 1".

		public List<Enemy> enemies;								//List of all Enemy units, used to issue the move commands.
		public List<EnemyHard> enemiesHard;
		public List<EnemyFast> enemiesFast;
		public List<KingKrampus> kingKrampus;

		public AudioClip levelOne;
		public AudioClip levelFive;
		public AudioClip levelEight;

		public int eneimesKilled;
		public bool end = false;
		public bool gameOver = false;

		private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		private GameObject levelImage2;
		private GameObject levelImage3;
		private GameObject levelImage4;

		private Text levelText;									//Text to display current level number.
		private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
		private KrampsSpawn krampusScript;
		private SoundManager soundScript;

		//This is called each time a scene is loaded.
		void OnLevelWasLoaded(int index)
		{	
			if(end == true || gameOver == true)
			{
				enabled = false;
				return;
			}
			//Add one to our level number.
			level++;

			if(level == 5 || level == 9 || level == 12)
			{
				levelStartDelay = 15f;
			}
			else
			{
				levelStartDelay = 2f;
			}

			if(level > 0 && level < 5)
			{
				if(SoundManager.instance.musicSource.clip == levelOne)
				{}
				else
				{
					SoundManager.instance.musicSource.clip = levelOne;
					SoundManager.instance.musicSource.Play();
				}
			}
			else if(level > 3 && level < 8)
			{
				if(SoundManager.instance.musicSource.clip == levelFive)
				{}
				else
				{
					SoundManager.instance.musicSource.clip = levelFive;
					SoundManager.instance.musicSource.Play();
				}
			}
			else if(level > 8 && level < 13)
			{
				if(SoundManager.instance.musicSource.clip == levelEight)
				{}
				else
				{
					SoundManager.instance.musicSource.clip = levelEight;
					SoundManager.instance.musicSource.Play();
				}
			}

									
			//Check if instance already exists
			if (instance == null)
				
				//if not, set instance to this
				instance = this;
			
			//If instance already exists and it's not this:
			else if (instance != this)
				
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);	
			
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
			
			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();
			
			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardManager>();
			
			krampusScript = GetComponent<KrampsSpawn>();

			//playerScript = GetComponent<Player>();
			
			//Call the InitGame function to initialize the first level 
			InitGame();
			
			ResetExit();
		}
		
		//Initializes the game for each level.
		void InitGame()
		{			
		
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;
		
				
			//Get a reference to our image LevelImage by finding it by name.
			levelImage = GameObject.Find("LevelImage");
			levelImage2 = GameObject.Find("LevelImage2");
			levelImage3 = GameObject.Find("LevelImage3");
			levelImage4 = GameObject.Find("LevelImage4");

			
			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();

			//Set the text of levelText to the string "Day" and append the current level number.
			levelText.text = "Night " + level;
			
			//Set levelImage to active blocking player's view of the game board during setup.
			if(level == 5)
				{
					levelImage2.SetActive(true);
					levelImage.SetActive(false);
				}
			else if (level == 9)
				{
					levelImage3.SetActive(true);
					levelImage2.SetActive(false);
					levelImage.SetActive(false);
				}
			else if(level == 12)
				{
					levelImage4.SetActive(true);
					levelImage3.SetActive(false);
					levelImage2.SetActive(false);
					levelImage.SetActive(false);
				}
			else
				{
				levelImage.SetActive(true);
				}

			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);
			
			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();

			//Call the SetupScene function of the BoardManager script, pass it current level number.
			boardScript.SetupScene(level);

			eneimesKilled = 0;			
		}
		
		
		//Hides black image used between levels
		void HideLevelImage()
		{
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);
			levelImage2.SetActive(false);
			levelImage3.SetActive(false);
			levelImage4.SetActive(false);

			
			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		}
		
		//Update is called every frame.
		void Update()
		{			
			//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
			if(enemiesMoving || doingSetup)
				
				//If any of these are true, return and do not start MoveEnemies.
				return;
			//Start moving enemies.
			StartCoroutine (MoveEnemies ());			
			
		}
		
		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Add(script);
		}

		public void RemoveEnemyFromList(Enemy script)
		{			
			enemies.Remove(script);
			eneimesKilled++;
		}

		public void AddEnemyToList(EnemyFast script)
		{
			enemiesFast.Add (script);
		}
		
		public void RemoveEnemyFromList(EnemyFast script)
		{			
			enemiesFast.Remove(script);
			eneimesKilled++;
		}

		public void AddEnemyToList(KingKrampus script)
		{
			kingKrampus.Add (script);
		}

		public void RemoveEnemyFromList(KingKrampus script)
		{
			kingKrampus.Remove(script);
			eneimesKilled++;
		}

		public void AddEnemyToList(EnemyHard script)
		{
			enemiesHard.Add (script);
		}

		public void RemoveEnemyFromList(EnemyHard script)
		{
			enemiesHard.Remove(script);
			eneimesKilled++;
		}
		
		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{	
					
			//Disable this GameManager.
			enabled = false;

			gameOver = true;
			
			Destroy(this, 0.2f);

			krampusScript.StopAllCoroutines();

			enemies.Clear();

			DestroyObject(KrampsSpawn.instance);
			
			DestroyObject(SoundManager.instance);
			
			SceneManager.LoadScene("Lose Menu");		
		}

		public void Winner()
		{
			krampusScript.StopAllCoroutines();
			krampusScript.enabled = false;
			Destroy(GetComponent<KrampsSpawn>());					
			enemies.Clear();
			//Disable this GameManager.
			enabled = false;

			gameOver = true;
			
			Destroy(this, 0.2f);

			SceneManager.LoadScene("BetweenLvl12");
		}
		
		//Coroutine to move enemies in sequence.
		IEnumerator MoveEnemies()
		{
			//While enemiesMoving is true player is unable to move.
			enemiesMoving = true;
			
			//Wait for turnDelay seconds, defaults to .1 (100 ms).
			yield return new WaitForSeconds(turnDelay);
			
			//Loop through List of Enemy objects.
			for (int i = 0; i < enemies.Count; i++)
			{
				//Call the MoveEnemy function of Enemy at index i in the enemies List.
				enemies[i].MoveEnemy ();				
				
				//Wait for Enemy's moveTime before moving next Enemy, 
				if(enemies.Count == 0)
				{	
					break;
				}
				else
				{	
					yield return new WaitForSeconds(enemies[i].moveTime);
				}
			}
			
			for (int i = 0; i < kingKrampus.Count; i++)
			{	
				//Call the MoveEnemy function of Enemy at index i in the enemies List.
				kingKrampus[i].MoveEnemy ();				
				
				//Wait for Enemy's moveTime before moving next Enemy, 
				if(kingKrampus.Count == 0)
				{	
					break;
				}
				else
				{	
					yield return new WaitForSeconds(kingKrampus[i].moveTime);
				}
			}

			for (int i = 0; i < enemiesHard.Count; i++)
			{
				//Call the MoveEnemy function of Enemy at index i in the enemies List.
				enemiesHard[i].MoveEnemy ();				
				
				//Wait for Enemy's moveTime before moving next Enemy, 
				if(enemiesHard.Count == 0)
				{	
					break;
				}
				else
				{	
					yield return new WaitForSeconds(enemiesHard[i].moveTime);
				}
			}


			for (int i = 0; i < enemiesFast.Count; i++)
			{
				//Call the MoveEnemy function of Enemy at index i in the enemies List.
				enemiesFast[i].MoveEnemy ();				
				
				//Wait for Enemy's moveTime before moving next Enemy, 
				if(enemiesFast.Count == 0)
				{	
					break;
				}
				else
				{	
					yield return new WaitForSeconds(enemiesFast[i].moveTime);
				}
			}
			//Enemies are done moving, set enemiesMoving to false.
			enemiesMoving = false;
		}
		
		void ResetExit()
		{
			var exit = GameObject.FindWithTag("Exit");
			exit.layer = 8;
			exit.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
		}
		
		void OnGUI() {
			GUI.contentColor = new Color(0f, 0f, 0f, 0f);
			GUILayout.Label ("Test");
		}
	}
}

