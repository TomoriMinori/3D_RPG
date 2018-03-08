using UnityEngine;
using System.Collections;

public class door : InGameObject
{
	GameObject thedoor;
	protected new void Awake( )
	{
		SaveDefaultScale( );
		thedoor = GameObject.FindWithTag( "SF_Door" );
		StartCoroutine( CoUpdate( ) );
	}
	bool isopen = false;
	IEnumerator CoUpdate( )
	{
		float t = 0f;
		while( true )
		{
			if( t <= 0f )
			{
				if( isopen )
				{
					thedoor.GetComponent<Animation>( ).Play( "close" );
					t = 5f;
				}
				else
				{
					thedoor.GetComponent<Animation>( ).Play( "open" );
					t = 5f;
				}
			}
			t -= i_TimeDelta;
			yield return i_WaitForSeconds;
		}
	}
	private void Update( )
	{
		if( Input.GetKeyDown( KeyCode.Alpha1 ) )
			thedoor.GetComponent<Animation>( ).Play( "open" );
		if( Input.GetKeyDown( KeyCode.Alpha2 ) )
			thedoor.GetComponent<Animation>( ).Play( "close" );

	}

	void OnTriggerEnter( Collider obj )
	{
		thedoor.GetComponent<Animation>( ).Play( "open" );
	}
	void OnTriggerExit( Collider obj )
	{
		thedoor = GameObject.FindWithTag( "SF_Door" );
		thedoor.GetComponent<Animation>( ).Play( "close" );
	}
}