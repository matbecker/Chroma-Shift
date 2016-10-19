using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Rigidbody2D rb;
	[SerializeField] Vector2 force;
	[SerializeField] Vector2 acceleration;
	[SerializeField] Vector2 intialVelocity;
	private bool alive;

	// Use this for initialization
	void Start () 
	{
		alive = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if (alive)
//		{
//			acceleration = force / rb.mass;
//
//			rb.position = intialVelocity + acceleration * Time.deltaTime;
//
//			rb.position += intialVelocity * Time.deltaTime + (0.5f * acceleration) * Mathf.Pow(Time.deltaTime, 2);
//
//			intialVelocity = rb.velocity;
//		}
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & HelperFunctions.collidableLayers) != 0)
		{
			alive = false;

			Destroy(gameObject);
		}
	}
}
