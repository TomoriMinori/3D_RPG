using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenBox : MonoBehaviour
{
	public InvenUiMgr invenUiMgr;
	public bool isEquipBox;
	public Image itemImage;

	public Image tempImg;
	public Text countT;
	[HideInInspector] public int index;

	public void SetEmpty( )
	{
		itemImage.enabled = false;
		tempImg.enabled = false;
		countT.enabled = false;
	}
	public void SetItem( int index, Item item )
	{
		this.index = index;
		countT.enabled = false;
		if( item.itemKind == ItemKind.potion )
		{
			countT.text = item.count.ToString( );
			countT.enabled = true;
		}
		itemImage.sprite = item.iconSprite;
		itemImage.enabled = true;
		tempImg.enabled = true;		
	}
	public void Click( )
	{
		if( !itemImage.enabled )
			return;
		if( isEquipBox )
		{
			invenUiMgr.Explain( index, true );
		}
		else
		{
			invenUiMgr.Explain( index );
		}
	}
}