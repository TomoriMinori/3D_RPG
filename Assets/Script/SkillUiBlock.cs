using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUiBlock : MonoBehaviour
{
	public SkillUiMgr skillUiMgr;
	public int skillId;
	public Image iconImage;
	public Text titleT;
	public Text levelT;
	public Text needCondiT;

	public GameObject upgradeBtn;
	public GameObject slotBtn;

	[HideInInspector] public Skill_Player skillScript;

	public void Show( )
	{
		skillScript = GameMgr.Instance.playerCtrl.skillStatus.skillList[skillId];
		iconImage.sprite = PlayerUIMgr.Instance.attackIcons[skillScript.iconNum];
		titleT.text = skillScript.titleStr;
	}
	public void UpdateBlock( )
	{
		slotBtn.SetActive( skillScript.skillLevel >= 0 && !skillScript.passive );
		upgradeBtn.SetActive( skillScript.skillLevel != skillScript.maxSkillLevel );

		levelT.text = ( skillScript.skillLevel + 1 ).ToString( );
		if( skillScript.NeedLevel( ) != -1 )
			needCondiT.text = skillScript.NeedLevel( ).ToString( );
		else
			needCondiT.text = "-";
	}
	public void Explain( )
	{
		skillUiMgr.SkillExplain( skillId );
	}
	public void Slot( )
	{
		skillUiMgr.OpenSlot( skillId );
	}
	public void Upgrade( )
	{
		skillUiMgr.SkillUpgrade( skillId );
	}
}