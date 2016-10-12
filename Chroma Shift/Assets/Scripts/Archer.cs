using UnityEngine;
using System.Collections;

public class Archer : Hero {

	[SerializeField] Transform target;
	[SerializeField] float shotAngle;
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

		tmpProjectile.GetComponent<Rigidbody2D>().velocity = HelperFunctions.ArcTowards(tmpProjectile.transform, target, shotAngle);
	}
	protected override void Block ()
	{
		base.Block ();
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
