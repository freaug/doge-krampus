using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

namespace Completed
	
{
	
	public class BoardManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
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
		
		public int columns = 8; 										//Number of columns in our game board.
		public int rows = 6;											//Number of rows in our game board.
		public GameObject exit;											//Prefab to spawn for exit.
		public GameObject[] floorTiles;									//Array of floor prefabs.
		public GameObject[] wallTiles;									//Array of wall prefabs.
		public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
		public GameObject[] brokenFloor;								//array of broken tiles.
		public GameObject[] backWall;
		
		private Transform boardHolder;												//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();				//A list of possible locations to place tiles.
		private List <Vector3> presentPositions = new List <Vector3> (); 			//list of place to place presents
		private List <Vector3> backWallPos = new List <Vector3> (); 			//list of place to place presents

		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();
			presentPositions.Clear ();
			backWallPos.Clear();

			
			//Loop through x axis (columns).
			for(int x = 6; x <= columns; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 1; y < rows+1; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
			
			//Loop through x axis (columns).
			for(int x = 6; x < 6; x++)
			{
				//Within each column, loop through y axis (rows).
				for(int y = 1; y < 6; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					presentPositions.Add (new Vector3(x, y, 0f));
				}
			}

			for(int x = 9; x < 10; x++)
			{
				for(int y = 1; y <= 5; y++)
				{
					backWallPos.Add (new Vector3(x, y, 0.0001f));
				}	
			}
		}


		
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;
			
			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for(int x = -2; x < columns + 3; x++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for(int y = 0; y < rows + 2; y++)
				{
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
					
					//Check if current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
					if(x == columns + 2 || y == 0 || y == rows + 1)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					
					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					
					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0, gridPositions.Count);
			
			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPosition = gridPositions[randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPosition;
		}
		
		
		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum+1);
			
			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = RandomPosition();
				
				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}
		
		Vector3 PresentPosition ()
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0,	presentPositions.Count);
			
			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPosition = gridPositions[randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPosition;
		}
		
		void LayoutPresentsAtRandom (GameObject[] tileArray, int total)
		{

			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < total; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = PresentPosition();
				
				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}

		Vector3 BackWallPosition ()
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0,	backWallPos.Count);
			
			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPos = backWallPos[randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			backWallPos.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPos;
		}


		void BackWall(GameObject[] tileArray, int total)
		{
			
				for(int i = 0; i < total; i++)
					{

						Vector3 randomPos = BackWallPosition();
						
						GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

						Instantiate(tileChoice, randomPos, Quaternion.identity);


					}
			
		}
		
		void BrokenTiles (GameObject[] brokenArray, int total)
		{
			for(int i = 0; i <= 0; i++)
			{
				Vector3 pos1 = new Vector3 (7, 1, -0.0001f);
				Vector3 pos2 = new Vector3 (7, 2, -0.0001f);
				Vector3 pos3 = new Vector3 (7, 3, -0.0001f); 
				Vector3 pos4 = new Vector3 (7, 4, -0.0001f); 
				Vector3 pos5 = new Vector3 (7, 5, -0.0001f); 
				Vector3 pos6 = new Vector3 (7, 6, -0.0001f); 
			
				GameObject brokenChoice1 = brokenArray[0];
				GameObject brokenChoice2 = brokenArray[1];
				GameObject brokenChoice3 = brokenArray[2];
				GameObject brokenChoice4 = brokenArray[3];
				GameObject brokenChoice5 = brokenArray[4];
				GameObject brokenChoice6 = brokenArray[5];
				
				Instantiate(brokenChoice1, pos1, Quaternion.identity);
				Instantiate(brokenChoice2, pos2, Quaternion.identity);
				Instantiate(brokenChoice3, pos3, Quaternion.identity);
				Instantiate(brokenChoice4, pos4, Quaternion.identity);
				Instantiate(brokenChoice5, pos5, Quaternion.identity);
				Instantiate(brokenChoice6, pos6, Quaternion.identity);
			}
		}
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
			BrokenTiles(brokenFloor, 6);

			//Creates the outer walls and floor.
			BoardSetup ();
			
			//Reset our list of gridpositions.
			InitialiseList ();			

			BackWall(backWall, 5);
		
			LayoutPresentsAtRandom (wallTiles, 6);
							
			//Instantiate the exit tile in the upper right hand corner of our game board
			Instantiate (exit, new Vector3 (9, 6, 0f), Quaternion.identity);
			
			
		}
	}
}
