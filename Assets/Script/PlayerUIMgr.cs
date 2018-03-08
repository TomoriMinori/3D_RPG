using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ConfigState
{
	none,
	list,
	inven,
	stat,
	skill,
	quest,
	home
}

public class PlayerUIMgr : MonoBehaviour
{
	public ConfigState state;
	public static PlayerUIMgr Instance;

	public Camera UICamera;
	public Slider HpBar;
	public Text HpT;
	public Slider ExpBar;
	public Text ExpT;
	public Text levelT;
	[Space]
	public PlayerButton[] skillBtns;
	public Sprite[] attackIcons;
	[Space]
	public GameObject configBtn;
	public GameObject configPannel;
	public GameObject listPannel;
	public GameObject invenPannel;
	public GameObject statPannel;
	public GameObject skillPannel;
	public GameObject questPannel;
	public GameObject homePannel;
	public Image volumeImage;
	public Sprite[] volumeSprite;
	int volumeCount = 0;

	public void Awake( )
	{
		Instance = this;
	}

	public void SetHP( float count, float maxCount )
	{
		HpT.text = count.ToString( ) + " / " + maxCount.ToString( );
		HpBar.value = count / maxCount;
	}

	public void SetExp( float count, float maxCount )
	{
		ExpT.text = count.ToString( ) + " / " + maxCount.ToString( );
		ExpBar.value = count / maxCount;
	}

	public void SetLevel( int level )
	{
		levelT.text = level.ToString( );
	}

	public void SetSkill( int slotNum, int skillId )
	{
		var skill = GameMgr.Instance.playerCtrl.skillStatus.skillList[skillId];
		for( int i = 0 ; i < skillBtns.Length ; ++i )
		{
			if( skillBtns[i].skillScript == skill )
				skillBtns[i].SetSkill( null );
		}
		skillBtns[slotNum].SetSkill( skill );
	}

	public void Config( )
	{
		GameMgr.Instance.GamePause( );
		state = ConfigState.list;
		configPannel.SetActive( true );
		listPannel.SetActive( true );

		invenPannel.SetActive( false );
		statPannel.SetActive( false );
		skillPannel.SetActive( false );
		questPannel.SetActive( false );
		homePannel.SetActive( false );
	}

	public void Inven( )
	{
		state = ConfigState.inven;
		listPannel.SetActive( false );
		invenPannel.SetActive( true );
	}

	public void Stat( )
	{
		state = ConfigState.stat;
		listPannel.SetActive( false );
		statPannel.SetActive( true );
	}

	public void Skill( )
	{
		state = ConfigState.skill;
		listPannel.SetActive( false );
		skillPannel.SetActive( true );
	}
	public void Quest( )
	{
		state = ConfigState.quest;
		listPannel.SetActive( false );
		questPannel.SetActive( true );
	}

	public void Home( )
	{
		state = ConfigState.home;
		listPannel.SetActive( false );
		homePannel.SetActive( true );
	}

	public void Volume( )
	{
		volumeCount++;
		volumeCount %= volumeSprite.Length;
		volumeImage.sprite = volumeSprite[volumeCount];
	}

	public void Back( )
	{
		switch( state )
		{
		case ConfigState.list:
			GameMgr.Instance.GameResume( );
			GameMgr.Instance.playerCtrl.UpdateSkillSetting( ); //끄면 스킬 데미지 설정
			listPannel.SetActive( false );
			configPannel.SetActive( false );
			break;
		case ConfigState.inven:
			Config( );
			break;
		case ConfigState.stat:
			Config( );
			break;
		case ConfigState.skill:
			Config( );
			break;
		case ConfigState.quest:
			Config( );
			break;
		case ConfigState.home:
			Config( );
			break;
		}

	}
}