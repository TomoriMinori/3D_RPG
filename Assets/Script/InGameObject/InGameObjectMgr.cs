using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectMgr : MonoBehaviour
{
	public static InGameObjectMgr Instance;

	public List<InGameObject> playerInGameObjects;
	public List<InGameObject> monsterInGameObjects;
	public List<InGameObject> otherInGameObjects;

	private void Awake( )
	{
		Instance = this;
	}

	public void StopAllObject( )
	{
		ForMacro( 1, monsterInGameObjects );
		ForMacro( 1, playerInGameObjects );
		ForMacro( 1, otherInGameObjects );
	}
	public void ResumeAllObject( )
	{
		ForMacro( 2, monsterInGameObjects );
		ForMacro( 2, playerInGameObjects );
		ForMacro( 2, otherInGameObjects );
	}

	public void SetObjectTimeScale( InGameObject _inGameObject, float _timeScale, float _duration, bool _compulsion = false )
	{
		if( !_inGameObject.I_IsChange || _compulsion )
			_inGameObject.SetTimeScale( _timeScale, _duration );
	}

	public void SetObjectDefaultTimeScale( InGameObject _inGameObject, bool _compulsion = false )
	{
		if( !_inGameObject.I_IsChange || _compulsion )
			_inGameObject.TimeScaleBack( );
	}

	private void Update( )
	{
		//if( Input.GetKeyDown( KeyCode.Alpha1 ) )
		//{
		//	for( int i = 0 ; i < monsterInGameObjects.Count ; ++i )
		//	{
		//		SetObjectDefaultTimeScale( monsterInGameObjects[i] );
		//	}
		//}
		//if( Input.GetKeyDown( KeyCode.Alpha2 ) )
		//{
		//	SetMonsterTimeScale( 0.5f, 5f );
		//}
		//if( Input.GetKeyDown( KeyCode.Alpha3 ) )
		//{
		//	SetMonsterTimeScale( 0.2f, 5f );
		//}

		//if( Input.GetKeyDown( KeyCode.Alpha4 ) )
		//{
		//	for( int i = 0 ; i < monsterInGameObjects.Count ; ++i )
		//	{
		//		SetObjectDefaultTimeScale( monsterInGameObjects[i], true );
		//	}
		//}
		//if( Input.GetKeyDown( KeyCode.Alpha5 ) )
		//{
		//	SetMonsterTimeScale( 0.5f, 5f, true );
		//}
		//if( Input.GetKeyDown( KeyCode.Alpha6 ) )
		//{
		//	SetMonsterTimeScale( 0.2f, 5f, true );
		//}


		//if( Input.GetKeyDown( KeyCode.Alpha9 ) )
		//{
		//	Time.timeScale = 0f;
		//	StopAllObject( );
		//}
		//if( Input.GetKeyDown( KeyCode.Alpha0 ) )
		//{
		//	Time.timeScale = 1f;
		//	ResumeAllObject( );
		//}
	}

	private void SetMonsterTimeScale( float _timeScale, float _duration = 0f, bool _compulsion = false )
	{
		ForMacro( 0, monsterInGameObjects, _timeScale, _duration, _compulsion );
	}
	private void SetPlayerTimeScale( float _timeScale, float _duration = 0f, bool _compulsion = false )
	{
		ForMacro( 0, playerInGameObjects, _timeScale, _duration, _compulsion );
	}
	private void SetOtherTimeScale( float _timeScale, float _duration = 0f, bool _compulsion = false )
	{
		ForMacro( 0, otherInGameObjects, _timeScale, _duration, _compulsion );
	}
	private void ForMacro( int _methodId, List<InGameObject> _list, float _timeScale = 0f, float _duration = 0f, bool _compulsion = false )
	{
		for( int i = 0 ; i < _list.Count ; ++i )
		{
			switch( _methodId )
			{
			case 0:
				if( !_list[i].I_IsChange || _compulsion )
					_list[i].SetTimeScale( _timeScale, _duration );
				break;
			case 1: _list[i].StopObject( ); break;
			case 2: _list[i].ResumeObject( ); break;
			}
		}
	}
}