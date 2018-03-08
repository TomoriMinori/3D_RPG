using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public enum ItemKind
{
	equip1,
	equip2,
	equip3,
	potion
}

public enum ItemRate
{
	normal,
	rare,
	legend
}

public class Item
{
	public ItemKind itemKind;
	public ItemRate itemRate;
	public string name;
	public string explain;
	public int iconId;
	public Sprite iconSprite;

	public PlusStatus plusStatus;

	public int count;

	public Item( )
	{
		count = 1;
	}

	public virtual Item CopyItem( )
	{
		Item copy = new Item();
		copy.itemKind = this.itemKind;
		copy.itemRate = this.itemRate;
		copy.name = this.name;
		copy.explain = this.explain;
		copy.iconId = this.iconId;
		copy.iconSprite = this.iconSprite;

		copy.plusStatus = this.plusStatus;

		copy.count = this.count;
		return copy;
	}

	public Item( bool trued )
	{

	}

	StringBuilder sb;
	public string EffectT( )
	{
		sb = new StringBuilder( );
		EffectT( "공격력", plusStatus.PlusAd );
		EffectT( "생명력", plusStatus.PlusHp );
		EffectT( "치명률", plusStatus.PlusCr );

		EffectT( "공격력", plusStatus.PlusAdPer, true );
		EffectT( "생명력", plusStatus.PlusHpPer, true );
		EffectT( "치명률", plusStatus.PlusCrPer, true );

		EffectT( "힘", plusStatus.PlusStrong );
		EffectT( "민첩", plusStatus.PlusAgility );
		EffectT( "체력", plusStatus.PlusHealth );
		EffectT( "행운", plusStatus.PlusLuck );

		EffectT( "힘", plusStatus.PlusStrongPer, true );
		EffectT( "민첩", plusStatus.PlusAgilityPer, true );
		EffectT( "체력", plusStatus.PlusHealthPer, true );
		EffectT( "행운", plusStatus.PlusLuckPer, true );
		return sb.ToString( );
	}
	public void EffectT( string name, float value, bool per = false )
	{
		if( value > 0.01f )
		{
			sb.Append( name );
			sb.Append( " + " );
			if( per )
			{
				sb.Append( value );
				sb.Append( "%" );
			}
			else
				sb.Append( value );
			sb.Append( "\n" );
		}
	}

	public void UseEffect( )
	{
	}
}

public class Katana : Item
{
	public int katanaEffect = 0;
	public override Item CopyItem( )
	{
		var a = base.CopyItem( );

		return a;
	}
}

public class Ring : Item
{ }

public class Shoes : Item
{ }
