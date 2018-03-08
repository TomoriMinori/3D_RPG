using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	public Item[] EquipArr;
	public List<Item> ItemList;
	public int maxCount;

	public Inventory( )
	{
		maxCount = 15;
		EquipArr = new Item[] { null, null, null };
		ItemList = new List<Item>( );
		for( int i = 0 ; i < maxCount ; ++i )
			ItemList.Add( null );
	}

	public bool CheckInven( int count )
	{
		if( ItemList.Count >= maxCount )
			return false;
		return true;
	}

	public void GetItem( Item item )
	{
		bool Get = false;
		if( item.itemKind == ItemKind.potion )
		{
			for( int i = 0 ; i < ItemList.Count ; ++i )
			{
				if( ItemList[i] != null )
				{
					if( ItemList[i].name == item.name )
					{
						ItemList[i].count++;
						Get = true;
						break;
					}
				}
			}
		}
		if( !Get )
		{
			for( int i = 0 ; i < ItemList.Count ; ++i )
			{
				if( ItemList[i] == null )
				{
					ItemList[i] = item;
					break;
				}
			}
		}
	}
	public void Sort( )
	{
		for( int i = 0 ; i < ItemList.Count - 1 ; ++i )
		{
			for( int j = i ; j < ItemList.Count - 1 ; ++j )
			{
				if( ItemList[j] == null )
				{
					if( ItemList[j + 1] != null )
					{
						ItemList[j] = ItemList[j + 1].CopyItem( );
						ItemList[j + 1] = null;
					}
				}
			}
		}
	}
	public void Use( int index )
	{
		var a = GameMgr.Instance.playerCtrl;
		var b = ItemList[index].plusStatus;
		int curHp = a.status.Hp;
		a.status.Strong += b.PlusStrong;
		a.status.Agility += b.PlusAgility;
		a.status.Health += b.PlusHealth;
		a.status.Luck += b.PlusLuck;

		a.status.DefaultAd += b.PlusAd;
		a.status.DefaultCr += b.PlusCr;
		a.status.DefaultHp += b.PlusHp;

		GameMgr.Instance.playerCtrl.status.SetStatus( );
		if( curHp != a.status.Hp )
			GameMgr.Instance.playerCtrl.UpdateHp( a.status.Hp - curHp );
		Destroy( index );
	}
	public void Equip( int index )
	{
		int eindex;
		if( ItemList[index].itemKind == ItemKind.equip1 )
			eindex = 0;
		else if( ItemList[index].itemKind == ItemKind.equip2 )
			eindex = 1;
		else
			eindex = 2;
		if( EquipArr[eindex] != null )
		{
			var before = EquipArr[eindex].CopyItem( );
			EquipArr[eindex] = ItemList[index];
			ItemList[index] = before;
		}
		else
		{
			EquipArr[eindex] = ItemList[index];
			ItemList[index] = null;
		}
		GameMgr.Instance.playerCtrl.status.SetStatus( );
	}
	public void UnEquip( int index )
	{
		GetItem( EquipArr[index] );
		EquipArr[index] = null;
		GameMgr.Instance.playerCtrl.status.SetStatus( );
	}
	public void Destroy( int index, bool isEquip = false )
	{
		if( isEquip )
		{
			EquipArr[index] = null;
			return;
		}
		ItemList[index].count--;
		if( ItemList[index].count == 0 )
			ItemList[index] = null;
		GameMgr.Instance.playerCtrl.status.SetStatus( );
	}
}