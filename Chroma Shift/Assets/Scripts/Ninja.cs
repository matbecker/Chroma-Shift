using UnityEngine;
using System.Collections;

public class Ninja : Hero {

	[SerializeField] private bool canDoubleJump;
	[SerializeField] GameObject dagger;
	[SerializeField] SpriteRenderer headband;
	[SerializeField] float invisibleDuration;
	[SerializeField] float lerpDuration;
	[SerializeField] BoxCollider2D swordTrigger;
	private float startLerpTimer;
	private float endLerpTimer;
	private bool freezeBlock;


	protected override void Start ()
	{	
		base.Start ();

		if (photonView.isMine)
			InputManager.Instance.DoubleJump += DoubleJump;

		//ninja cannot see his shield timer
		shieldBar.enabled = false;
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();

		if (InputManager.Instance)
			if (photonView.isMine)
				InputManager.Instance.DoubleJump -= DoubleJump;
	}
	

	protected override void Update ()
	{
		base.Update ();

		if (startShieldTimer)
		{
			//make the ninja sprite dissapear
			sprite.color = Color.Lerp(sprite.color, Color.clear, startLerpTimer);
			//make the headband sprite dissapear
			headband.color = Color.Lerp(headband.color, Color.clear, startLerpTimer);
			//make the dagger dissapear
			dagger.SetActive(false);

			startLerpTimer += Time.deltaTime / lerpDuration;
		}
		else
		{
			freezeBlock = false;
			//make the ninja sprite reappear
			sprite.color = Color.Lerp(Color.clear, colour.GetCurrentColor(), endLerpTimer);
			//make the headband sprite reappear
			headband.color = Color.Lerp(Color.clear, Color.black, endLerpTimer);
			//make the dagger reappear
			dagger.SetActive(true);

			endLerpTimer += Time.deltaTime / lerpDuration;
		}
		if (isAttacking)
			swordTrigger.enabled = true;
		else
			swordTrigger.enabled = false;
	}

	protected override void Attack ()
	{
		base.Attack ();
		disableInput = true;

		PlayAttackAnimation();
		//call the play attack animation over the network
		if (photonView.isMine)
			photonView.RPC("PlayAttackAnimation", PhotonTargets.Others);
	}
	//method for playing the attack animation
	[PunRPC] private void PlayAttackAnimation()
	{
		anim.SetBool("isAttacking", true);
	}

	[PunRPC] protected override void Block ()
	{
		base.Block ();

		//dont allow the player to reset the LerpTimer if they are currently blocking
		if (!freezeBlock && canBlock)
		{
			startLerpTimer = 0.0f;
			endLerpTimer = 0.0f;
		}
		freezeBlock = true;
	}
	protected override void OnCollisionExit2D (Collision2D other)
	{
		base.OnCollisionExit2D (other);
		//if the player has left the ground layer they can double jump
		canDoubleJump = true;
	}
	protected override void OnCollisionStay2D(Collision2D other)
	{
		base.OnCollisionStay2D(other);
		//if the player is on the ground layer they cannot double jump
		canDoubleJump = false;
	}
	private void DoubleJump()
	{
		//if the player can double jump
		if (canDoubleJump)
		{
			//apply a force in the y direction once again
			rb.AddForce(stats.jumpForce);
			//set the canDoubleJump variable to false to ensure the player cannot jump a third time
			canDoubleJump = false;
		}
	}
}
