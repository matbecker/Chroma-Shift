using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public enum EnemyType {Buzzer, Bomber, Bouncer };
	public EnemyType type;

	[System.Serializable]
	public class Stats
	{
		public int health;
		public int attackPower;
		public int movementSpeed;
	}
	[SerializeField] GameObject[] powerUps;
	[SerializeField] protected Stats stats;
	[SerializeField] protected Rigidbody2D rb;
	[SerializeField] protected BoxCollider2D col;
	[SerializeField] protected SpriteRenderer sprite;
	[SerializeField] protected ColourManager colour;
	[SerializeField] protected GameObject target;
	protected Vector2 direction;
	[SerializeField] protected float distance;

	protected virtual void Awake()
	{
		target = GameObject.FindGameObjectWithTag("Player");
	}
	// Use this for initialization
	protected virtual void Start () 
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		col = gameObject.GetComponent<BoxCollider2D>();
		sprite = gameObject.GetComponent<SpriteRenderer>();
		colour = gameObject.GetComponent<ColourManager>();

		int rand = Random.Range(0,2);
		int randShade = Random.Range(0,6);

		if (rand == 0)
		{
			var topColour = colour.colors[(int)ColourWheel.Instance.currentColourTop].colors[randShade];
			topColour.a = 1;
			sprite.color = topColour;
		}
		else
		{
			var bottomColour = colour.colors[(int)ColourWheel.Instance.currentColourBottom].colors[randShade];
			bottomColour.a = 1;
			sprite.color = bottomColour;
		}
			
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if (transform.position.y < LevelManager.Instance.levelBottom)
			Death();

		distance = direction.magnitude;

		if (distance > 20)
			Death();

	}
	protected virtual void FixedUpdate(){}

	protected virtual void Death()
	{
		int rand = Random.Range(0,5);

		if (rand == 0)
		{
			GameObject powerUp = Instantiate(powerUps[0], col.bounds.center, Quaternion.identity) as GameObject;
		}
		EnemySpawner.enemyWave.Remove(gameObject);
		Destroy(gameObject);
	}
	protected virtual void Damage(int damageAmount)
	{
		stats.health -= damageAmount;

		if (stats.health <= 0)
		{
			Death();
			return;
		}
		SetSize();
	}
	protected virtual void Move()
	{
		direction = target.transform.position - transform.position;
		direction.Normalize();
		direction *= stats.movementSpeed;
		rb.velocity = new Vector2(direction.x, rb.velocity.y);
	}
	protected virtual void SetSize()
	{
		switch (type)
		{
		case EnemyType.Bomber:
			gameObject.transform.localScale = new Vector3(1.0f,0.75f,1.0f);
			break;
		case EnemyType.Bouncer:
			if (stats.health <= 2)
				gameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			else if (stats.health <= 4 && stats.health > 2)
				gameObject.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
			else if (stats.health <= 6 && stats.health > 4)
				gameObject.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
			break;
		case EnemyType.Buzzer:
			gameObject.transform.localScale = new Vector3(1.0f,0.25f,1.0f);
			break;
		default:
			break;
		}
			//size differences based on health
			

	}
}
