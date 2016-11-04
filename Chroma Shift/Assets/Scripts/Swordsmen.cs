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
	protected override void Attack ()
	{
		base.Attack ();
		disableInput = true;
	}
	protected override void Block ()
	{
		base.Block ();
		anim.SetBool("isBlocking", true);
	}
	protected override void UnBlock()
	{
		base.UnBlock();
		anim.SetBool("isBlocking", false);
	}
	protected override void Update ()
	{
		base.Update ();
	}
}
