using UnityEngine;
using System.Collections;

public class Bouncer : Enemy {

	[SerializeField] Vector2 verticalForce;
	[SerializeField] Vector2 horizontalForce;
	[SerializeField] Vector2 velocity;
	[SerializeField] Vector2 maxVelocity;
	[SerializeField] EdgeCollider2D edgeCol;

	public bool grounded;
	private float airTimer;

	protected override void Awake ()
	{
		base.Awake ();
		//lots of health
		stats.health = Random.Range(4,7);
		//strong attack
		stats.attackPower = Random.Range(4,7);
		//slow movement
		stats.movementSpeed = Random.Range(1,3);
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

		type = EnemyType.Bouncer;

		SetSize();
	}
	protected override void Update ()
	{
		base.Update();
	}
	// Update is called once per frame
	protected override void FixedUpdate () 
	{
		Move();

		if (grounded)
		{
			rb.gravityScale = 1.0f;
			Jump();	
		}

		if (velocity.x > maxVelocity.x)
			velocity.x = maxVelocity.x;

		if (velocity.y > maxVelocity.y)
			velocity.y = maxVelocity.y;

		velocity = rb.velocity;
	}
		
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (HelperFunctions.GroundCheck(edgeCol) && transform.position.y > other.transform.position.y)
			grounded = true;

		if (HelperFunctions.WallCheck(col, transform, true))
			rb.AddForce(horizontalForce);
		
		if (HelperFunctions.WallCheck(col, transform, false))
			rb.AddForce(-horizontalForce);

		//if I land on top of the hero
		if (other.collider.CompareTag("Player") && transform.position.y > other.transform.position.y)
		{
			other.gameObject.SendMessage("Damage", stats.attackPower, SendMessageOptions.DontRequireReceiver);
		}
	}
	private void OnCollisionExit2D(Collision2D other)
	{
		grounded = false;
	}
	private void Jump()
	{
		rb.AddForce(verticalForce);
	}
	protected override void Move ()
	{
		base.Move();
	}
	

}
