using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

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
	protected float distance;

	protected virtual void Awake()
	{
		target = GameObject.FindGameObjectWithTag("Player");
	}
	// Use this for initialization
	void Start () 
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		col = gameObject.GetComponent<BoxCollider2D>();
		sprite = gameObject.GetComponent<SpriteRenderer>();
		colour = gameObject.GetComponent<ColourManager>();
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
		SetSize(false);
	}
	protected virtual void Move()
	{
		direction = target.transform.position - transform.position;
		direction.Normalize();
		direction *= stats.movementSpeed;
		rb.velocity = new Vector2(direction.x, rb.velocity.y);
	}
	protected virtual void SetSize(bool buzzer)
	{
		if (buzzer)
			gameObject.transform.localScale = new Vector3(1.0f,0.25f,1.0f);
		else
		{
			//size differences based on health
			if (stats.health <= 2)
				gameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			else if (stats.health <= 4 && stats.health > 2)
				gameObject.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
			else if (stats.health <= 6 && stats.health > 4)
				gameObject.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
		}

	}
}
