using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
//using UniRx;

public enum AttackState
{
	none,
	atk1,
	atk2,
	atk3,
	bladeDance,
	parrying,
	thrusting
}

public partial class PlayerCtrl : AvatarCtrl
{
	public Material material;
	public Text debugT;
	public cakeslice.Outline katanaOutline;
	public TrailRenderer katanaTrail;
	[HideInInspector] public CharState state;
	[HideInInspector] public AttackState atkState;
	[HideInInspector] public PlayerAtkScript playerAtkScript;
	[HideInInspector] public Transform myTrans;
	public Status status;
	public Inventory inventory;
	public SkillStatus skillStatus;

	float Ad;
	float Cr;
	int Hp;
	int Exp;
	float moveSpeed = 4.5f;
	float stunTime = 0f;
	bool isAvoid;
	bool isAttack;
	bool isHit;

	private new void Awake( )
	{
		Init( );
	}

	private void Init( )
	{
		playerAtkScript = GetComponent<PlayerAtkScript>( );

		i_Animator = GetComponent<Animator>( );
		i_Rigidbody = GetComponent<Rigidbody>( );
		i_Animator.speed = 1.5f;
		SaveDefaultScale( );

		state = CharState.idle;
		atkState = AttackState.none;
		myTrans = transform;

		status = new Status( this );
		inventory = new Inventory( );
		skillStatus = new SkillStatus( this );
	}

	private void Start( )
	{
		SetStatus( ); //이전에 Status의 저장된 스탯 불러오기
		StartCoroutine( CoUpdate( ) );
	}

	public void SetStatus( )
	{
		status.SetStatus( );
		SetTrailEffect( 0 );
		Hp = status.Hp;
		Exp = 0;
		SetExp( );
		SetLevel( );
		UpdateStatus( );
		UpdateSkillSetting( );
	}

	public void UpdateStatus( )
	{
		SetHp( );
		Ad = status.Ad;
		Cr = status.Cr;
	}

	public void UpdateSkillSetting( )
	{
		skillStatus.SkillSetting( );
	}

	public void UpdateHp( int value )
	{
		Hp = Mathf.Min( Hp + value, status.Hp );
		SetHp( );
	}

	public void UpdateExp( int value )
	{
		Exp += value;
		SetExp( );
	}

	public void SetExp( )
	{
		while( Exp >= status.NeedExp )
		{
			status.Level++;
			SetLevel( );
			Exp -= status.NeedExp;
			status.LevelUp( );
		}
		PlayerUIMgr.Instance.SetExp( Exp, status.NeedExp );
	}

	public void SetLevel( )
	{
		PlayerUIMgr.Instance.SetLevel( status.Level );
	}

	public void SetHp( )
	{
		PlayerUIMgr.Instance.SetHP( Hp, status.Hp );
	}

	IEnumerator CoUpdate( )
	{
		while( state != CharState.die )
		{
			debugT.text = "state = " + state.ToString( ) + "\natkState = " + atkState.ToString( );
			Move( );
			i_Rigidbody.velocity = new Vector3( 0f, 0f, 0f );
			yield return i_TimeDelta;
		}
	}

	private void Move( )
	{
		if( state == CharState.attack
				|| state == CharState.stun
				|| state == CharState.avoid
				)
		{
			i_Animator.SetFloat( "Speed", 0f );
			return;
		}
		katanaTrail.enabled = false;

		var a = SimpleTouchController.Instance.GetTouchPosition;
		float h = a.x;
		float v = a.y;

		if( h == 0f && v == 0f )
		{
			i_Animator.SetFloat( "Speed", 0f );
			return;
		}
		transform.eulerAngles = new Vector3( transform.eulerAngles.x, Mathf.Atan2( h, v ) * Mathf.Rad2Deg, transform.eulerAngles.z );
		transform.Translate( Vector3.forward * moveSpeed * Time.deltaTime );
		i_Animator.SetFloat( "Speed", 1f );
	}

	public void MoveForward( float speed )
	{
		StartCoroutine( CoMoveForward( speed ) );
	}
	IEnumerator CoMoveForward( float speed )
	{
		float time = 0.4f;
		speed *= 2.25f;
		while( time > 0f )
		{
			transform.position += transform.forward * speed * Time.deltaTime;
			time -= i_TimeDelta;
			yield return i_TimeDelta;
		}
	}

	public void Delay( float time )
	{
		StartCoroutine( CoDelay( time ) );
	}
	IEnumerator CoDelay( float time )
	{
		state = CharState.delay;
		while( time > 0f )
		{
			time -= i_TimeDelta;
			yield return i_TimeDelta;
		}
		if( state == CharState.delay )
			state = CharState.idle;
	}
	public void Hit( float _damage, float time, AttackType _attackType, DamageType _damgeType = DamageType.melee )
	{
		if( Hp <= 0 )
			return;
		if( state == CharState.avoid )
			return;
		else
		{
			if( state == CharState.delay )
			{
				_damage *= 2f;
				_attackType = AttackType.hard;
				time = 1f;
			}
			UpdateHp( Mathf.RoundToInt( -_damage ) );
			EnemyUIMgr.Instance.SetDamageText( transform, _damage, false, false );
			if( state != CharState.hit )
			{
				if( Hp <= 0 )
				{
					StartCoroutine( CoDie( ) );
				}
				else if( _attackType == AttackType.super )
					StartCoroutine( CoStun( time ) );
				else
					StartCoroutine( CoHit( time ) );
			}
			else
			{
				stunTime = stunTime > time ? stunTime : time;
			}
		}
	}

	IEnumerator CoHit( float time )
	{
		yield return null;
	}

	IEnumerator CoStun( float time )
	{
		state = CharState.stun;
		i_Animator.SetTrigger( "Hit" );
		i_Animator.SetBool( "IsHit", true );
		i_Animator.SetBool( "Stunned", true );

		stunTime = time;
		while( stunTime > 0f )
		{
			stunTime -= i_TimeDelta;
			yield return i_TimeDelta;
		}
		i_Animator.SetBool( "IsHit", false );
		i_Animator.SetBool( "Stunned", false );
		state = CharState.idle;
		yield return null;
	}

	IEnumerator CoDie( )
	{
		state = CharState.die;
		i_Animator.SetTrigger( "Death" );
		yield return new WaitForSeconds( 1.0f );
		GameMgr.Instance.GameOver( );
	}
}

public partial class PlayerCtrl : AvatarCtrl
{
	public void SetTrailEffect( int num )
	{
		katanaOutline.enabled = false;
		switch( num )
		{
		case 0:
			katanaTrail.startColor = Color.white; break;
		case 1:
			katanaTrail.startColor = Color.yellow; break;
		case 2:
			katanaTrail.startColor = Color.red;
			katanaOutline.enabled = true;
			break;
		}
	}
}