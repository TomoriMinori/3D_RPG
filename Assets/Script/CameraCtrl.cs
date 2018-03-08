using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;
	float lerpSpeed = 3f;

	public void CameraStart( )
	{
		StartCoroutine( CoUpdate( ) );
	}

	private void Awake( )
	{
		CameraStart( );
	}

	IEnumerator CoUpdate( )
	{
		while( true )
		{
			if( target != null )
				transform.position = Vector3.Lerp( transform.position, target.position + offset, lerpSpeed * Time.deltaTime );
			yield return null;
		}
	}
}