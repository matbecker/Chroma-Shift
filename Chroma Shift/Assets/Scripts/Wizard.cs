using UnityEngine;
using System.Collections;

public class Wizard : Hero {


	[SerializeField] private bool canHover;
	[SerializeField] private bool isHovering;
	[SerializeField] private float hoverTimer;
	[SerializeField] float desiredHoverDuration;

	protected override void Start ()
	{
		base.Start ();
		InputManager.Instance.Hover += Hover;
	}
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		InputManager.Instance.Hover -= Hover;
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
			
	}
	protected override void Attack ()
	{
		base.Attack ();

		tmpProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.attackSpeed, 0.0f);

		tmpProjectile.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
	}
	protected override void Block ()
	{
		base.Block ();
		isInvinsible = true;
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
