using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public abstract class Skill_Player
{
	public PlayerButton buttonScript;
	public PlayerCtrl charCtrl;
	public CharState SkillType;
	public AttackState AttackState;
	public AttackType attackType;

	public int id;
	public int iconNum;
	public int skillLevel = 0;
	public int maxSkillLevel = 2;
	public int[] needLevel;
	public bool passive;
	public string titleStr;
	public string explainStr;

	public float playTime;
	public float coolTime;

	public float[] Ad;
	public float[] perAd;
	public float[] perTargetHp;
	public float[] maxCoolTime;

	public bool isCancel;
	public bool isFinish;

	public Skill_Player( PlayerCtrl _charCtrl )
	{
		charCtrl = _charCtrl;
		isFinish = true;
		isCancel = false;
		skillLevel = -1;
	}

	public virtual void SkillColSetting( )
	{
	}

	public virtual void ResetCoolTime( )
	{
		isFinish = true;
		isCancel = false;
		coolTime = maxCoolTime[skillLevel] + Time.time;
	}
	public virtual string EffectStr( int level )
	{
		return null;
	}
	public int NeedLevel( )
	{
		if( skillLevel != maxSkillLevel )
			return needLevel[skillLevel + 1];
		else
			return -1;
	}
	public virtual void SetDefault( )
	{
	}

	public virtual IEnumerator CoPlayingCheck( )
	{
		isFinish = false;
		coolTime = Time.time + maxCoolTime[skillLevel] + playTime;
		buttonScript.SetFillAmount( 1f );
		float time = playTime;
		while( time > 0f && !isCancel )
		{
			if( charCtrl.state != SkillType )
				break;
			if( charCtrl.atkState != AttackState )
				break;
			time -= Time.deltaTime;
			yield return null;
		}
		if( charCtrl.state == SkillType )
			charCtrl.state = CharState.idle;
		if( charCtrl.atkState == AttackState )
			charCtrl.atkState = AttackState.none;
		isFinish = true;
		buttonScript.SetFillAmount( Time.time, coolTime );
	}

	public virtual bool Able( )
	{
		if( Time.time > coolTime && isFinish )
		{
			isFinish = true;
			isCancel = false;
			return true;
		}
		else
		{
			return false;
		}
	}
	public virtual void Play( )
	{
		charCtrl.StartCoroutine( CoPlay( ) );
	}
	protected abstract IEnumerator CoPlay( );
	public abstract void SkillCallback( PlayerCtrl _charCtrl, object _obj );
}



public class Skill_Player_Attack : Skill_Player
{
	int attackState = 0;
	public Skill_Player_Attack( PlayerCtrl _charCtrl, int skillLevel ) : base( _charCtrl )
	{
		SkillType = CharState.attack;
		iconNum = 1;
		passive = true;

		id = 0;
		titleStr = "집중의 3연타";
		explainStr = "세번째 기본 공격이 추가됩니다.";

		perAd = new float[] { 2.0f, 2.5f, 3.0f };
		needLevel = new int[] { 1, 1, 1 };

		//원래는 슬롯배정 이후에 해줘야하는 것.
		//buttonScript = PlayerUIMgr.Instance.skillBtns[0];


		playTime = 0.7f;

		atk1Delay = 0.25f;
		atk2Delay = 0.25f;
		atk3Delay = 0.7f;

		atkDefaultDelay = 0.7f;
		atk2Limit = atk1Delay + 0.35f;
		atk3Limit = atk2Delay + 0.35f;
	}
	public override void SkillColSetting( )
	{
		charCtrl.playerAtkScript.atk[0].ColSetting( charCtrl.status.Ad, charCtrl.status.Cr, AttackType.normal, DamageType.melee );
		if( skillLevel > -1 )
			charCtrl.playerAtkScript.atk[1].ColSetting( charCtrl.status.Ad * perAd[skillLevel], charCtrl.status.Cr, AttackType.hard, DamageType.melee );
		if( skillLevel == 2 )
			charCtrl.playerAtkScript.atk[1].ColSetting( charCtrl.status.Ad * perAd[skillLevel], charCtrl.status.Cr, AttackType.hard, DamageType.melee, 1f );
	}

