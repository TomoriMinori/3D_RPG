using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
	public void GetItem( int id )
	{
		var a = ResourceMgr.Instance.potionItemList[id];
		GameMgr.Instance.playerCtrl.inventory.GetItem( a.CopyItem( ) );
	}
	public void GetItem1( int id )
	{
		var a = ResourceMgr.Instance.equip1List[id];
		GameMgr.Instance.playerCtrl.inventory.GetItem( a.CopyItem( ) );
	}
	public void GetItem2( int id )
	{
		var a = ResourceMgr.Instance.equip2List[id];
		GameMgr.Instance.playerCtrl.inventory.GetItem( a.CopyItem( ) );
	}
	public void GetItem3( int id )
	{
		var a = ResourceMgr.Instance.equip3List[id];
		GameMgr.Instance.playerCtrl.inventory.GetItem( a.CopyItem( ) );
	}
}
