using UnityEngine;
using System.Collections;

public class Bomber : Enemy {

	protected override void Awake()
	{
		base.Awake();
		//low health
		stats.health = 1;
		//medium attack
		stats.attackPower = Random.Range(1,4);
		//fast movement
		stats.movementSpeed = Random.Range(5,7);
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

		type = EnemyType.Bomber;

		SetSize();
	}
	protected override void Update()
	{
		base.Update ();
	}	
	protected override void FixedUpdate()
	{
		Move();
	}
	protected override void Move()
	{
		base.Move();
	}
	protected override void OnCollisionEnter2D(Collision2D other)
	{
		if(!CheckExtraDamage(other))
			if (other.collider.CompareTag("Player"))
				other.gameObject.SendMessage("Damage", stats.attackPower, SendMessageOptions.DontRequireReceiver);

		Death();
	}
}
