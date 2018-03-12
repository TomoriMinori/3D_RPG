using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarpBoomScript : InGameObject
{
	public float timeScale;
	public float duration;
	private float forceX = 500f;
	private float forceY = 200f;
	private float playTime = 3f;
	private bool isBoom = false;

	public Transform forwardTrans;
	public TimeWarpBoomColScript timeColScript;
	public override void AwakeInit( )
	{
		GetComponent<MeshRenderer>( ).enabled = true;
		i_Rigidbody.velocity = Vector3.zero;
		i_Rigidbody.isKinematic = false;
		isBoom = false;
		i_ParticleSystem.gameObject.SetActive( false );
	}
	private new void Awake( )
	{
		SaveDefaultScale( );
	}
	private void Start( )
	{
		forwardTrans = transform;
		timeColScript.targetList.Clear( );
		timeColScript.timeScale = timeScale;
		timeColScript.duration = duration;
		Shoot( );
	}
	public void Shoot( )
	{
		i_Rigidbody.AddForce( forwardTrans.forward * forceX + forwardTrans.up * forceY );
	}
	private void Boom( )
	{
		i_Rigidbody.isKinematic = true;
		StartCoroutine( CoBoom( ) );
		GetComponent<MeshRenderer>( ).enabled = false;
	}
	private IEnumerator CoBoom( )
	{
		float time = 0f;
		i_ParticleSystem.gameObject.SetActive( true );
		while( !isBoom && time <= playTime )
		{
			time += i_TimeDelta;
			yield return null;
		}
		if( !isBoom )
			Boom( );
		yield return new WaitForSeconds( 1f );
		i_ParticleSystem.gameObject.SetActive( false );
		gameObject.SetActive( false );
	}

	private void OnCollisionEnter( Collision collision )
	{
		if( !isBoom )
			Boom( );
	}
}