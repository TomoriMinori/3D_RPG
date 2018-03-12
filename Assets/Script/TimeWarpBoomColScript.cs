using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarpBoomColScript : MonoBehaviour
{
	public float timeScale;
	public float duration;
	public List<InGameObject> targetList = new List<InGameObject>();

	private void OnTriggerEnter( Collider other )
	{
		if( other.GetComponent<InGameObject>( ) )
		{
			if( other.GetComponent<PlayerCtrl>( ) )
				return;
			InGameObject igo = other.GetComponent<InGameObject>( );
			if( targetList.Contains( igo ) )
				return;
			InGameObjectMgr.Instance.SetObjectTimeScale( igo, timeScale, duration );
			targetList.Add( igo );
			Debug.Log( "go" );
		}
	}
}