	public override string EffectStr( int level )
	{
		StringBuilder sb = new StringBuilder( );
		sb.Append( "피해량 : " );
		sb.Append( perAd[level] );
		sb.Append( " * 공격력 " );
		if( level == 2 )
		{
			sb.Append( "\n피격대상 1초간 스턴" );
		}
		return sb.ToString( );
	}

	public override void SkillCallback( PlayerCtrl _charCtrl, object _obj )
	{
	}

	float atk1Delay,atk2Delay,atk3Delay;
	float atk2Limit,atk3Limit;
	float atkDefaultDelay;

	public override void SetDefault( )
	{
		switch( charCtrl.atkState )
		{
		case AttackState.atk1:
			charCtrl.atkState = AttackState.none;
			buttonScript.SetSkillIcon( iconNum );
			break;
		case AttackState.atk2:
			charCtrl.atkState = AttackState.none;
			buttonScript.SetSkillIcon( iconNum );
			if( skillLevel > 0 )
				buttonScript.SetFillAmount( 0f, atkDefaultDelay );
			break;
		case AttackState.atk3:
			charCtrl.atkState = AttackState.none;
			buttonScript.SetSkillIcon( iconNum );
			break;
		}
		if( charCtrl.state == CharState.attack )
		{
			charCtrl.state = CharState.idle;
		}
	}

	public override bool Able( )
	{
		if( charCtrl.state == CharState.stun )
			return false;
		if( charCtrl.atkState == AttackState.atk3 ||
			charCtrl.atkState == AttackState.bladeDance ||
			charCtrl.atkState == AttackState.parrying ||
			charCtrl.atkState == AttackState.thrusting )
			return false;
		if( charCtrl.atkState == AttackState.atk2 && skillLevel < 0 )
		{
			return false;
		}
		return true;
	}
	bool isCombo;
	float comboTime;

	IEnumerator CoComboCheck( float time )
	{
		isCombo = true;
		comboTime = time;
		while( comboTime > 0f && isCombo )
		{
			comboTime -= Time.deltaTime;
			yield return null;
		}
		isCombo = false;
		SetDefault( );
	}
	protected override IEnumerator CoPlay( )
	{
		charCtrl.katanaTrail.enabled = true;
		charCtrl.state = CharState.attack;

		switch( charCtrl.atkState )
		{
		case AttackState.none:
			charCtrl.atkState = AttackState.atk1;
			charCtrl.i_Animator.SetTrigger( "AttackA" );
			buttonScript.SetSkillIcon( iconNum + 1 );
			buttonScript.SetFillAmount( 0f, atk1Delay );
			charCtrl.StartCoroutine( CoComboCheck( atk1Delay + atk2Limit ) );
			break;
		case AttackState.atk1:
			charCtrl.atkState = AttackState.atk2;
			charCtrl.i_Animator.SetTrigger( "AttackB" );
			if( skillLevel < 0 )
			{
				comboTime += atk2Delay;
				buttonScript.SetSkillIcon( iconNum );
				buttonScript.SetFillAmount( 0f, atkDefaultDelay );
			}
			else
			{
				comboTime = atk2Delay + atk3Limit;
				buttonScript.SetSkillIcon( iconNum + 2 );
				buttonScript.SetFillAmount( 0f, atk2Delay );
			}
			break;
		case AttackState.atk2:
			comboTime += atk3Delay;
			charCtrl.atkState = AttackState.atk3;
			charCtrl.i_Animator.SetTrigger( "AttackC" );
			buttonScript.SetSkillIcon( iconNum );
			buttonScript.SetFillAmount( 0f, atk3Delay );
			break;
		}
		yield return null;
	}
}


