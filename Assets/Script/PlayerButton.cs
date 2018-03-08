using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerButton : MonoBehaviour
{
	public bool attackBtn;
	[HideInInspector] public Skill_Player skillScript;
	public Image skillImage;
	public Image fillImage;

	public bool IsClick;
	float delay = 0.1f;
	bool isDelay;

	public void Start( )
	{
		if( attackBtn )
			SetSkill( GameMgr.Instance.playerCtrl.skillStatus.skillList[0] );
	}

	public void SetSkill( Skill_Player skillScript )
	{
		if( skillScript == null )
		{
			StopAllCoroutines( ); //coplaying 마지막에서 호출할때 대비용
			skillImage.sprite = PlayerUIMgr.Instance.attackIcons[0];
			this.skillScript = null;
			SetFillAmount( 0f );
			return;
		}
		this.skillScript = skillScript;
		skillScript.buttonScript = this;
		SetFillAmount( 1f );
		skillImage.sprite = PlayerUIMgr.Instance.attackIcons[skillScript.iconNum];
		if( skillScript.isFinish )
			SetFillAmount( Time.time, skillScript.coolTime );
	}

	public void SetFillAmount( float per )
	{
		fillImage.fillAmount = per;
	}
	public void SetFillAmount( float time, float maxTime )
	{
		//Debug.Log( Time.time );
		//Debug.Log( skillScript.coolTime );
		if( time >= maxTime )
			fillImage.fillAmount = 0f;
		else
			StartCoroutine( CoSetFillAmount( maxTime - time ) );
	}
	IEnumerator CoSetFillAmount( float time )
	{
		float cTime = time;
		while( cTime > 0f && skillScript != null )
		{
			cTime -= Time.deltaTime;
			fillImage.fillAmount = cTime / time;
			yield return null;
		}
		fillImage.fillAmount = 0f;
	}
	public void SetSkillIcon(int iconId)
	{
		skillImage.sprite = PlayerUIMgr.Instance.attackIcons[iconId];
	}
	public void SetAttack( Sprite sprite, float time, float limitTime = 0f )
	{
		skillImage.sprite = sprite;
		SetFillAmountAttack( time, limitTime );
	}
	public void SetFillAmountAttack( float time, float limitTime )
	{
		StartCoroutine( CoSetFillAmountAttack( time, limitTime ) );
	}
	IEnumerator CoSetFillAmountAttack( float time, float limitTime )
	{
		AttackState ats = GameMgr.Instance.player.GetComponent<PlayerCtrl>( ).atkState;
		float cTime = time;
		while( cTime > 0f )
		{
			fillImage.fillAmount = cTime / time;
			cTime -= Time.deltaTime;
			yield return null;
		}
		fillImage.fillAmount = 0f;

		if( limitTime != 0f )
		{
			cTime = limitTime;
			while( cTime > 0f )
			{
				if( IsClick )
					break;
				else if( ats != GameMgr.Instance.player.GetComponent<PlayerCtrl>( ).atkState )
					break;
				cTime -= Time.deltaTime;
				yield return null;
			}
			if( cTime <= 0f )
				skillScript.SetDefault( );
		}
	}

	public void ButtonDown( )
	{
		if( skillScript == null )
			return;
		if( fillImage.fillAmount == 0f && !isDelay && skillScript.Able( ) )
		{
			IsClick = true;
			isDelay = true;
			skillScript.Play( );
			StartCoroutine( CoDelay( ) );
		}
	}

	IEnumerator CoDelay( )
	{
		yield return new WaitForEndOfFrame( );
		isDelay = false;
		IsClick = false;
	}
}