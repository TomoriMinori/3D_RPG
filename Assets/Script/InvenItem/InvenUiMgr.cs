using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenUiMgr : MonoBehaviour
{
	public InvenBox[] EquipBoxArr;
	public GameObject InvenParent;
	public List<InvenBox> InvenBoxList;
	[Space]
	public Image Select;
	public Text nameT;
	public Text kindT;
	public Text effectT;
	public Text explainT;
	[Space]
	public GameObject useBtn;
	public GameObject equipBtn;
	public GameObject cancelBtn;
	public GameObject destroyBtn;
	[HideInInspector] public Item SelectItem;
	[HideInInspector] public int SelectItemIndex;
	[HideInInspector] public bool SelectItemIsEquip;

	private void Awake( )
	{
		InvenBoxList = new List<InvenBox>( );
		var a = InvenParent.GetComponentsInChildren<InvenBox>( );
		for( int i = 0 ; i < a.Length ; ++i )
			InvenBoxList.Add( a[i] );
	}
	public void OnEnable( )
	{
		Show( );
	}

	public void Show( )
	{
		Select.sprite = null;
		nameT.text = "";
		kindT.text = "";
		effectT.text = "";
		explainT.text = "";

		useBtn.SetActive( false );
		equipBtn.SetActive( false );
		cancelBtn.SetActive( false );
		destroyBtn.SetActive( false );

		var inven = GameMgr.Instance.playerCtrl.inventory;
		for( int i = 0 ; i < inven.EquipArr.Length ; ++i )
		{
			if( inven.EquipArr[i] == null )
				EquipBoxArr[i].SetEmpty( );
			else
				EquipBoxArr[i].SetItem( i, inven.EquipArr[i] );
		}
		for( int i = 0 ; i < inven.maxCount ; ++i )
		{
			if( inven.ItemList[i] == null )
				InvenBoxList[i].SetEmpty( );
			else
				InvenBoxList[i].SetItem( i, inven.ItemList[i] );
		}
	}
	public void Explain( int index, bool isEquip = false )
	{
		var inven = GameMgr.Instance.playerCtrl.inventory;
		if( isEquip )
			SelectItem = inven.EquipArr[index];
		else
			SelectItem = inven.ItemList[index];
		SelectItemIndex = index;
		SelectItemIsEquip = isEquip;

		Select.sprite = SelectItem.iconSprite;
		nameT.text = SelectItem.name;
		explainT.text = SelectItem.explain;
		kindT.text = SelectItem.itemKind.ToString( );
		effectT.text = SelectItem.EffectT( );

		useBtn.SetActive( false );
		destroyBtn.SetActive( false );
		equipBtn.SetActive( false );
		cancelBtn.SetActive( false );

		if( SelectItem.itemKind == ItemKind.potion )
		{
			useBtn.SetActive( true );
			destroyBtn.SetActive( true );
		}
		else if( isEquip )
		{
			cancelBtn.SetActive( true );
			destroyBtn.SetActive( true );
		}
		else
		{
			equipBtn.SetActive( true );
			destroyBtn.SetActive( true );
		}
	}
	public void Sort( )
	{
		var inven = GameMgr.Instance.playerCtrl.inventory;
		inven.Sort( );
		Show( );
	}
	public void Use( )
	{
		var inven = GameMgr.Instance.playerCtrl.inventory;
		inven.Use( SelectItemIndex );
		Show( );
	}
	public void Equip( )
	{
		var inven = GameMgr.Instance.playerCtrl.inventory;
		inven.Equip( SelectItemIndex );
		Show( );
	}
	public void UnEquip( )
	{
		var inven = GameMgr.Instance.playerCtrl.inventory;
		inven.UnEquip( SelectItemIndex );
		Show( );
	}
	public void Destroy( )
	{
		var inven = GameMgr.Instance.playerCtrl.inventory;
		inven.Destroy( SelectItemIndex, SelectItemIsEquip );
		Show( );
	}
}