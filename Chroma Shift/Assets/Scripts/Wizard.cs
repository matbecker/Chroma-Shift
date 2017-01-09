using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wizard : Hero {


	[SerializeField] private bool canHover;
	[SerializeField] private bool isHovering;
	[SerializeField] private float hoverTimer;
	[SerializeField] float desiredHoverDuration;
	[SerializeField] private bool isChargingShot;
	[SerializeField] private float chargingShotTimer;
	[SerializeField] GameObject shield;
	[SerializeField] float lerpDuration;
	private Color halfTransparency;
	private float startLerpTimer;
	private float endLerpTimer;
	private bool freezeBlock;
	private Vector3 rotation;


	protected override void Start ()
	{
		base.Start ();

		if(photonView.isMine)
		{
			InputManager.Instance.Hover += Hover;
			InputManager.Instance.UnAttack += UnAttack;
		}

		shield.GetComponent<SpriteRenderer>().color = Color.clear;
		halfTransparency = new Color(1,1,1,0.5f);
		freezeBlock = false;

	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();

		if (InputManager.Instance)
		{
			if(photonView.isMine)
			{
				InputManager.Instance.Hover -= Hover;
				InputManager.Instance.UnAttack -= UnAttack;
			}
		}
	}

	protected override void Update()
	{
		base.Update();

		//if the player is hovering start timing their hover
		if (isHovering)
			hoverTimer += Time.deltaTime;
		else
			hoverTimer = 0.0f;

		//if the timer reaches the hover duration
		if (hoverTimer >= desiredHoverDuration)
		{
			//call the StopHovering method
			StopHovering();
		}
		if (isChargingShot)
		{
			if (!tmpProjectile)
				return;
			
			chargingShotTimer += Time.deltaTime;

			//if the attackbutton has been held for under 1 second
			if (chargingShotTimer < 1.0f)
			{
				//increase attack power
				stats.attackPower = 1;

				//expand the size of the projectile
				tmpProjectile.GetComponent<Transform>().localScale = new Vector3(1.2f,1.2f,1.2f);
			}
			//if the attack button has been held bwteen 1 and 2 seconds
			else if (chargingShotTimer > 1.0f && chargingShotTimer < 2.0f)
			{
				//increase attack power
				stats.attackPower = 2;

				//expand the size of the projectile
				tmpProjectile.GetComponent<Transform>().localScale = new Vector3(1.4f,1.4f,1.4f);
			}
			//if the attack button has been held between 2 and 3 seconds
			else if (chargingShotTimer > 2.0f && chargingShotTimer < 3.0f)
			{
				//increase attack power
				stats.attackPower = 3;

				//expand the size of the projectile
				tmpProjectile.GetComponent<Transform>().localScale = new Vector3(1.6f,1.6f,1.6f);
			}
			//if the attack button has been held for over 3 seconds
			else if (chargingShotTimer > 3.0f)
			{
				//increase attack power
				stats.attackPower = 4;

				//expand the size of the projectile
				tmpProjectile.GetComponent<Transform>().localScale = new Vector3(1.8f,1.8f,1.8f);
			}
		}
		else
		{
			//reset the charge timer
			chargingShotTimer = 0.0f;
		}


		if (startShieldTimer)
		{
			//make the shield bubble appear
			shield.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, halfTransparency, startLerpTimer);

			startLerpTimer += Time.deltaTime / lerpDuration;
		}
		else
		{
			freezeBlock = false;
			//make the shield bubble dissapear
			shield.GetComponent<SpriteRenderer>().color = Color.Lerp(halfTransparency, Color.clear, endLerpTimer);

			endLerpTimer += Time.deltaTime / lerpDuration;
		}
		rotation = transform.rotation.eulerAngles;
	}

	protected override void Attack ()
	{
		base.Attack ();

		Vector2 spawnProjectilePoint;

		//if the wizard is facing right spawn a projectile at the right side of their sprite
		if(facingRight)
			spawnProjectilePoint = new Vector3(transform.position.x + edgeCol.bounds.extents.x + projectile.GetComponent<SpriteRenderer>().sprite.bounds.extents.x, transform.position.y, transform.position.z);
		//if the player is facing left spawn a projectile at the left side of their sprite
		else 
			spawnProjectilePoint = new Vector3(transform.position.x - edgeCol.bounds.extents.x - projectile.GetComponent<SpriteRenderer>().sprite.bounds.extents.x, transform.position.y, transform.position.z);


		StartChargeShot(spawnProjectilePoint);

		//call teh start charge shot method over the network
		if(photonView.isMine)
			photonView.RPC("StartChargeShot", PhotonTargets.Others, spawnProjectilePoint);
	}
	//method for spawning the wizards projectile
	[PunRPC] void StartChargeShot(Vector2 spawnProjectilePoint)
	{
		//spawn the projectile
		tmpProjectile = Instantiate(projectile, spawnProjectilePoint, Quaternion.identity) as GameObject;
		//Freeze the y position of the projectile so it doesnt fall
		tmpProjectile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
		//wizard is now charging their shot
		isChargingShot = true;
	}
	//method for shooting the wizards projectile
	[PunRPC] void ShootChargeShot(Vector2 velocity)
	{
		//set the projectiles velocity
		tmpProjectile.GetComponent<Rigidbody2D>().velocity = velocity;
		//wizard is no longer charging their shot
		isChargingShot = false;
	}

	private void UnAttack()
	{
		//give the projectile a velocity in the x direction once the attack button is released
		var velocity = new Vector2(stats.attackSpeed, 0.0f);
		//negate the velocity if the player is facing left
		if(!facingRight) 
			velocity.x *= -1;
		
		ShootChargeShot(velocity);
		//call the shoot charge shot method over the network
		if(photonView.isMine)
			photonView.RPC("ShootChargeShot", PhotonTargets.Others, velocity);
	}

	[PunRPC] protected override void Block ()
	{
		base.Block ();

		//dont allow the player to reset the LerpTimer if they are currently blocking
		if (!freezeBlock)
		{
			startLerpTimer = 0.0f;
			endLerpTimer = 0.0f;
		}
		freezeBlock = true;
	}
	protected override void OnCollisionEnter2D (Collision2D other)
	{
		base.OnCollisionEnter2D (other);

		if (HelperFunctions.GroundCheck(edgeCol))
		{
			isHovering = false;
			canHover = false;
		}
	}

	private void OnCollisionExit2D (Collision2D other)
	{
		//player is off the ground and can hover if they wish
		if(!grounded && photonView.isMine)
			canHover = true;
	}

	private void Hover()
	{
		//if the player can hover freeze their y position so they begin hovering
		if (canHover)
		{
			rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

			//player is now hovering 
			isHovering = true;
		}
			
		

	}

	private void StopHovering()
	{
		//unfreeze the players position
		rb.constraints = RigidbodyConstraints2D.None;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		//player is no longer hovering
		isHovering = false;
		//and cannot hover again
		canHover = false;
	}
}