public class Skill_Player_Attack_BladeDance : Skill_Player
{
	int attackState = 0;
	public Skill_Player_Attack_BladeDance( PlayerCtrl _charCtrl, int skillLevel ) : base( _charCtrl )
	{
		SkillType = CharState.attack;
		AttackState = AttackState.bladeDance;
		playTime = 1.9f;
		id = 1;
		titleStr = "검무";
		explainStr = "화려한 검무를 추며 적에게 4연속 공격을 가합니다.";

		perAd = new float[] { 6.0f, 9.0f, 12.0f };
		maxCoolTime = new float[] { 20f, 18f, 15f };
		needLevel = new int[] { 1, 1, 2 };

		//buttonScript = PlayerUIMgr.Instance.skillBtns[1];
		iconNum = 4;
	}

	public override string EffectStr( int level )
	{
		StringBuilder sb = new StringBuilder( );
		sb.Append( "총 피해량 : " );
		sb.Append( perAd[level] );
		sb.Append( " * 공격력 " );
		sb.Append( "\n쿨타임 : " );
		sb.Append( maxCoolTime[level] );
		sb.Append( "초" );
		return sb.ToString( );
	}

	public override void SkillColSetting( )
	{
		charCtrl.playerAtkScript.atk[3].ColSetting( charCtrl.status.Ad * perAd[skillLevel] * 0.15f, charCtrl.status.Cr, AttackType.normal, DamageType.melee );
		charCtrl.playerAtkScript.atk[4].ColSetting( charCtrl.status.Ad * perAd[skillLevel] * 0.25f, charCtrl.status.Cr, AttackType.normal, DamageType.melee );
		charCtrl.playerAtkScript.atk[5].ColSetting( charCtrl.status.Ad * perAd[skillLevel] * 0.25f, charCtrl.status.Cr, AttackType.normal, DamageType.melee );
		charCtrl.playerAtkScript.atk[6].ColSetting( charCtrl.status.Ad * perAd[skillLevel] * 0.35f, charCtrl.status.Cr, AttackType.normal, DamageType.melee );
	}

	public override void SkillCallback( PlayerCtrl _charCtrl, object _obj )
	{
	}

	public override bool Able( )
	{
		if( charCtrl.state == CharState.stun )
			return false;
		if( charCtrl.atkState != AttackState.none )
			return false;
		return true;
	}
	protected override IEnumerator CoPlay( )
	{
		charCtrl.katanaTrail.enabled = true;
		charCtrl.state = CharState.attack;
		charCtrl.atkState = AttackState.bladeDance;
		charCtrl.i_Animator.SetTrigger( "SkillA" );

		yield return charCtrl.StartCoroutine( CoPlayingCheck( ) );
		//임시. 애니메이션이 끝날때 atkState를 none으로 바꾸게끔 해야함.
	}
}

public class Skill_Player_Attack_Parrying : Skill_Player
{
	int attackState = 0;
	public Skill_Player_Attack_Parrying( PlayerCtrl _charCtrl, int skillLevel ) : base( _charCtrl )
	{
		SkillType = CharState.guard;
		AttackState = AttackState.parrying;
		attackType = AttackType.super;

		playTime = 0.4f;
		id = 2;
		titleStr = "패링";
		explainStr = "적의 근접공격을 튕겨내고 혼신의 일격을 가합니다.\n실패할 경우 1초간 무방비 상태가 됩니다.";
		perAd = new float[] { 7.0f, 8.0f, 9.9f };
		maxCoolTime = new float[] { 24f, 20f, 15f };
		needLevel = new int[] { 1, 1, 2 };

		//buttonScript = PlayerUIMgr.Instance.skillBtns[2];
		iconNum = 5;
	}
	public override string EffectStr( int level )
	{
		StringBuilder sb = new StringBuilder( );
		sb.Append( "피해량 : " );
		sb.Append( perAd[level] );
		sb.Append( " * 공격력 " );
		sb.Append( "\n쿨타임 : " );
		sb.Append( maxCoolTime[level] );
		sb.Append( "초" );
		return sb.ToString( );
	}
	public override void SkillColSetting( )
	{
		//charCtrl.playerAtkScript.atk[2].ColSetting( charCtrl.status.Ad * perAd[skillLevel], charCtrl.status.Cr, AttackType.super, DamageType.melee );
	}
	public override void SkillCallback( PlayerCtrl _charCtrl, object _obj )
	{
	}

