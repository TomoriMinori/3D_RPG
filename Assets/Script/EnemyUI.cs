using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
	[HideInInspector]
	public bool isUse;
	public EnemyType enemyType;
	public Text nameText;
	public Slider hpSlider;
	public RectTransform rectTrans;

	Transform targetTrans;
	float HpPer;
	float updateTime;
	float maxTime;
	float playTime;
	Vector2 resolution;
	Vector2 offset;

	void Awake( )
	{
		Init( );
	}

	public void Init( )
	{
		isUse = false;
		targetTrans = null;
		updateTime = 0.04f;
		maxTime = 5f;
		offset = new Vector2( -0.5f, -0.3334f );
		resolution = GameMgr.Instance.gameResolution;
	}

	public void SetTargetTrans( string name, EnemyType type, Transform target, float per )
	{
		gameObject.SetActive( true );
		isUse = true;
		enemyType = type;
		nameText.text = name;
		targetTrans = target;

		HpPer = per;
		hpSlider.value = HpPer;
		playTime = maxTime;

		gameObject.SetActive( true );
		StartCoroutine( CoSetHpSlider( per ) );
	}

	IEnumerator CoSetHpSlider( float per )
	{
		yield return new WaitForEndOfFrame( );
		StartCoroutine( CoUpdate( ) );
	}

	public void SetHpSlider( float per ) //다른 곳에서 hpslider만 호출할 때
	{
		if( !gameObject.activeSelf )
		{
			gameObject.SetActive( true );
			StartCoroutine( CoUpdate( ) );
		}
		HpPer = per;
		hpSlider.value = HpPer;
		playTime = maxTime;
	}

	IEnumerator CoUpdate( )
	{
		playTime = maxTime;
		Vector3 BeforeUIPos;
		Vector3 AfterUIPos;
		while( playTime >= 0f )
		{
			BeforeUIPos = Camera.main.WorldToViewportPoint( targetTrans.position );
			AfterUIPos = new Vector3( ( BeforeUIPos.x + offset.x ) * resolution.x, ( BeforeUIPos.y + offset.y ) * resolution.y, 0f );
			rectTrans.localPosition = AfterUIPos;
			playTime -= updateTime;
			yield return null;
		}
		isUse = false;
		gameObject.SetActive( false );
	}
}