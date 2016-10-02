using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	[System.Serializable]
	public class Stats
	{
		public int health;
		public int attackPower;
		public int defence;
		public float currentShieldStrngth;
		public float shieldCapacity;
		public float attackRange;
		public float attackSpeed;
		public float maxVelocity;
		public Vector2 jumpForce;
		public Vector2 movementForce;

	}

	[SerializeField] Stats stats;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] EdgeCollider2D col;
	[SerializeField] LayerMask groundLayer;
	// Use this for initialization
	void Start () 
	{
		//register jump event
		InputManager.Instance.Jump += Jump;
		//register run event
		InputManager.Instance.Run += Run;
		//register attack event
		InputManager.Instance.Attack += Attack;
		//register Block event
		InputManager.Instance.Block += Block;
		//get the rigidbody of the gameobject
		rb = gameObject.GetComponent<Rigidbody2D>();
		//get the edge collider of the gameobject
		col = gameObject.GetComponent<EdgeCollider2D>();
		//ignore all layers but the ground layer
		groundLayer = 1 << LayerMask.NameToLayer("Ground");

	}
	void OnDestroy () 
	{
		//unregister jump event
		InputManager.Instance.Jump -= Jump;
		//unregister run event
		InputManager.Instance.Run -= Run;
		//unregister attack event
		InputManager.Instance.Attack -= Attack;
		//unregister Block event
		InputManager.Instance.Block -= Block;
	}

	
	// Update is called once per frame
	void Update () 
	{
		Vector3 down = transform.TransformDirection(Vector3.down) / 3;
		Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), down, Color.black);

//		if (GroundCheck())
//			Debug.Log("grounded!");
	}
	protected virtual void Jump()
	{
		if (GroundCheck())
			rb.AddForce(stats.jumpForce);
	}
	protected virtual void Run(float horizontalAxis)
	{
		if (horizontalAxis > 0)
		{
			rb.AddForce(stats.movementForce);

			//limit player velocity in right direction
			if (rb.velocity.x > stats.maxVelocity)
				rb.velocity = new Vector2(stats.maxVelocity, rb.velocity.y);
		}
		if (horizontalAxis < 0)
		{
			rb.AddForce(-stats.movementForce);

			//limit players velocity in left direction
			if (rb.velocity.x < -stats.maxVelocity) 
				rb.velocity = new Vector2(-stats.maxVelocity, rb.velocity.y);
		}
	}
	protected virtual void Attack()
	{
		Debug.Log("Attacking!");
	}
	protected virtual void Block()
	{
		Debug.Log("Blocking!");
	}
	protected virtual bool GroundCheck()
	{
		return Physics2D.OverlapCircle(col.bounds.center, 0.1f, groundLayer);
	}

}
