using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectTest : InGameObject
{
	float speed = 0.5f;
	private void Start( )
	{
		StartCoroutine( CoUpdate( ) );
	}
	IEnumerator CoUpdate( )
	{
		while( true )
		{
			transform.position += Vector3.forward * i_TimeScale * 0.02f * speed;
			yield return i_WaitForSeconds;
		}
	}
}