	public override bool Able( )
	{
		if( charCtrl.state == CharState.stun )
			return false;
		if( charCtrl.atkState != AttackState.none )
			return false;
		return true;
	}
	protected override IEnumerator CoPlay( )
	{
		charCtrl.katanaTrail.enabled = true;
		charCtrl.state = SkillType;
		charCtrl.atkState = AttackState;
		charCtrl.i_Animator.SetTrigger( "SkillB" );
		yield return charCtrl.StartCoroutine( CoPlayingCheck( ) );

		//if( charCtrl.atkState == AttackState.parrying )
		//{
		//	yield return new WaitForSeconds( playTime );
		//	if( charCtrl.state == CharState.guard )
		//		charCtrl.Delay( 0.5f );
		//	if( charCtrl.atkState == AttackState.parrying )
		//		charCtrl.atkState = AttackState.none;
		//	//임시. 애니메이션이 끝날때 atkState를 none으로 바꾸게끔 해야함.
		//}
		yield return null;
	}
}

public class Skill_Player_Attack_Thrusting : Skill_Player
{
	int attackState = 0;
	public Skill_Player_Attack_Thrusting( PlayerCtrl _charCtrl, int skillLevel ) : base( _charCtrl )
	{
		SkillType = CharState.attack;
		AttackState = AttackState.thrusting;
		attackType = AttackType.guardBreak;

		playTime = 1.0f;
		id = 3;
		titleStr = "찌르기";
		explainStr = "적의 가드를 무력화하는 찌르기 공격을 합니다.";
		perAd = new float[] { 2.0f, 2.5f, 3.0f };
		maxCoolTime = new float[] { 20f, 18f, 15f };
		needLevel = new int[] { 2, 2, 2 };

		//buttonScript = PlayerUIMgr.Instance.skillBtns[3];
		iconNum = 6;
	}
	public override string EffectStr( int level )
	{
		StringBuilder sb = new StringBuilder( );
		sb.Append( "피해량 : " );
		sb.Append( perAd[level] );
		sb.Append( " * 공격력 " );
		sb.Append( "\n쿨타임 : " );
		sb.Append( maxCoolTime[level] );
		sb.Append( "초" );
		return sb.ToString( );
	}
	public override void SkillColSetting( )
	{
		charCtrl.playerAtkScript.atk[2].ColSetting( charCtrl.status.Ad * perAd[skillLevel], charCtrl.status.Cr, AttackType.guardBreak, DamageType.melee );
	}
	public override void SkillCallback( PlayerCtrl _charCtrl, object _obj )
	{
	}

	public override bool Able( )
	{
		if( charCtrl.state == CharState.stun )
			return false;
		if( charCtrl.atkState != AttackState.none )
			return false;
		return true;
	}
	protected override IEnumerator CoPlay( )
	{
		charCtrl.katanaTrail.enabled = true;
		charCtrl.state = SkillType;
		charCtrl.atkState = AttackState;
		charCtrl.i_Animator.SetTrigger( "Attack_Avoid" );
		yield return charCtrl.StartCoroutine( CoPlayingCheck( ) );

		//if( charCtrl.atkState == AttackState.thrusting )
		//{
		//	yield return new WaitForSeconds( playTime );
		//	if( charCtrl.state == CharState.attack )
		//		charCtrl.state = CharState.idle;
		//	if( charCtrl.atkState == AttackState.thrusting )
		//		charCtrl.atkState = AttackState.none;
		//	//임시. 애니메이션이 끝날때 atkState를 none으로 바꾸게끔 해야함.
		//}
	}
}


























public enum MonsterType
{
	robot,
	monster,
	boss
}

public abstract class Skill
{
	public EnemyCtrl charCtrl;
	public CharState SkillType;

	public int level = 0;
	public int maxLevel = 3;

	public float playTime;
	public float coolTime;
	public float maxCoolTime;

