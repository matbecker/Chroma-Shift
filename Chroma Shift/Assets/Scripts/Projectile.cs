using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public enum ProjectileType { Arrow, Magic };
	public ProjectileType type;

	public Rigidbody2D rb;
	[SerializeField] Vector2 force;
	[SerializeField] Vector2 acceleration;
	[SerializeField] Vector2 intialVelocity;
	[SerializeField] GameObject hero;
	private bool alive;

	// Use this for initialization
	void Start () 
	{
		alive = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (alive)
			hero = GameObject.FindGameObjectWithTag("Player");
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			switch (type)
			{
			case ProjectileType.Arrow:
				other.SendMessage("Damage", hero.GetComponent<Hero>().stats.attackPower, SendMessageOptions.DontRequireReceiver);
				Destroy(gameObject);
				break;
			case ProjectileType.Magic:
				other.SendMessage("Damage", hero.GetComponent<Hero>().stats.attackPower, SendMessageOptions.DontRequireReceiver);
				Destroy(gameObject);
				break;
			default:
				break;
			}
			Debug.Log(hero.GetComponent<Hero>().stats.attackPower);
		}
		if (((1 << other.gameObject.layer) & HelperFunctions.collidableLayers) != 0)
		{
			Destroy(gameObject);
		}
	}
}
