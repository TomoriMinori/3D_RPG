using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
	public Text damageText;
	public Image damageImage;
	public RectTransform rectTrans;

	Transform targetTrans;
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
		targetTrans = null;
		updateTime = 0.04f;
		maxTime = 0.5f;
		offset = new Vector2( -0.5f, -0.3334f );
		resolution = GameMgr.Instance.gameResolution;
	}

	public void ShowText( Transform _target, float _damage, bool _cri, bool _isMon )
	{
		Init( );
		targetTrans = _target;
		if( _cri )
		{
			damageText.fontSize = 20;
			damageText.text = _damage.ToString( ) + "!";
		}
		else
		{
			damageText.fontSize = 25;
			damageText.text = _damage.ToString( );
		}
		if( _isMon )
			damageText.color = Color.green;
		else
			damageText.color = Color.red;
		StartCoroutine( CoUpdate( ) );
	}

	IEnumerator CoUpdate( )
	{
		playTime = maxTime;
		Vector3 BeforeUIPos;
		Vector3 AfterUIPos;
		Vector4 c = damageText.color;
		Vector4 c2 = damageImage.color;
		while( playTime >= 0f )
		{
			float off = Mathf.Lerp(50f,150f,maxTime - playTime);
			c.w = Mathf.Lerp( 1f, 0f, maxTime - playTime );
			c2.w = Mathf.Lerp( 1f, 0f, maxTime - playTime );
			BeforeUIPos = Camera.main.WorldToViewportPoint( targetTrans.position );
			AfterUIPos = new Vector3( ( BeforeUIPos.x + offset.x ) * resolution.x, ( BeforeUIPos.y + offset.y ) * resolution.y + off, 0f );
			rectTrans.localPosition = AfterUIPos;
			damageText.color = c;
			damageImage.color = c2;
			playTime -= Time.deltaTime;
			yield return null;
		}
		gameObject.SetActive( false );
	}
}