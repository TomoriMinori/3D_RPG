using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : InGameObject
{
	public GameObject doorOB;
	protected virtual new void Awake( )
	{
		SaveDefaultScale( );
	}
	private void Start( )
	{
		StartCoroutine( CoUpdate( ) );
	}

	float delayTime = 0.5f;
	float playTime = 1f;

	bool IsOpen = false;
	IEnumerator CoUpdate( )
	{
		float time = 0f;
		while( true )
		{
			if( time <= 0f )
			{
				if( !IsOpen )
				{
					yield return StartCoroutine( CoOpen( ) );
					time = delayTime;
				}
				else
				{
					yield return StartCoroutine( CoClose( ) );
					time = delayTime;
				}
				IsOpen = !IsOpen;
			}
			time -= i_TimeDelta;
			yield return null;
		}
	}

	IEnumerator CoOpen( )
	{
		float time = 0f;
		Quaternion start = Quaternion.Euler(0f,0f,0f);
		Quaternion end = Quaternion.Euler(0f,90f,0f);
		while( time <= playTime )
		{
			doorOB.transform.localRotation = Quaternion.Lerp( start, end, time / playTime );
			time += i_TimeDelta;
			yield return null;
		}
		doorOB.transform.localRotation = end;
	}
	IEnumerator CoClose( )
	{
		float time = 0f;
		Quaternion start = Quaternion.Euler(0f,90f,0f);
		Quaternion end = Quaternion.Euler(0f,0f,0f);
		while( time <= playTime )
		{
			doorOB.transform.localRotation = Quaternion.Lerp( start, end, time / playTime );
			time += i_TimeDelta;
			yield return null;
		}
		doorOB.transform.localRotation = end;
	}
}