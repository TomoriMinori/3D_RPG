using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
	normal,
	elite,
	boss
}

public class EnemyUIMgr : MonoBehaviour
{
	public static EnemyUIMgr Instance;
	Vector2 resolution;
	List<EnemyUI> UIList;
	List<DamageText> DTList;

	void Awake( )
	{
		Instance = this;
		resolution = GameMgr.Instance.gameResolution;

		UIList = new List<EnemyUI>( );
		var uis = gameObject.GetComponentsInChildren<EnemyUI>(true);
		for( int i = 0 ; i < uis.Length ; ++i )
			UIList.Add( uis[i] );
		DTList = new List<DamageText>( );
		var dts = gameObject.GetComponentsInChildren<DamageText>(true);
		for( int i = 0 ; i < dts.Length ; ++i )
			DTList.Add( dts[i] );
	}

	public EnemyUI SetHp( string name, EnemyType type, Transform target, float per )
	{
		for( int i = 0 ; i < UIList.Count ; ++i )
		{
			if( !UIList[i].isUse )
			{
				UIList[i].SetTargetTrans( name, type, target, per );
				return UIList[i];
			}
		}
		Debug.LogError( "UI 개수가 모자름. 현재 개수 : " + UIList.Count );
		return null;
	}

	public void SetDamageText( Transform _target, float _damage, bool _cri, bool _isMon )
	{
		for( int i = 0 ; i < DTList.Count ; ++i )
		{
			if( !DTList[i].isActiveAndEnabled )
			{
				DTList[i].gameObject.SetActive( true );
				DTList[i].ShowText( _target, _damage, _cri, _isMon );
				break;
			}
		}
	}
}