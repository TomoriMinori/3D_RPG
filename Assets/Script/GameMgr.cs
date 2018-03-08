using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
	public static GameMgr Instance;
	[HideInInspector]
	public Vector2 gameResolution;
	public SceneKind nowScene;
	public GameObject button;
	[Space]
	public GameObject player;
	public PlayerCtrl playerCtrl;
	public GameObject joyStick;
	public PlayerUIMgr playerUIMgr;
	public EnemyUIMgr enemyUIMgr;
	[Space]
	public GameObject diePannel;

	void Awake( )
	{
		Instance = this;
		var canvas = GetComponent<CanvasScaler>();
		gameResolution = canvas.referenceResolution;
		playerCtrl = player.GetComponent<PlayerCtrl>( );
		//DontDestroyOnLoad( gameObject );
		//DontDestroyOnLoad( Camera.main );
		//DontDestroyOnLoad( GameObject.Find( "Canvas" ) );
		//DontDestroyOnLoad( GameObject.Find( "EventSystem" ) );
		//DontDestroyOnLoad( GameObject.Find( "PlayerOBPool" ) );
	}

	void LoadScene( )
	{
		if( nowScene == SceneKind.stage1 )
		{
			player.SetActive( true );
			//캐릭터를 먼저 만든 뒤
			button.SetActive( false );
			joyStick.SetActive( true );
			playerUIMgr.gameObject.SetActive( true );
			enemyUIMgr.gameObject.SetActive( true );
		}
	}

	public void GamePause( )
	{
		Time.timeScale = 0f;
		InGameObjectMgr.Instance.StopAllObject( );
	}

	public void GameResume( )
	{
		Time.timeScale = 1f;
		InGameObjectMgr.Instance.ResumeAllObject( );
	}

	public void GameOver( )
	{
		diePannel.SetActive( true );
	}

	public void NextScene( SceneKind scene )
	{
		nowScene = scene;
		SceneManager.LoadScene( (int)scene, LoadSceneMode.Single );
		LoadScene( );
	}

	public void NextScene( int scene )
	{
		nowScene = (SceneKind)scene;
		SceneManager.LoadScene( scene, LoadSceneMode.Single );
		LoadScene( );
	}
}

public enum SceneKind
{
	main,
	stage1
}