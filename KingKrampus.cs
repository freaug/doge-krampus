﻿using UnityEngine;
using System.Collections.Generic; 		//Allows us to use Lists.
using System.Collections;
using UnityEngine.UI;					//Allows us to use UI.


namespace Completed
{
	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
	public class KingKrampus : MovingObject
	{
		public float enemyAttackPoints; 					//The amount of food points to subtract from the player when attacking.
		public GameManager gameScript;
		public int hp = 6;
						
		private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private bool skipMove;								//Boolean to determine whether or not enemy should skip a turn or move this turn.
		private bool skipAttack;
		private int waitTime;
		private int timesHit;	
				
		//Start overrides the virtual Start function of the base class.
		protected override void Start ()
		{
			//Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
			//This allows the GameManager to issue movement commands.
			GameManager.instance.AddEnemyToList (this);
						
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//krampusScript = GetComponent<KrampsSpawn> ();
			
			//Call the start function of our base class MovingObject.
			base.Start ();
		}
		
		//Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
		//See comments in MovingObject for more on how base AttemptMove function works.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
			//Check if skipMove is true, if so set it to false and skip this turn.
			if(skipMove && waitTime > 2)
			{
				skipMove = false;
				waitTime = 0;
				return;
				
			}
			else if(!skipMove && waitTime < 3)
			{
				waitTime++;
				return;
			}
			
			//Call the AttemptMove function from MovingObject.
			base.AttemptMove <T> (xDir, yDir);
			
			//Now that Enemy has moved, set skipMove to true to skip next move. MESS WITH THIS TO SET TURN DELAYS FOR HARDER ENEMIES
			skipMove = true;
		}
		
		void Update()
		{			
			if(Wall.totalPresents <= 0)
			{
				enabled = false;
				return;
			}
			else
			{
				target = GameObject.FindGameObjectWithTag ("Present").transform;
			}
		}
		
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveEnemy ()
		{	
			//Declare variables for X and Y axis move directions, these range from -1 to 1.
			//These values allow us to choose between the cardinal directions: up, down, left and right.
			int xDir = 0;
			int yDir = 0;
			
			if(target == null)
			{	
				return;
			}
			else
			{				
				//If the difference in positions is approximately zero (Epsilon) do the following:
				if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
					
					//If the y coordinate of the target's (walls) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
					yDir = target.position.y > transform.position.y ? 1 : -1;
				
				//If the difference in positions is not approximately zero (Epsilon) do the following:
				else
					//Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
					xDir = target.position.x > transform.position.x ? 1 : -1;
				
				//Call the AttemptMove function and pass in the generic parameter Wall, because Enemy is moving and expecting to potentially encounter a Wall
				AttemptMove <Wall> (xDir, yDir);
			}
		}
		
		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
		//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
		protected override void OnCantMove <T> (T component)
		{
			if(skipAttack)
			{
				skipAttack = false;
				return;
			}

			//Declare hitPlayer and set it to equal the encountered component.
			Wall hitPlayer = component as Wall;
			
			//Call the DamageWall function of playerDamage passing it playerDamage, the amount of foodpoints to be subtracted.
			hitPlayer.DamageWall (enemyAttackPoints);
			
			//Set the attack trigger of animator to trigger Enemy attack animation.
			animator.SetTrigger ("Attack");
			
			skipAttack = false;

		}
		
		void OnCollisionEnter2D ()
		{
			HandleHits();	
		}
		
		void HandleHits () 
		{
			timesHit++;
			int maxHits = hp;
						
			if(timesHit >= maxHits)
			{			
				// try player script here
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				animator.SetTrigger("Die");
				Destroy(gameObject,0.5f);
				GameManager.instance.RemoveEnemyFromList (this);
			}			
			
		}
	}
}
