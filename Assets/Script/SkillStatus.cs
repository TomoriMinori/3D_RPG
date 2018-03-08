using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatus
{
	public int skillPoint;
	public List<Skill_Player> skillList;

	public SkillStatus( PlayerCtrl playerCtrl )
	{
		skillList = new List<Skill_Player>( );
		skillList.Add( new Skill_Player_Attack( playerCtrl, -1 ) );
		skillList.Add( new Skill_Player_Attack_BladeDance( playerCtrl, -1 ) );
		skillList.Add( new Skill_Player_Attack_Parrying( playerCtrl, -1 ) );
		skillList.Add( new Skill_Player_Attack_Thrusting( playerCtrl, -1 ) );
		skillPoint = 100;
	}

	public void SkillSetting( )
	{
		for( int i = 0 ; i < skillList.Count ; ++i )
		{
			if( i == 0 )
			{
				skillList[i].SkillColSetting( );
			}
			else if( skillList[i].skillLevel != -1 )
			{
				skillList[i].SkillColSetting( );
				Debug.Log( skillList[i].titleStr + "의 레벨 : " + skillList[i].skillLevel + 1 );
			}
		}
	}
	public bool SkillLevelUp( int skillId )
	{
		if( GameMgr.Instance.playerCtrl.status.Level < skillList[skillId].NeedLevel( ) )
			return false;
		skillPoint--;
		skillList[skillId].skillLevel++;
		return true;
	}
}