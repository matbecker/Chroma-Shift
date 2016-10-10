﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hero : MonoBehaviour {

	[System.Serializable]
	public class Stats
	{
		public int currentHealth;
		public int maxHealth;
		public int attackPower;
		public int defence;
		public float currentShieldStrength;
		public float shieldCapacity;
		public float attackRange;
		public float attackSpeed;
		public Vector2 maxVelocity;
		public Vector2 jumpForce;
		public Vector2 movementForce;
	}

	[SerializeField] protected Stats stats;
	[SerializeField] protected Rigidbody2D rb;
	[SerializeField] protected EdgeCollider2D col;
	[SerializeField] protected BoxCollider2D trig;
	[SerializeField] protected LayerMask groundLayers;
	[SerializeField] protected LayerMask attackLayers;
	[SerializeField] protected Image shieldBar;
	[SerializeField] protected bool grounded;
	[SerializeField] protected bool startShieldTimer;
	[SerializeField] protected bool canBlock;
	[SerializeField] protected bool isBlocking;
	[SerializeField] protected bool rechargeShield;
	[SerializeField] protected bool depleteOnHit;
	[SerializeField] protected bool rangedAttack;

	private Coroutine transparencyCor;

	// Use this for initialization
	protected virtual void Start () 
	{
		//register jump event
		InputManager.Instance.Jump += Jump;
		//register run event
		InputManager.Instance.Run += Run;
		//register attack event
		InputManager.Instance.Attack += Attack;
		//register Block event
		InputManager.Instance.Block += Block;
		InputManager.Instance.UnBlock += UnBlock;
		//get the rigidbody of the gameobject
		rb = gameObject.GetComponent<Rigidbody2D>();
		//get the edge collider of the gameobject
		col = gameObject.GetComponent<EdgeCollider2D>();
		//get the box trigger of the gameObject
		trig = gameObject.GetComponent<BoxCollider2D>();
		//ignore all layers but the ground layer
		groundLayers = 1 << LayerMask.NameToLayer("Ground");
		//ignore al layers but attack layer
		attackLayers = 1 << LayerMask.NameToLayer("Attackable");
		//ensure the player starts with max health
		stats.currentHealth = stats.maxHealth;
		//ensure the players shield is at max capacity
		stats.currentShieldStrength = stats.shieldCapacity;
		//get the shield health bar from the canvas object attached to the player
		shieldBar = gameObject.GetComponentInChildren<Image>();
		//the player can block when they start
		canBlock = true;
		//ensure the players shieldbar is not showing
		shieldBar.canvasRenderer.SetAlpha(0.01f);

		transparencyCor = null;
	}
	protected virtual void OnDestroy () 
	{
		//unregister jump event
		InputManager.Instance.Jump -= Jump;
		//unregister run event
		InputManager.Instance.Run -= Run;
		//unregister attack event
		InputManager.Instance.Attack -= Attack;
		//unregister Block event
		InputManager.Instance.Block -= Block;
		InputManager.Instance.UnBlock -= UnBlock;
	}

	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if (Input.GetKeyDown("t"))
		{
			stats.currentShieldStrength -= 0.5f;
		}
		//if the heros shield doesnt depletes when it is hit
		if (!depleteOnHit)
		{
			//start the shield timer
			if (startShieldTimer)
			{
				//shield depletes based over time
				stats.currentShieldStrength -= Time.deltaTime;
			}
		}
		//players shield depletes when it is hit
		else
		{
			//if the player is not blocking and their shield strength is not at its max capacity
			if (!isBlocking && stats.currentShieldStrength < stats.shieldCapacity)
			{
				//increase their shield strength over time
				stats.currentShieldStrength += Time.deltaTime;
				//check if their shield is full
				CheckShieldFull();
			}
				
		}
		//if there shield is empty
		if (stats.currentShieldStrength <= 0.0f)
		{
			//their shield is broken and they can not use it 
			ShieldDepleted();
		}
		//if the player can not block
		if (!canBlock)
		{
			//recharge their shield over time
			stats.currentShieldStrength += Time.deltaTime;
			//check if their shield is full
			CheckShieldFull();
		}
		//make the x scale of the shield bar image equal the heros current shield strength / their max shield strength (value between 0-1)
		shieldBar.rectTransform.localScale = new Vector2(stats.currentShieldStrength / stats.shieldCapacity, 1.0f);

	}
	private void CheckShieldFull()
	{
		//if the players shield s full 
		if (stats.currentShieldStrength >= stats.shieldCapacity)
		{
			//check to see if the coroutine is still running
			if(transparencyCor != null)
			{
				//stop it from running
				StopCoroutine(transparencyCor);
				transparencyCor = null;
			}
			// dont let their shield capcity go higher than the max capacity
			stats.currentShieldStrength = stats.shieldCapacity;
			//Fade out the shield bar image
			shieldBar.CrossFadeAlpha(0.01f, 0.4f, false);
			//player can block once again
			canBlock = true;
		}
	}
	protected virtual void Jump()
	{
		//if the player is on the ground layer then apply a force in the y direction
		if (GroundCheck())
			rb.AddForce(stats.jumpForce);

		if (rb.velocity.y > stats.maxVelocity.y)
			rb.velocity = new Vector2(rb.velocity.x, stats.maxVelocity.y);
	}
	protected virtual void Run(float horizontalAxis)
	{
		if (horizontalAxis > 0)
		{
			rb.AddForce(stats.movementForce);

			//limit player velocity in right direction
			if (rb.velocity.x > stats.maxVelocity.x)
				rb.velocity = new Vector2(stats.maxVelocity.x, rb.velocity.y);
		}
		if (horizontalAxis < 0)
		{
			rb.AddForce(-stats.movementForce);

			//limit players velocity in left direction
			if (rb.velocity.x < -stats.maxVelocity.x) 
				rb.velocity = new Vector2(-stats.maxVelocity.x, rb.velocity.y);
		}
	}
	protected virtual void Attack()
	{
		if (rangedAttack)
		{
			Debug.Log("Ranged Attack!");
		}
		else
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position + trig.bounds.extents, Vector2.right, stats.attackRange, attackLayers);

			if (hit.collider != null)
				Debug.Log("Attacking!");
		}

	}
	protected virtual void Block()
	{
		if (canBlock)
		{
			//display their shield bar
			shieldBar.CrossFadeAlpha(1f, 0.4f, false);

			if (depleteOnHit)
			{
				//player is holding the block button and their shield will break on impact
				isBlocking = true;
			}
			else
			{
				//player has pushed the block button is temporarily invinsible
				startShieldTimer = true;
			}
		}
	}
	protected virtual void UnBlock()
	{
		//if their shield breaks on impact
		if (depleteOnHit)
		{
			//check if their shield is full
			CheckShieldFull();
			//they are no longer blocking because they let go of the block keypress
			isBlocking = false;
		}

	}
	protected virtual void ShieldDepleted()
	{
		// this will get increased in update because canBlock is false
		stats.currentShieldStrength = 0.0f;
		//player can no longer block 
		canBlock = false;
		startShieldTimer = false;

		if(transparencyCor == null) 
		{
			//start the coroutine to let the player know their shield is depleted and they cant use it
			transparencyCor = StartCoroutine(HelperFunctions.TransitionTransparency(shieldBar, 0.1f));
		}
	}
	protected virtual bool GroundCheck()
	{
		return Physics2D.OverlapCircle(col.bounds.center, 0.1f, groundLayers);
	}
	protected virtual void OnCollisionStay2D(Collision2D other)
	{
		if (((1 << other.gameObject.layer) & groundLayers) != 0)
		{
			grounded = true;
		}
	}
	protected virtual void OnCollisionExit2D(Collision2D other)
	{
		if (((1 << other.gameObject.layer) & groundLayers) != 0)
		{
			grounded = false;
		}
	}

}
