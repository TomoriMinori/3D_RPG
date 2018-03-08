using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUiMgr : MonoBehaviour
{
	PlayerCtrl playerCtrl;

	public Text strongT;
	public Text agilityT;
	public Text healthT;
	public Text luckT;

	public Text strong2T;
	public Text agility2T;
	public Text health2T;
	public Text luck2T;

	[Space]
	public Text AdT;
	public Text HpT;
	public Text Cr;
	public Text unusedT;

	float curHp;

	public void OnEnable( )
	{
		Show( );
	}

	public void Show( )
	{
		playerCtrl = GameMgr.Instance.playerCtrl;
		var a = playerCtrl.status;

		strongT.text = a.Strong.ToString( );
		agilityT.text = a.Agility.ToString( );
		healthT.text = a.Health.ToString( );
		luckT.text = a.Luck.ToString( );

		strong2T.text = a.plusStatus.PlusStrong.ToString( );
		agility2T.text = a.plusStatus.PlusAgility.ToString( );
		health2T.text = a.plusStatus.PlusHealth.ToString( );
		luck2T.text = a.plusStatus.PlusLuck.ToString( );

		AdT.text = a.Ad.ToString( );
		HpT.text = a.Hp.ToString( );
		Cr.text = a.Cr.ToString( );
		unusedT.text = a.unusedStat.ToString( );
	}

	public void PlusStat( int num )
	{
		int curHp = playerCtrl.status.Hp;
		Debug.Log( curHp );
		var a = playerCtrl.status;
		if( a.unusedStat <= 0 )
			return;
		switch( num )
		{
		case 0:
			a.Strong += 1; break;
		case 1:
			a.Agility += 1; break;
		case 2:
			a.Health += 1; break;
		case 3:
			a.Luck += 1; break;
		}
		a.unusedStat -= 1;

		playerCtrl.status.SetStatus( );

		Show( );
		if( curHp != playerCtrl.status.Hp )
			playerCtrl.UpdateHp( playerCtrl.status.Hp - curHp );
		GameMgr.Instance.playerCtrl.UpdateStatus( );
	}
}
