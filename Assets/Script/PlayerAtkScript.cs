using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkScript : MonoBehaviour
{
	public AttackColliderScript[] atk;
	public float delay = 0.03f;
	float maxDelay = 0.5f;

	public void AtkColSetting( )
	{
		for( int i = 0 ; i < atk.Length ; ++i )
		{

		}
	}

	public void AtkAlarm( int num )
	{
		atk[num].Play( true );
	}

	public void Atk( int num )
	{
		atk[num].Play( false );
	}
}