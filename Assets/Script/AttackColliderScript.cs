using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderScript : MonoBehaviour
{
	public bool isObstacle;
	public bool infinityCount;
	public float damage;
	public AttackType attackType;
	public DamageType damageType;

	public bool alarm;
	public float stuckTime;
	public float criPer;

	public int curCount = 0;
	public int count = 1;
	public float playTime = 0.06f;
	public float attackDelayTime = 0f;

	PlayerCtrl playerCtrl;
	List<EnemyCtrl> enemyList;

	private void Awake( )
	{
		enemyList = new List<EnemyCtrl>( );
	}

	public void ColSetting( float _damage, float _criPer, AttackType _attackType, DamageType _damageType, float _stunTime = 0f, int _count = 10, float _playTime = 0.03f, float _attackDelayTime = 1f )
	{
		damage = _damage;
		criPer = _criPer;
		attackType = _attackType;
		damageType = _damageType;
		stuckTime = _stunTime;
		count = _count;
		playTime = _playTime;
		attackDelayTime = _attackDelayTime;
	}

	public void Play( bool isAlram )
	{
		StopAllCoroutines( );
		alarm = isAlram;
		curCount = count;
		gameObject.SetActive( true );
		StartCoroutine( CoPlay( ) );
	}
	public void ResetCol( EnemyCtrl enemyCtrl )
	{
		Debug.Log( "EnemReset" );
		enemyList.Remove( enemyCtrl );
	}
	public void ResetCol( )
	{
		playerCtrl = null;
	}
	IEnumerator CoPlay( )
	{
		enemyList.Clear( );
		playerCtrl = null;
		yield return new WaitForSeconds( playTime );
		gameObject.SetActive( false );
	}

	IEnumerator CoResetCol( )
	{
		while( gameObject.activeSelf )
		{
			yield return null;
		}
	}
	private void OnTriggerStay( Collider other )
	{
		if( curCount == 0 && !infinityCount )
		{
			return;
		}
		if( other.tag == "Enemy" )
		{
			var a = other.gameObject.GetComponent<EnemyCtrl>( );
			if( enemyList.Contains( a ) )
				return;
			enemyList.Add( a );
			AttackDelay atd = new AttackDelay( this, a, attackDelayTime );
			if( alarm )
				a.Avoid( );
			else
				a.Hit( damage, stuckTime, attackType, damageType );
			curCount -= 1;
		}
		else if( other.tag == "Player" )
		{
			var a = other.gameObject.GetComponent<PlayerCtrl>( );
			if( playerCtrl == a )
				return;
			playerCtrl = a;
			AttackDelay atd = new AttackDelay(this,a,attackDelayTime);
			if( alarm )
			{
			}
			else
				a.Hit( damage, stuckTime, attackType, damageType );
			curCount -= 1;
		}
	}
}

public class AttackDelay
{
	public AttackColliderScript atkColScript;

	public EnemyCtrl enemyCtrl;
	public PlayerCtrl playerCtrl;

	public AttackDelay( AttackColliderScript _atkColScript, PlayerCtrl _playerCtrl, float _time )
	{
		atkColScript = _atkColScript;
		playerCtrl = _playerCtrl;
		atkColScript.StartCoroutine( CoCoolTime( _time ) );
	}
	public AttackDelay( AttackColliderScript _atkColScript, EnemyCtrl _enemyCtrl, float _time )
	{
		atkColScript = _atkColScript;
		enemyCtrl = _enemyCtrl;
		atkColScript.StartCoroutine( CoCoolTime( _time ) );
	}
	IEnumerator CoCoolTime( float time )
	{
		yield return new WaitForSeconds( time );
		if( atkColScript != null )
		{
			if( enemyCtrl != null )
			{
				atkColScript.ResetCol( enemyCtrl );
			}
			else if( playerCtrl != null )
			{
				atkColScript.ResetCol( );
			}
		}
	}
}


public enum AttackType
{
	normalPunch,
	normalKick,
	hardPunch,
	hardKick,

	normal,
	hard,
	guardBreak,
	super,
	range
}
public enum DamageType
{
	range,
	melee,
	floorSpace
}