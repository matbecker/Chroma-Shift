using UnityEngine;
using System.Collections;

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

	protected override void Start ()
	{
		base.Start ();
		InputManager.Instance.Hover += Hover;
		InputManager.Instance.UnAttack += UnAttack;
		shield.GetComponent<SpriteRenderer>().color = Color.clear;
		halfTransparency = new Color(1,1,1,0.5f);
		freezeBlock = false;
	}
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		InputManager.Instance.Hover -= Hover;
		InputManager.Instance.UnAttack -= UnAttack;
	}
	protected override void Update()
	{
		base.Update();
		//if the player is hovering start timing their hover
		if (isHovering)
			hoverTimer += Time.deltaTime;

		//if the timer reaches the hover duration
		if (hoverTimer >= desiredHoverDuration)
		{
			//call the StopHovering method
			StopHovering();
			//reset the timer
			hoverTimer = 0.0f;
		}
		if (isChargingShot)
		{
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

	}
	protected override void Attack ()
	{
		base.Attack ();

		isChargingShot = true;
		//Freeze the y position of the projectile so it doesnt fall
		tmpProjectile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
	}
	private void UnAttack()
	{
		//give the projectile a velocity once the attack button is released
		tmpProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.attackSpeed, 0.0f);

		isChargingShot = false;
	}
	protected override void Block ()
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
	protected override void OnCollisionStay2D (Collision2D other)
	{
		base.OnCollisionStay2D (other);
		//player is on the ground and is not hovering
		isHovering = false;

		canHover = false;
	}
	protected override void OnCollisionExit2D (Collision2D other)
	{
		base.OnCollisionExit2D (other);

		canHover = true;
	}
	private void Hover()
	{
		//if the player can hover freeze their y position so they begin hovering
		if (canHover)
			rb.constraints = RigidbodyConstraints2D.FreezePositionY;
		//player is now hovering 
		isHovering = true;
	}
	private void StopHovering()
	{
		//unfreeze the players position
		rb.constraints = ~RigidbodyConstraints2D.FreezePositionY & 
						 ~RigidbodyConstraints2D.FreezePositionX;
		//player is no longer hovering
		isHovering = false;
		//and no cannot hover again
		canHover = false;
	}
}
