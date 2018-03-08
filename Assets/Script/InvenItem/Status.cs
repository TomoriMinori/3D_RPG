using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
	PlayerCtrl playerCtrl;
	public PlusStatus plusStatus;
	public int Level;
	public int NeedExp;

	int strong;
	public int Strong
	{
		get
		{
			return strong;
		}
		set
		{
			strong = value;
		}
	}
	int agility;
	public int Agility
	{
		get
		{
			return agility;
		}
		set
		{
			agility = value;
		}
	}
	int health;
	public int Health
	{
		get
		{
			return health;
		}
		set
		{
			health = value;
		}
	}
	int luck;
	public int Luck
	{
		get
		{
			return luck;
		}
		set
		{
			luck = value;
		}
	}

	public float Ad;
	public int Hp;
	public float Cr;
	public int unusedStat;
	float defaultAd;
	public float DefaultAd
	{
		get
		{
			return defaultAd;
		}
		set
		{
			defaultAd = value;
		}
	}
	float defaultCr;
	public float DefaultCr
	{
		get
		{
			return defaultCr;
		}
		set
		{
			defaultCr = value;
		}
	}
	float defaultHp;
	public float DefaultHp
	{
		get
		{
			return defaultHp;
		}
		set
		{
			defaultHp = value;
		}
	}

	public Status( PlayerCtrl playerCtrl )
	{
		this.playerCtrl = playerCtrl;
		defaultAd = 0;
		defaultHp = 100;
		defaultCr = 10;

		strong = 1;
		agility = 1;
		health = 1;
		luck = 1;

		NeedExp = 10;

		Level = 1;
		unusedStat = 100;
	}

	public void LevelUp( )
	{
		NeedExp = 10 * Mathf.Abs( Level );
		unusedStat += 3;
	}

	public void SetStatus( )
	{
		plusStatus.Default( );
		var equip = playerCtrl.inventory.EquipArr;
		for( int i = 0 ; i < equip.Length ; ++i )
		{
			if( equip[i] != null )
				plusStatus.SumStatus( equip[i].plusStatus );
		}
		float finalSt = ( Strong + plusStatus.PlusStrong ) * ( 1f + plusStatus.PlusStrongPer * 0.01f );
		float finalAg = ( Agility + plusStatus.PlusAgility ) * ( 1f + plusStatus.PlusAgilityPer * 0.01f);
		float finalHt = ( Health + plusStatus.PlusHealth ) * ( 1f + plusStatus.PlusHealthPer * 0.01f );
		float finalLk = ( Luck + plusStatus.PlusLuck ) * ( 1f + plusStatus.PlusLuckPer * 0.01f );

		Ad = DefaultAd + finalSt * 2f + finalAg * 1f;
		Hp = Mathf.RoundToInt( DefaultHp + finalHt * 10f );
		Cr = DefaultCr + finalAg * 1f;

		Ad *= ( 1f + plusStatus.PlusAdPer * 0.01f );
		Hp = Mathf.RoundToInt( Hp * ( 1f + plusStatus.PlusHpPer * 0.01f ) );
		Cr *= ( 1f + plusStatus.PlusCrPer * 0.01f );

		//playerCtrl.UpdateStatus( ); 이건 필요할 때 한번에 처리하는 걸로.
	}
}

public struct PlusStatus
{
	public int PlusStrong;
	public int PlusAgility;
	public int PlusHealth;
	public int PlusLuck;

	public int PlusStrongPer;
	public int PlusAgilityPer;
	public int PlusHealthPer;
	public int PlusLuckPer;

	public float PlusAd;
	public float PlusHp;
	public float PlusCr;

	public float PlusAdPer;
	public float PlusHpPer;
	public float PlusCrPer;

	public void Default( )
	{
		PlusStrong = 0;
		PlusAgility = 0;
		PlusHealth = 0;
		PlusLuck = 0;

		PlusStrongPer = 0;
		PlusAgilityPer = 0;
		PlusHealthPer = 0;
		PlusLuckPer = 0;

		PlusAd = 0;
		PlusHp = 0;
		PlusCr = 0;

		PlusAdPer = 0;
		PlusHpPer = 0;
		PlusCrPer = 0;
	}
	public void SumStatus( PlusStatus plusStatus )
	{
		PlusStrong += plusStatus.PlusStrong;
		PlusAgility += plusStatus.PlusAgility;
		PlusHealth += plusStatus.PlusHealth;
		PlusLuck += plusStatus.PlusLuck;

		PlusStrongPer += plusStatus.PlusStrongPer;
		PlusAgilityPer += plusStatus.PlusAgilityPer;
		PlusHealthPer += plusStatus.PlusHealthPer;
		PlusLuckPer += plusStatus.PlusLuckPer;

		PlusAd += plusStatus.PlusAd;
		PlusHp += plusStatus.PlusHp;
		PlusCr += plusStatus.PlusCr;

		PlusAdPer += plusStatus.PlusAdPer;
		PlusHpPer += plusStatus.PlusHpPer;
		PlusCrPer += plusStatus.PlusCrPer;
	}
}