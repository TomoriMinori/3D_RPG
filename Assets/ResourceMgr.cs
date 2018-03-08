using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr : MonoBehaviour
{
	public static ResourceMgr Instance;
	public Sprite[] skill;
	[Space]
	public Sprite[] equip1;
	public Sprite[] equip2;
	public Sprite[] equip3;
	public Sprite[] potion;

	string skillPath;
	string equip1Path;
	string equip2Path;
	string equip3Path;
	string potionPath;

	public List<Item> equip1List;
	public List<Item> equip2List;
	public List<Item> equip3List;
	public List<Item> potionItemList;

	void Awake( )
	{
		Instance = this;
		skillPath = "Sprite/Skill";
		equip1Path = "Sprite/Item/Equip1";
		equip2Path = "Sprite/Item/Equip2";
		equip3Path = "Sprite/Item/Equip3";
		potionPath = "Sprite/Item/Potion";

		skill = Resources.LoadAll<Sprite>( skillPath );
		equip1 = Resources.LoadAll<Sprite>( equip1Path );
		equip2 = Resources.LoadAll<Sprite>( equip2Path );
		equip3 = Resources.LoadAll<Sprite>( equip3Path );
		potion = Resources.LoadAll<Sprite>( potionPath );
	}
	private void Start( )
	{
		equip1List = new List<Item>( );
		equip2List = new List<Item>( );
		equip3List = new List<Item>( );
		potionItemList = new List<Item>( );
		for( int i = 0 ; i < 3 ; ++i )
		{
			MakeE1( i );
			MakeE2( i );
			MakeE3( i );
		}
		for( int i = 0 ; i < 4 ; ++i )
			MakePotion( i );
	}

	private void MakeE1( int num )
	{
		if( num == 0 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip1;
			item.itemRate = ItemRate.normal;
			item.name = "검기(1)";
			item.explain = "검에 검기를 불어 넣는다.";
			item.iconId = 0;
			item.iconSprite = equip1[item.iconId];
			item.count = 1;

			item.plusStatus.PlusAdPer = 10f;
			equip1List.Add( item );
		}
		if( num == 1 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip1;
			item.itemRate = ItemRate.rare;
			item.name = "검기(2)";
			item.explain = "검에 짱센 검기를 불어 넣는다.";
			item.iconId = 1;
			item.iconSprite = equip1[item.iconId];
			item.count = 1;

			item.plusStatus.PlusStrong = 5;
			item.plusStatus.PlusAdPer = 30f;
			equip1List.Add( item );
		}
		if( num == 2 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip1;
			item.itemRate = ItemRate.legend;
			item.name = "검기(3)";
			item.explain = "검에 전설의 검기를 불어 넣는다.";
			item.iconId = 2;
			item.iconSprite = equip1[item.iconId];
			item.count = 1;

			item.plusStatus.PlusStrong = 10;
			item.plusStatus.PlusAgility = 10;
			item.plusStatus.PlusAdPer = 50f;
			equip1List.Add( item );
		}
	}

	private void MakeE2( int num )
	{
		if( num == 0 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip2;
			item.itemRate = ItemRate.normal;
			item.name = "반지(1)";
			item.explain = "무슨 힘이 깃들었는지 모를 신비한 반지.";
			item.iconId = 0;
			item.iconSprite = equip2[item.iconId];
			item.count = 1;

			item.plusStatus.PlusLuck = 5;
			item.plusStatus.PlusCrPer = 5f;
			equip2List.Add( item );
		}
		if( num == 1 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip2;
			item.itemRate = ItemRate.rare;
			item.name = "반지(2)";
			item.explain = "무슨 힘이 깃들었는지 모를 신비한 반지.";
			item.iconId = 1;
			item.iconSprite = equip2[item.iconId];
			item.count = 1;

			item.plusStatus.PlusLuck = 10;
			item.plusStatus.PlusCrPer = 10f;
			equip2List.Add( item );
		}
		if( num == 2 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip2;
			item.itemRate = ItemRate.legend;
			item.name = "반지(3)";
			item.explain = "무슨 힘이 깃들었는지 모를 신비한 반지.";
			item.iconId = 2;
			item.iconSprite = equip2[item.iconId];
			item.count = 1;

			item.plusStatus.PlusStrong = 5;
			item.plusStatus.PlusAgility = 5;
			item.plusStatus.PlusHealth = 5;
			item.plusStatus.PlusCrPer = 50f;
			equip2List.Add( item );
		}
	}

	private void MakeE3( int num )
	{
		if( num == 0 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip3;
			item.itemRate = ItemRate.normal;
			item.name = "신발(1)";
			item.explain = "착용감이 다소 구려 보이는 신발";
			item.iconId = 0;
			item.iconSprite = equip3[item.iconId];
			item.count = 1;

			item.plusStatus.PlusAgilityPer = 5;
			item.plusStatus.PlusCrPer = 5;
			equip3List.Add( item );
		}
		if( num == 1 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip3;
			item.itemRate = ItemRate.rare;
			item.name = "신발(2)";
			item.explain = "착용감이 괜찮은 신발";
			item.iconId = 1;
			item.iconSprite = equip3[item.iconId];
			item.count = 1;

			item.plusStatus.PlusAgilityPer = 10;
			item.plusStatus.PlusCrPer = 10;
			equip3List.Add( item );
		}
		if( num == 2 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.equip3;
			item.itemRate = ItemRate.legend;
			item.name = "신발(3)";
			item.explain = "착용감이 좋은 신발";
			item.iconId = 2;
			item.iconSprite = equip3[item.iconId];
			item.count = 1;

			item.plusStatus.PlusAgilityPer = 20;
			item.plusStatus.PlusCrPer = 20;
			equip3List.Add( item );
		}
	}

	private void MakePotion( int num )
	{
		if( num == 0 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.potion;
			item.itemRate = ItemRate.normal;
			item.name = "신선한 물";
			item.explain = "지하 암반수로 제조한 비싼 물이다. 마시면 체력이 늘어날 것 같다.";
			item.iconId = 0;
			item.iconSprite = potion[item.iconId];

			item.plusStatus.PlusHp = 5f;
			potionItemList.Add( item );
		}
		if( num == 1 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.potion;
			item.itemRate = ItemRate.normal;
			item.name = "신선한 고기";
			item.explain = "맛있어 보이는 고기다. 누가 조리했을까?";
			item.iconId = 1;
			item.iconSprite = potion[item.iconId];

			item.plusStatus.PlusStrong = 2;
			item.plusStatus.PlusHealth = 2;
			potionItemList.Add( item );
		}
		if( num == 2 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.potion;
			item.itemRate = ItemRate.rare;
			item.name = "고오급 영양제";
			item.explain = "고오급 영양제라고 적혀있다. 믿음직한 문구다.";
			item.iconId = 2;
			item.iconSprite = potion[item.iconId];

			item.plusStatus.PlusStrong = 3;
			item.plusStatus.PlusAgility = 3;
			item.plusStatus.PlusHealth = 3;
			potionItemList.Add( item );
		}
		if( num == 3 )
		{
			Item item = new Item();
			item.itemKind = ItemKind.potion;
			item.itemRate = ItemRate.legend;
			item.name = "한정판 까나리 에디션";
			item.explain = "세계적으로 100개만 생산했다는 초 한정판 츄파캔디. 까나리의 기운이 모든 스탯을 상승시킬 것만 같다.";
			item.iconId = 3;
			item.iconSprite = potion[item.iconId];

			item.plusStatus.PlusStrong = 10;
			item.plusStatus.PlusAgility = 10;
			item.plusStatus.PlusHealth = 10;
			item.plusStatus.PlusLuck = 10;
			potionItemList.Add( item );
		}
	}
}