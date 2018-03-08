using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEditor.Events;

public class RobotCtrl : EnemyCtrl
{
	Skill_Enemy_Humanoid_AttackPunch skill_attackPunch;
	Skill_Enemy_Humanoid_AttackKick skill_attackKick;
	Skill_Enemy_Humanoid_Avoid skill_avoid;

	protected override void Awake( )
	{
		base.Awake( );
		stunAvailable = true;
		avoidAvailable = true;
		skill_attackPunch = new Skill_Enemy_Humanoid_AttackPunch( this, 0f );
		skill_attackKick = new Skill_Enemy_Humanoid_AttackKick( this, 2f );
		skill_avoid = new Skill_Enemy_Humanoid_Avoid( this, 5f );
	}

	protected override void Start( )
	{
		base.Start( );
		state = CharState.idle;
	}

	protected override void LateUpdate( )
	{
		i_Animator.SetFloat( "Velocity X", i_NavMeshAgent.velocity.sqrMagnitude );
		i_Animator.SetFloat( "Velocity Z", i_NavMeshAgent.velocity.sqrMagnitude );
		base.LateUpdate( );
	}

	public override void Attack( )
	{
		if( isAttack )
			return;
		var ran = Random.Range(0f,1f);
		if( ran < 0.5f && skill_attackPunch.Able( ) )
			skill_attackPunch.Play( );
		else if( ran < 1.0f && skill_attackKick.Able( ) )
			skill_attackKick.Play( );
	}

	public override void Avoid( )
	{
		if( !avoidAvailable || isAvoid )
			return;
		if( skill_avoid.Able( ) )
		{
			float ran = Random.Range(0f, 1f);
			if( ran < avoidPer )
			{
				state = CharState.avoid;
				skill_avoid.Play( );
			}
			else
			{
				state = CharState.idle;
			}
		}
	}

	protected override void Hit( )
	{
		i_Animator.SetTrigger( "GetHit1Trigger" );
	}

	protected override void Death( )
	{
		base.Death( );
		i_Animator.SetTrigger( "Death1Trigger" );
	}
}