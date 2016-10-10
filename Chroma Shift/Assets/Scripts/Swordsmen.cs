using UnityEngine;
using System.Collections;

public class Swordsmen : Hero {

	protected override void Start ()
	{
		base.Start ();
	}
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
	}
	protected override void Jump ()
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
	protected override void OnCollisionStay2D (Collision2D other)
	{
		base.OnCollisionStay2D (other);
	}
	protected override void OnCollisionExit2D (Collision2D other)
	{
		base.OnCollisionExit2D (other);
	}
	protected override void UnBlock()
	{
		base.UnBlock();
	}
	protected override void Update ()
	{
		base.Update ();
	}
}
