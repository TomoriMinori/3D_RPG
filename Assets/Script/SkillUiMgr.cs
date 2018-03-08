using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUiMgr : MonoBehaviour
{
	public GameObject SkillList;
	public SkillUiBlock[] skillUiBlockArr;
	[Space]
	public Text explainT;
	public Text effectT;
	public Text nextEffectT;
	[Space]
	public Text skillPointT;
	public GameObject slotPannel;
	int selectSkillId;
	SkillStatus skillStatus;

	private void Awake( )
	{
		//SkillList.GetComponentsInChildren<SkillUiBlock>( false );
	}
	private void OnEnable( )
	{
		skillStatus = GameMgr.Instance.playerCtrl.skillStatus;
		for( int i = 0 ; i < skillUiBlockArr.Length ; ++i )
		{
			skillUiBlockArr[i].Show( );
		}
		UpdateSkillUi( );
		slotPannel.SetActive( false );
	}
	public void UpdateSkillUi( )
	{
		for( int i = 0 ; i < skillUiBlockArr.Length ; ++i )
		{
			skillUiBlockArr[i].UpdateBlock( );
		}
		skillPointT.text = skillStatus.skillPoint.ToString( );
		explainT.text = "-";
		effectT.text = "-";
		nextEffectT.text = "-";
	}
	public void SkillExplain( int skillId )
	{
		var a = skillStatus.skillList[skillId];
		explainT.text = a.explainStr;
		if( a.skillLevel > -1 )
			effectT.text = a.EffectStr( a.skillLevel );
		else
			effectT.text = "-";
		if( a.skillLevel < a.maxSkillLevel )
			nextEffectT.text = a.EffectStr( a.skillLevel + 1 );
		else
			nextEffectT.text = "-";
	}
	public void SkillUpgrade( int skillId )
	{
		if( skillStatus.skillPoint > 0 &&
			skillStatus.SkillLevelUp( skillId ) )
		{
			UpdateSkillUi( );
		}
	}
	public void OpenSlot( int skillId )
	{
		selectSkillId = skillId;
		slotPannel.SetActive( true );
	}
	public void CloseSlot( )
	{
		slotPannel.SetActive( false );
	}
	public void SetSlot( int slotNum )
	{
		PlayerUIMgr.Instance.SetSkill( slotNum, selectSkillId );
		CloseSlot( );
	}
}