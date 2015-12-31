using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Wall : MonoBehaviour
	{
		public Sprite[] dmgSprite;					//Alternate sprite to display after Wall has been attacked by player.
		public float hp = 4;						//hit points for the wall.

		[HideInInspector]public static int totalPresents = 0;
		
		private int timesHit = 0;
		
		void Start ()
		{
			totalPresents = GameObject.FindGameObjectsWithTag("Present").Length;
		}
		
		//DamageWall is called when the player attacks a wall.
		public void DamageWall (float loss)
		{
			//Keep track of presents
			HandleHits();
		}
		
		public void HandleHits () {
			// number of times an object has been hit
			timesHit++;
			// number of time an object can be hit
			int maxHits = dmgSprite.Length + 1;
			//if hit too many times do someting
			if(timesHit >= maxHits){
				//subtract from total presents in level
				totalPresents--;
				//destroy the present with too many hits
				Destroy (GetComponent<SpriteRenderer>());
				gameObject.tag = "Untagged";
				gameObject.layer = 8;
				gameObject.SetActive(false);				
			} 
			else {
			
				// if object not destroyed then load next sprite
				LoadSprites ();
			}
			
			//if all presents are gone then game over
			if(totalPresents <= 0)
				//Check to see if game has ended.			
				CheckIfGameOver ();			
		}
		
		//create an array for present damage sprites
		void LoadSprites () {
			int spriteIndex = timesHit - 1; 
			if(dmgSprite[spriteIndex]){
				this.GetComponent<SpriteRenderer>().sprite = dmgSprite[spriteIndex]; 
			}	
		}
		
		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (totalPresents <= 0) 
			{
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();				
			}
		}
	}
}
