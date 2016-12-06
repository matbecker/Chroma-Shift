using UnityEngine;
using System.Collections;

public class Swordsmen : Hero {

	[SerializeField] GameObject sword;

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
	}

	protected override void Attack ()
	{
		base.Attack ();
		//dont allow the swordsmen to do anything else while hes swinging his giant sword
		disableInput = true;

		PlayAttackAnimation();
		//call the play attack animation over the network
		if (photonView.isMine)
			photonView.RPC("PlayAttackAnimation", PhotonTargets.Others);
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position + boxCol.bounds.extents, Vector2.right, stats.attackRange, HelperFunctions.collidableLayers);

		if (hit.collider != null)
			hit.collider.gameObject.SendMessage("Damage", stats.attackPower, SendMessageOptions.DontRequireReceiver);
	}
	//method for playing the ninjas attack animation
	[PunRPC] private void PlayAttackAnimation()
	{
		anim.SetBool("isAttacking", true);
	}
	//swordsmens block consists of a block animation
	[PunRPC] protected override void Block ()
	{
		base.Block ();
		anim.SetBool("isBlocking", true);
	}
	//swordsmens finished blocking consists of stopping the block animation
	[PunRPC] protected override void FinishedBlocking()
	{
		base.FinishedBlocking();
		anim.SetBool("isBlocking", false);
	}

	protected override void Update ()
	{
		base.Update ();

		if (canBlock)
		{
			sword.SetActive(true);
		}
		else
			sword.SetActive(false);
	}
}
