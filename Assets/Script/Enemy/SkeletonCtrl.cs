using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCtrl : EnemyCtrl
{
	bool isAttackCool;
	public AttackColliderScript AttackCol;
	public AttackColliderScript SkillCol;
	public GameObject shoutParticle;
	protected float stressGauge;

	protected override void Awake( )
	{
		base.Awake( );
		attackDist = 1.4f;
		attackAvailableDist = 2.3f;
		traceDist = 5.5f;
		traceStopDist = 1.4f;
		stressGauge = 0f;
	}

	protected override void Start( )
	{
		outlineScript.enabled = false;
		targetTrans = GameMgr.Instance.player.transform;
		StartCoroutine( CheckMonsterState( ) );
		StartCoroutine( MonsterAction( ) );
	}

	protected override IEnumerator CheckMonsterState( )
	{
		while( state != CharState.die )
		{
			DebugState( );
			dist = Vector3.Distance( targetTrans.position, myTrans.position );
			//Debug.Log( dist );
			if( state == CharState.idle )
			{
				if( dist <= attackDist && !isAttackCool )
					state = CharState.attack;
				else if( dist <= traceDist && dist > attackDist )
					state = CharState.trace;
			}
			else if( state == CharState.attack )
			{
				if( dist >= attackAvailableDist && !isAttack )
					state = CharState.idle;
			}
			else if( state == CharState.trace )
			{
				if( dist <= traceStopDist )
					state = CharState.idle;
				else if( dist > traceDist )
					state = CharState.idle;
			}
			yield return i_WaitForSeconds;
		}
	}

	protected override IEnumerator MonsterAction( )
	{
		while( state != CharState.die )
		{
			switch( state )
			{
			case CharState.idle:
				i_NavMeshAgent.isStopped = true;
				break;
			case CharState.trace:
				i_NavMeshAgent.isStopped = false;
				i_NavMeshAgent.destination = targetTrans.position;
				break;
			case CharState.attack:
				i_NavMeshAgent.isStopped = true;
				Attack( );
				break;
			case CharState.die:
				i_NavMeshAgent.isStopped = true;
				break;
			case CharState.avoid:
				if( i_NavMeshAgent.enabled )
					i_NavMeshAgent.isStopped = true;
				break;
			case CharState.super:
				i_NavMeshAgent.isStopped = true;
				break;
			case CharState.away:
				i_NavMeshAgent.isStopped = false;
				Away( );
				break;
			}
			if( cur != state )
				cur = state;
			yield return i_WaitForSeconds;
		}
	}

	public override void AttackAlarm( )
	{
		AttackCol.Play( true );
	}
	public override void AttackJudge( )
	{
		AttackCol.Play( false );
	}

	public override void SkillAlarm( )
	{
		SkillCol.Play( true );
	}
	public override void SkillJudge( )
	{
		SkillCol.Play( false );
		shoutParticle.SetActive( true );
	}

	public override void Attack( )
	{
		if( isAttack )
			return;
		if( isAttackCool )
			return;
		myTrans.LookAt( targetTrans );
		state = CharState.attack;
		isAttack = true;
		isAttackCool = true;
		i_Animator.SetTrigger( "Attack" );
		StartCoroutine( CoAttack( ) );
	}

	public void Shout( )
	{
		state = CharState.super;
		i_Animator.SetTrigger( "Shout" );
		stressGauge = 0f;
		outlineScript.enabled = true;
	}

	public override void AttackEnd( int num = 0 )
	{
		base.AttackEnd( num );
	}
	public override void SkillEnd( int num = 0 )
	{
		base.SkillEnd( num );
		shoutParticle.SetActive( false );
		if( Hp > 20f )
		{
			outlineScript.enabled = false;
		}
	}

	public override IEnumerator CoAttack( )
	{
		float time = 0f;
		while( isAttack && state == CharState.attack )
		{
			time += Time.deltaTime;
			if( time < 0.6f )
			{
				myTrans.LookAt( targetTrans );
				if( dist >= attackAvailableDist )
				{
					state = CharState.idle;
					i_Animator.SetTrigger( "Idle" );
					break;
				}
			}
			yield return null;
		}
		if( !isAttack ) //정상적으로 끝났을 때
		{
			time = 0.5f;
			while( time > 0f )
			{
				time -= Time.deltaTime;
				yield return null;
			}
			isAttackCool = false;
			if( state == CharState.attack )
				state = CharState.idle;
		}
		else if( state != CharState.attack ) //도중 캔슬
		{
			isAttack = false;
			isAttackCool = false;
		}
	}

	public override void Hit( float _damage, float _stunTime, AttackType _atkType, DamageType _dmgType )
	{
		if( Hp <= 0f || state == CharState.super )
			return;
		if( Hp <= 20f )
		{
			outlineScript.enabled = true;
		}
		stressGauge += 40f;
		if( stressGauge >= 100f )
		{
			Shout( );
			return;
		}
		HitDamage( _damage, _stunTime, _atkType );
		if( _atkType == AttackType.super || _atkType == AttackType.hard )
			Hit( );
	}

	protected override void Hit( )
	{
		if( Hp > 0f )
			i_Animator.SetTrigger( "Hit" );
	}

	protected override void Death( )
	{
		base.Death( );
		i_Animator.SetTrigger( "Death" );
	}
}