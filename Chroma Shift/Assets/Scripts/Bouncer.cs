using UnityEngine;
using System.Collections;

public class Bouncer : Enemy {

	[SerializeField] Vector2 jumpForce;
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
	void Start () 
	{
		SetSize(false);
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
		Move();

		if (grounded)
		{
			rb.gravityScale = 1.0f;
			Jump();	
		}
		else
		{
			airTimer += Time.deltaTime;

			if (airTimer > 3.0f)
			{
				rb.gravityScale = 10.0f;
				airTimer = 0.0f;
			}
		}
			
	}
		
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		
		if (HelperFunctions.GroundCheck(edgeCol) && transform.position.y > other.transform.position.y)
			grounded = true;
		
		//if the hero touches me
		if (other.collider.CompareTag("Player") && transform.position.y < other.transform.position.y)
		{
			other.gameObject.SendMessage("Damage", 1, SendMessageOptions.DontRequireReceiver);
		}
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
		rb.AddForce(jumpForce);
	}
	protected override void Move ()
	{
		base.Move();
	}
	

}
