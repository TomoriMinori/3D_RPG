using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkColliderScript : MonoBehaviour
{
	public EnemyCtrl enemyCtrl;
	public AttackType type;
	public bool alarm;
	public float damage;
	public float stuckTime;
	public int curCount = 0;
	public int count = 1;
	float playTime = 0.06f;

	public int Count
	{
		get { return count; }
	}

	public void Play( bool isAlram )
	{
		StopAllCoroutines( );
		alarm = isAlram;
		curCount = count;
		gameObject.SetActive( true );
		StartCoroutine( CoPlay( ) );
	}

	IEnumerator CoPlay( )
	{
		yield return new WaitForSeconds( playTime );
		gameObject.SetActive( false );
	}

	private void OnTriggerEnter( Collider other )
	{
		if( other.tag == "Player" )
		{
			var a = other.gameObject.GetComponent<PlayerCtrl>( );
			if( !alarm )
			{
				if( a.state == CharState.guard )
				{
					enemyCtrl.Hit( 100f, 2f, AttackType.hard, DamageType.melee );
				}
				else
					a.Hit( damage, stuckTime, type );
			}
			curCount -= 1;
			if( curCount == 0 )
				gameObject.SetActive( false );
		}
	}
}