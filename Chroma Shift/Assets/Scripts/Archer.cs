using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Archer : Hero {

	[SerializeField] GameObject bow;
	[SerializeField] GameObject shield;

	protected override void Start ()
	{
		base.Start ();

		if(photonView.isMine)
			InputManager.Instance.TrackMouseEvent += TrackMouseEvent;
	}
	protected override void OnDestroy ()
	{
		base.OnDestroy ();

		if(photonView.isMine)
			InputManager.Instance.TrackMouseEvent -= TrackMouseEvent;
	}
	//method for the archers attack
	protected override void Attack ()
	{
		Vector2 arrowSpawnPoint;

		//if the hero is facing right spawn the arrow at the right side of their sprite
		if(facingRight)
			arrowSpawnPoint = new Vector3(transform.position.x + edgeCol.bounds.extents.x + projectile.GetComponent<SpriteRenderer>().sprite.bounds.extents.x, transform.position.y, transform.position.z);
		//if the hero is facing left spawn an arrow at the left side of their sprite
		else 
			arrowSpawnPoint = new Vector3(transform.position.x - edgeCol.bounds.extents.x - projectile.GetComponent<SpriteRenderer>().sprite.bounds.extents.x, transform.position.y, transform.position.z);

		//call the arc method to get the velocity for the arrow
		var arrowVelocity = HelperFunctions.Arc(bow.GetComponent<Transform>()) * stats.attackSpeed;

		//negate the velocity if the hero is facing left
		if(!facingRight)
			arrowVelocity.x *= -1;

		//call the shoot arrow method
		ShootArrow(arrowSpawnPoint, arrowVelocity);

		//call the shoot arrow method over the network
		if(photonView.isMine)
			photonView.RPC("ShootArrow", PhotonTargets.Others, arrowSpawnPoint, arrowVelocity);
	}
	//method for shooting an arrow 
	[PunRPC] void ShootArrow(Vector2 spawn, Vector2 velocity)
	{
		//instantiate the arrow
		var arrow = Instantiate(projectile, spawn, Quaternion.identity) as GameObject;
		//set the projectiles velocity
		arrow.GetComponent<Rigidbody2D>().velocity = velocity;
		//get the scale of the arrow
		var scale = arrow.transform.localScale;
		//flip the scale of the arrow if the velocity has been negated 
		arrow.transform.localScale = new Vector3(scale.x * (velocity.x < 0 ? -1 : 1), scale.y, scale.z);

	}

	[PunRPC] protected override void Block ()
	{
		base.Block ();
		//hide the archers bow
		bow.GetComponent<SpriteRenderer>().sortingOrder = 1;
		//make the archers shield appear
		shield.GetComponent<SpriteRenderer>().sortingOrder = 3;
		//move the shield ahead a bit
		shield.transform.Translate(new Vector3(0.1f,0.0f,0.0f));
	}

	[PunRPC] protected override void FinishedBlocking()
	{
		base.FinishedBlocking();
		//make the bow reappear
		bow.GetComponent<SpriteRenderer>().sortingOrder = 3;
		//hide the shield again
		shield.GetComponent<SpriteRenderer>().sortingOrder = 1;
		//move the shield back to its original position
		shield.transform.Translate(new Vector3(-0.1f,0.0f,0.0f));
	}

	private void TrackMouseEvent(float x, float y)
	{
		//track the mouse cursor at the position of the player
		y -= Camera.main.WorldToScreenPoint(transform.position).y;
		//clamp the rotation of the bow between 0 and 90 degrees
		y = Mathf.Clamp(y, 0, 90);
		//rotate the bow zround the z axis based on the mouse position
		bow.transform.localEulerAngles = new Vector3(bow.transform.localRotation.x, bow.transform.localRotation.y, y);
	}

	protected override void Update ()
	{
		base.Update ();
	}
}
