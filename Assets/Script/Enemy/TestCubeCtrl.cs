using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeCtrl : EnemyCtrl
{
	protected override void Awake( )
	{
		base.Awake( );
		stunAvailable = true;
		avoidAvailable = true;
	}

	protected override void Start( )
	{
		base.Start( );
		state = CharState.idle;
	}

	protected override void LateUpdate( )
	{
		//Debug.Log( rb.collisionDetectionMode );
		//Debug.Log( rb.detectCollisions );
		//Debug.Log( rb.sleepThreshold );
		//Debug.Log( "///" );
		//animator.SetFloat( "Velocity X", agent.velocity.sqrMagnitude );
		//animator.SetFloat( "Velocity Z", agent.velocity.sqrMagnitude );
		base.LateUpdate( );
	}

	public override void Attack( )
	{
	}

	public override void Avoid( )
	{
		if( !avoidAvailable || isAvoid )
			return;
	}

	protected override void Hit( )
	{
		//animator.SetTrigger( "GetHit1Trigger" );
	}

	protected override void Death( )
	{
		base.Death( );
		//animator.SetTrigger( "Death1Trigger" );
	}
	private void OnTriggerStay( Collider other )
	{
		Debug.Log( other.tag );
	}
}