	public bool isCancel;
	public bool isFinish;

	public Skill( EnemyCtrl _charCtrl )
	{
		charCtrl = _charCtrl;
		isFinish = true;
		isCancel = false;
	}

	public virtual void ResetCoolTime( )
	{
		isFinish = true;
		isCancel = false;
		coolTime = maxCoolTime + Time.time;
	}

	public virtual IEnumerator PlayingCheck( )
	{
		float time = playTime;
		while( time > 0f && !isCancel )
		{
			if( charCtrl.state != SkillType )
				isCancel = true;
			time -= Time.deltaTime;
			yield return null;
		}
	}

	public virtual bool Able( )
	{
		if( Time.time > coolTime && isFinish )
		{
			isFinish = true;
			isCancel = false;
			return true;
		}
		else
		{
			return false;
		}
	}
	public void Play( )
	{
		charCtrl.StartCoroutine( CoPlay( ) );
	}
	protected abstract IEnumerator CoPlay( );
	public abstract void SkillCallback( EnemyCtrl _charCtrl, object _obj );
}

public abstract class Skill_Enemy : Skill
{
	public Skill_Enemy( EnemyCtrl _charCtrl ) : base( _charCtrl )
	{
		charCtrl = _charCtrl;
		isFinish = true;
		isCancel = false;
	}
}

public class Skill_Enemy_Humanoid_Avoid : Skill_Enemy
{
	public Skill_Enemy_Humanoid_Avoid( EnemyCtrl _charCtrl, float _maxCoolTime ) : base( _charCtrl )
	{
		charCtrl = _charCtrl;
		isFinish = true;
		isCancel = false;
		SkillType = CharState.avoid;
		coolTime = 0f;
		playTime = 0.5f;
		maxCoolTime = _maxCoolTime;
	}
	public override void SkillCallback( EnemyCtrl _charCtrl, object _obj )
	{
	}
	public override bool Able( )
	{
		if( Time.time > coolTime && isFinish
			&& charCtrl.state != CharState.hit && charCtrl.state != CharState.stun )
		{
			isFinish = true;
			isCancel = false;
			return true;
		}
		else
		{
			return false;
		}
	}
	protected override IEnumerator CoPlay( )
	{
		charCtrl.isAvoid = true;
		isFinish = false;
		charCtrl.i_Animator.SetBool( "Avoid", true );

		float ran = UnityEngine.Random.Range(0f, 1f);
		if( ran < 0.5f )
			charCtrl.i_Animator.SetTrigger( "RollLeftTrigger" );
		else
			charCtrl.i_Animator.SetTrigger( "RollRightTrigger" );
		yield return charCtrl.StartCoroutine( PlayingCheck( ) );
		if( !isCancel )
			charCtrl.state = CharState.idle;
		charCtrl.i_Animator.SetBool( "Avoid", false );
		charCtrl.isAvoid = false;
		ResetCoolTime( );
	}
}

public class Skill_Enemy_Humanoid_AttackPunch : Skill_Enemy
{
	public Skill_Enemy_Humanoid_AttackPunch( EnemyCtrl _charCtrl, float _maxCoolTime ) : base( _charCtrl )
	{
		charCtrl = _charCtrl;
		isFinish = true;
		isCancel = false;
		SkillType = CharState.attack;
		coolTime = 0f;
		playTime = 0.9f;
		maxCoolTime = _maxCoolTime;
	}
	public override void SkillCallback( EnemyCtrl _charCtrl, object _obj )
	{
	}
	public override bool Able( )
	{
		if( Time.time > coolTime && isFinish )
		{
			isFinish = true;
			isCancel = false;
			return true;
		}
		else
		{
			return false;
		}
	}
	protected override IEnumerator CoPlay( )
	{
		charCtrl.isAttack = true;
		isFinish = false;
		charCtrl.i_Animator.applyRootMotion = false;
		charCtrl.myTrans.LookAt( charCtrl.targetTrans );

		float ran = UnityEngine.Random.Range(0f, 1f);
		if( ran < 0.5f )
			charCtrl.i_Animator.SetTrigger( "Attack3Trigger" );
		else
			charCtrl.i_Animator.SetTrigger( "Attack6Trigger" );

		yield return charCtrl.StartCoroutine( PlayingCheck( ) );
		if( !isCancel )
			charCtrl.state = CharState.idle;
		charCtrl.isAttack = false;
		charCtrl.i_Animator.applyRootMotion = true;
		ResetCoolTime( );
	}
}

