using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharState
{
	die = 1 << 0,

	idle = 1 << 1,
	trace = 1 << 2,
	attack = 1 << 3,
	avoid = 1 << 4,
	hit = 1 << 5,
	stun = 1 << 6,

	guard = 1 << 7,
	heal = 1 << 8,
	super = 1 << 9,

	away = 1 << 10,
	delay = 1 << 11,
	emersion = 1 << 12,
}

public abstract class EnemyCtrl : InGameObject
{
	public string charName = "";
	public EnemyType enemyType;
	public float Hp = 100f;
	public float maxHp = 100f;
	public int exp = 6;
	public Transform leftPos;
	public cakeslice.Outline outlineScript;

	protected EnemyUI enemyUI;


	public CharState state;
	protected CharState cur;
	protected CharState debugCur;
	[HideInInspector] public Transform myTrans;
	[HideInInspector] public Transform targetTrans;
	[HideInInspector] public float avoidPer = 0.3f;

	protected float dist;

	protected float awayStopDist = 10f;
	protected float awayPer = 0.9f;
	protected float updateTime = 0.04f;
	protected float stunTime = 0f;

	protected float attackDist = 1.3f;
	protected float attackAvailableDist = 1.5f;
	protected float traceDist = 7.0f;
	protected float traceStopDist = 1.2f;

	public float moveSpeed = 1.0f;

	protected float attackPlaytime = 2f;

	[HideInInspector]protected bool avoidAvailable;
	[HideInInspector]protected bool stunAvailable;

	[HideInInspector]public bool isAway;
	[HideInInspector]public bool isThreat;
	[HideInInspector]public bool isAttack;
	[HideInInspector]public bool isAvoid;

	protected delegate void SkillList( );

	protected virtual new void Awake( )
	{
		i_NavMeshAgent = GetComponent<NavMeshAgent>( );
		i_NavMeshAgent.speed = moveSpeed;
		i_Animator = GetComponent<Animator>( );
		i_Rigidbody = GetComponent<Rigidbody>( );
		SaveDefaultScale( );
		enemyUI = null;
		state = CharState.idle;
		myTrans = transform;
	}

	protected virtual void Start( )
	{
		outlineScript.enabled = false;
		targetTrans = GameMgr.Instance.player.transform;
		StartCoroutine( CheckMonsterState( ) );
		StartCoroutine( MonsterAction( ) );
	}

	protected virtual IEnumerator CheckMonsterState( )
	{
		while( state != CharState.die )
		{
			//DebugState( );
			dist = Vector3.Distance( targetTrans.position, myTrans.position );
			if( state == CharState.idle )
			{
				//if( Hp / maxHp <= awayPer && dist > traceStopDist )
				//	state = CharState.away;
				if( dist <= attackDist )
				{
					state = CharState.attack;
				}
				//else if( dist > traceStopDist )
				//{
				//	state = CharState.trace;
				//}
				else if( dist < traceDist )
				{
					state = CharState.trace;
				}
				else
					Debug.Log( "예외" );
			}
			else if( state == CharState.attack )
			{
				if( dist > attackAvailableDist && !isAttack )
				{
					state = CharState.idle;
				}
			}
			else if( state == CharState.trace )
			{
				if( dist <= traceStopDist )
					state = CharState.idle;
			}
			yield return new WaitForSeconds( 0.02f );
		}
	}

	public virtual void AttackAlarm( )
	{ }
	public virtual void AttackJudge( )
	{ }

	public virtual void SkillAlarm( )
	{ }
	public virtual void SkillJudge( )
	{ }

	protected virtual void DebugState( )
	{
		if( debugCur != state )
			Debug.Log( debugCur + " TO " + state );
		debugCur = state;
	}

	protected virtual IEnumerator MonsterAction( )
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
			case CharState.away:
				i_NavMeshAgent.isStopped = false;
				Away( );
				break;
			}
			yield return null;
			//yield return new WaitForSeconds( 0.05f );
		}
	}

	protected virtual void LateUpdate( )
	{
		if( i_Animator == null )
			return;
		if( i_NavMeshAgent.velocity.magnitude > 0 )
		{
			i_Animator.SetBool( "Moving", true );
		}
		else
		{
			i_Animator.SetBool( "Moving", false );
		}
	}

	public virtual void Away( )
	{
		if( isAway )
			return;
		isAway = true;
		transform.rotation = Quaternion.LookRotation( myTrans.position - targetTrans.position );
		Vector3 runTo = transform.position + transform.forward * 10f;
		NavMeshHit hit;
		NavMesh.SamplePosition( runTo, out hit, 5, 1 );
		i_NavMeshAgent.destination = runTo;
	}

	public virtual void Attack( )
	{
		if( isAttack )
			return;
	}

	public virtual void AttackEnd( int num = 0 )
	{
		Debug.Log( "atk end" );
		isAttack = false;
	}
	public virtual void SkillEnd( int num = 0 )
	{
		Debug.Log( "skill end" );
		state = CharState.idle;
	}

	public virtual IEnumerator CoAttack( )
	{
		yield return null;
	}

	public virtual void Avoid( )
	{
		if( !avoidAvailable || isAvoid )
			return;
	}

	public virtual void Hit( float _damage, float _stunTime, AttackType _atkType, DamageType _dmgType )
	{
		if( Hp <= 0f )
			return;
		else if( Hp <= 30f )
		{
			outlineScript.enabled = true;
		}
		if( state == CharState.avoid && _atkType != AttackType.super )
			return;
		if( _atkType == AttackType.hard )
			Hit( );
		HitDamage( _damage, _stunTime, _atkType );
	}

	protected abstract void Hit( );

	protected virtual void Death( )
	{
		GameMgr.Instance.playerCtrl.UpdateExp( exp );
	}

	public virtual void HitDamage( float _damage, float _stunTime, AttackType _type )
	{
		Hp -= _damage;
		if( enemyUI == null )
			enemyUI = EnemyUIMgr.Instance.SetHp( charName, enemyType, myTrans, Hp / maxHp );
		else
			enemyUI.SetHpSlider( Hp / maxHp );

		EnemyUIMgr.Instance.SetDamageText( transform, _damage, false, true );

		if( Hp <= 0f )
		{
			outlineScript.enabled = false;
			state = CharState.die;
			enemyUI = null;
			Death( );
		}
		else
		{
			if( state != CharState.hit )
				StartCoroutine( CoHit( _stunTime ) );
			else
			{
				stunTime = stunTime > _stunTime ? stunTime : _stunTime;
			}
		}
	}

	protected virtual IEnumerator CoHit( float _stunTime )
	{
		stunTime = _stunTime;
		state = CharState.hit;
		while( _stunTime > 0f && stunAvailable && i_Animator != null )
		{
			i_Animator.SetBool( "Stunned", true );
			state = CharState.stun;
			_stunTime -= updateTime;
			yield return new WaitForSeconds( updateTime );
		}
		if( stunAvailable && i_Animator != null )
			i_Animator.SetBool( "Stunned", false );
		state = CharState.idle;
	}
}