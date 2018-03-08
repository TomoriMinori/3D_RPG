using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
	public EnemyCtrl[] monster;
	public GameObject[] walls;
	public UnityEngine.UI.Text clear;

	void Start( )
	{
		StartCoroutine( CoUpdate( ) );
	}
	IEnumerator CoUpdate( )
	{
		while( !GameMgr.Instance.player.activeSelf )
			yield return null;
		while( true )
		{
			for( int i = 0 ; i < monster.Length ; ++i )
			{
				if( monster[i].state == CharState.die )
					WallOpen( i );
			}
			yield return null;
		}
	}

	public void WallOpen( int num )
	{
		walls[num].SetActive( false );
		if( num == walls.Length - 1 )
			clear.gameObject.SetActive( true );
	}
}