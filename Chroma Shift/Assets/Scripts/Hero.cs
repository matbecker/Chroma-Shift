using UnityEngine;
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
		//Debug.DrawRay(transform.position + trig.bounds.extents, Vector2.right * stats.attackRange);
		if (!depleteOnHit)
		{
			if (startShieldTimer)
			{
				Debug.Log(stats.currentShieldStrength);
				stats.currentShieldStrength -= Time.deltaTime;
			}
			//if (!canBlock)
			//StartCoroutine(HelperFunctions.TransitionTransparency(shieldBar, 0.1f));
		}
		else
		{
			if (!isBlocking && stats.currentShieldStrength < stats.shieldCapacity)
			{
				stats.currentShieldStrength += Time.deltaTime;

				CheckShieldFull();
			}
				
		}
		if (stats.currentShieldStrength <= 0.0f)
		{
			ShieldDepleted();
		}
		if (!canBlock)
		{
			stats.currentShieldStrength += Time.deltaTime;

			CheckShieldFull();
		}
		shieldBar.rectTransform.localScale = new Vector2(stats.currentShieldStrength / stats.shieldCapacity, 1.0f);

	}
	private void CheckShieldFull()
	{
		if (stats.currentShieldStrength >= stats.shieldCapacity)
		{
			//StopAllCoroutines();
			if(transparencyCor != null)
			{
				StopCoroutine(transparencyCor);
				transparencyCor = null;
			}

			stats.currentShieldStrength = stats.shieldCapacity;

			shieldBar.CrossFadeAlpha(0.01f, 0.4f, false);

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
			shieldBar.CrossFadeAlpha(1f, 0.4f, false);

			if (depleteOnHit)
			{
				//player is holding the block button and their shield will break on impact
				isBlocking = true;
			}
			else
			{
				startShieldTimer = true;
				//player has pushed the block button is temporarily invinsible
			}
		}
	}
	protected virtual void UnBlock()
	{
		if (depleteOnHit)
		{
			isBlocking = false;
		}

	}
	protected virtual void ShieldDepleted()
	{
		// this will get increased in update because canBlock is false
		stats.currentShieldStrength = 0.0f;
		canBlock = false;
		startShieldTimer = false;

		if(transparencyCor == null) 
		{
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