public class Skill_Enemy_Humanoid_AttackKick : Skill_Enemy
{
	public Skill_Enemy_Humanoid_AttackKick( EnemyCtrl _charCtrl, float _maxCoolTime ) : base( _charCtrl )
	{
		charCtrl = _charCtrl;
		isFinish = true;
		isCancel = false;
		SkillType = CharState.attack;
		coolTime = 0f;
		playTime = 2.0f;
		maxCoolTime = _maxCoolTime;
	}
	public override void SkillCallback( EnemyCtrl _charCtrl, object _obj )
	{
	}
	public override bool Able( )
	{
		if( Time.time > coolTime && isFinish )
		{
			isFinish = true;
			isCancel = false;
			return true;
		}
		else
		{
			return false;
		}
	}
	protected override IEnumerator CoPlay( )
	{
		charCtrl.i_Animator.applyRootMotion = false;
		charCtrl.isAttack = true;
		isFinish = false;
		charCtrl.myTrans.LookAt( charCtrl.targetTrans );

		float ran = UnityEngine.Random.Range(0f, 1f);
		if( ran < 0.5f )
			charCtrl.i_Animator.SetTrigger( "AttackKick1Trigger" );
		else
			charCtrl.i_Animator.SetTrigger( "AttackKick2Trigger" );

		yield return charCtrl.StartCoroutine( PlayingCheck( ) );
		if( !isCancel )
			charCtrl.state = CharState.idle;
		charCtrl.isAttack = false;
		charCtrl.i_Animator.applyRootMotion = true;
		ResetCoolTime( );
	}
}


//public abstract class PassiveConditionSkill : MonoBehaviour//조건형 패시브
//{
//	public Buff buff;
//	public float maxCount; //스킬 발동 조건

//	public void SkillCheck( ref float count )
//	{
//		if( count >= maxCount )
//		{
//			//버프 적용 시킨다
//			count = 0f; //초기화 시킨다.
//		}
//		else
//		{
//			//버프 해제 시킨다
//		}
//	}
//}

//버프형
//public class ActiveBuffSkill : ActiveSkill
//{
//	public int buffType;

//	public ActiveBuffSkill( StatusScript stat, int _spriteId, PlanetUtil.WeaponType _type, int _idx )
//		: base( stat, _spriteId, _type, _idx )
//	{
//	}

//	public override void SkillCallback( StatusScript stat, object obj )
//	{
//	}

//	public override IEnumerator CoSkill( float time = 0.3f )
//	{
//		Debug.Log( "▶" + ToString( ) + " start" );
//		stat.StartCoroutine( isSwap( ) );
//		ResetCoolTime( );
//		yield return 0;
//	}

//	//public virtual Buff GetBuff( )
//	//{
//	//	return null;
//	//}

//	//public virtual Performence GetPerfromence( PlanetUtil.CharacterStat type, float add, float scale )
//	//{
//	//	Performence p = new Performence();
//	//	p.type = type;
//	//	p.add = add;
//	//	p.scale = scale;
//	//	return p;
//	//}
//	public float[] GetPlayTime( int count = 1 )
//	{
//		float[] f = new float[count];
//		for( int i = 0 ; i < f.Length ; ++i )
//			f[i] = playTime[level];
//		return f;
//	}
//	public virtual IEnumerator isSwap( )
//	{
//		float pTime = Time.time +  playTime[level];
//		while( weaponType == GameMgr.Instance.Player.nowWeaponType && pTime > Time.time )
//		{
//			yield return null;
//		}
//		GameMgr.Instance.Player.RemoveBuff( buffType );
//	}
//}