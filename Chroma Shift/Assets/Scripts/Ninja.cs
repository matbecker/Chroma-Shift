using UnityEngine;
using System.Collections;

public class Ninja : Hero {

	[SerializeField] private bool canDoubleJump;
	// Use this for initialization
	protected override void Start ()
	{
		InputManager.Instance.DoubleJump += DoubleJump;
		base.Start ();
	}
	protected override void OnDestroy ()
	{
		InputManager.Instance.DoubleJump -= DoubleJump;
		base.OnDestroy ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	protected override void Jump()
	{
		base.Jump ();
	}
	protected override void Run (float horizontalAxis)
	{
		base.Run (horizontalAxis);
	}
	protected override void Attack ()
	{
		base.Attack ();
	}
	protected override void Block ()
	{
		base.Block ();
	}
	protected override bool GroundCheck ()
	{
		return base.GroundCheck ();
	}
	protected override void OnCollisionExit2D (Collision2D other)
	{
		base.OnCollisionExit2D (other);
		//if the player has left the ground layer they can double jump
		canDoubleJump = true;
	}
	protected override void OnCollisionStay2D(Collision2D other)
	{
		base.OnCollisionStay2D(other);
		//if the player is on the ground layer they cannot double jump
		canDoubleJump = false;
	}
	private void DoubleJump()
	{
		//if the player can double jump
		if (canDoubleJump)
		{
			//apply a force in the y direction once again
			rb.AddForce(stats.jumpForce);
			//set the canDoubleJump variable to false to ensure the player cannot jump a third time
			canDoubleJump = false;
		}
	}
}
