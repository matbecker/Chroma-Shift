using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Archer : Hero {

	[SerializeField] GameObject bow;
	[SerializeField] GameObject shield;

	protected override void Start ()
	{
		base.Start ();
		InputManager.Instance.TrackMouseEvent += TrackMouseEvent;
	}
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		InputManager.Instance.TrackMouseEvent -= TrackMouseEvent;
	}
	protected override void Attack ()
	{
		base.Attack ();

		tmpProjectile.GetComponent<Rigidbody2D>().velocity = HelperFunctions.Arc(bow.GetComponent<Transform>(), facingRight) * stats.attackSpeed;
	}
	protected override void Block ()
	{
		base.Block ();
		bow.GetComponent<SpriteRenderer>().sortingOrder = 1;
		shield.GetComponent<SpriteRenderer>().sortingOrder = 3;
		shield.transform.Translate(new Vector3(0.1f,0.0f,0.0f));
	}
	protected override void UnBlock()
	{
		base.UnBlock();
		bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
		shield.GetComponent<SpriteRenderer>().sortingOrder = 1;
		shield.transform.Translate(new Vector3(-0.1f,0.0f,0.0f));
	}
	private void TrackMouseEvent(float x, float y)
	{
		//track the mouse cursor at the position of the player
		y -= Camera.main.WorldToScreenPoint(transform.position).y;
		//clamp the rotation of the bow between 0 and 90 degrees
		y = Mathf.Clamp(y, 0, 90);
		bow.transform.localEulerAngles = new Vector3(bow.transform.localRotation.x, bow.transform.localRotation.y, y);
	}
	protected override void Update ()
	{
		base.Update ();
	}
}
