using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMgr
{
	public static QuestMgr Instance;
	public void QuestTextRemove( int uiId )
	{
	}
	public void PlayerDataSet( )
	{

	}
}

public class Static
{
	public static int level;
	public static List<Quest> questList;
	public static List<int> playerDic;
}

[System.Serializable]
public struct Quest
{
	public int id;
	public int uiId;
	public string name;
	public string explain;

	public bool reAble;
	public QuestState state;
	public DataKind kinds;

	public List<GoalInfo> goalList;
	public List<Reward> rewardList;

	public int needLevel;
	public int needClearQuest;

	public void QuestEnableCheck( )
	{
		if( state == QuestState.disable )
		{
			if( needLevel <= Static.level )
				if( needClearQuest == 0 )
					state = QuestState.enable;
				else if( Static.questList[needClearQuest].state == QuestState.complete )
					QuestStateSet( QuestState.enable );
		}
		else if( state != QuestState.complete && state != QuestState.enable )
		{
			//진행중 퀘스트 새로 등록
		}
	}

	public Quest( int id, string name, string explain, int needLevel, int needClearQuest, bool reAble )
	{
		this.id = id;
		this.name = name;
		this.explain = explain;
		this.state = QuestState.disable;
		this.goalList = new List<GoalInfo>( );
		this.rewardList = new List<Reward>( );
		this.needLevel = needLevel;
		this.needClearQuest = needClearQuest;
		this.reAble = reAble;

		uiId = 0;
		kinds = 0;
		//Debug.Log( id + " " + this.questName );
	}

	public void QuestStateSet( QuestState state )
	{
		this.state = state;
		switch( state )
		{
		case QuestState.enable: break;
		case QuestState.ing: break;
		case QuestState.clear: break;
		case QuestState.complete:
			if( reAble )
			{
				foreach( var a in goalList )
					a.GoalSetDefault( );
				this.state = QuestState.enable;
			}
			break;
		}
	}

	public void QuestStart( )
	{
		foreach( var a in goalList )
			a.GoalSetDefault( );
		QuestStateSet( QuestState.ing );
	}

	public void QuestGiveup( )
	{
		QuestMgr.Instance.QuestTextRemove( uiId );
		foreach( var a in goalList )
			a.GoalSetDefault( );
		QuestStateSet( QuestState.enable );
	}

	public void QuestComplete( )
	{
		QuestMgr.Instance.QuestTextRemove( uiId );
		QuestStateSet( QuestState.complete );
	}

	//죽으면 id와 kill값을 넘긴다.
	//퀘스트에 kind, id , count를 넘긴다.


	//몬스터 처치, 아이템 획득 등의 경우에 호출된다.
	public void goalCountSet( DataKind kind, int id, int count )
	{
		if( ( kinds & kind ) == 0 ) //퀘스트에 해당 kind를 가진 목표가 없다면 return
			return;
		for( int i = 0 ; i < goalList.Count ; ++i )
		{
			if( goalList[i].kind == kind )
			{
				if( goalList[i].targetIdList.Contains( id ) )
				{
					var gl = goalList[i];
					gl.goalCount += count;
					goalList[i] = gl;
					isClear( ); //이후 해당 퀘스트의 상태를 갱신.
				}
			}
		}
	}

	public void isClear( )
	{
		QuestState curState = QuestState.clear; //아래의 조건을 모두 만족하면 퀘스트 clear.
		for( int i = 0 ; i < goalList.Count ; ++i )
		{
			if( !isGoalClear( goalList[i] ) )
			{
				curState = QuestState.ing;
				break; //조건을 만족시키지 못한 목표가 있으므로 ing.
			}
		}
		QuestStateSet( curState ); //최종적으로 퀘스트 상태를 변경.
	}

	public bool isGoalClear( GoalInfo goal )
	{
		if( goal.goalCount >= goal.targetCount )
			return true;
		return false;
	}
}


[System.Serializable]
public struct GoalInfo
{
	public int id;
	public DataKind kind;
	public string goalName;
	public string targetName;
	public int targetKey;
	public int goalCount;
	public List<int> targetIdList;
	public int targetCount;
	public int targetCurCount;

	public void GoalSetDefault( )
	{
		switch( kind )
		{
		case DataKind.kill:
			targetCurCount = Static.playerDic[targetKey];
			break;
		case DataKind.item:
			//QuestMgr.Instance.PlayerDataSet( DataKind.item, targetKey, -targetCount );
			break;
		case DataKind.clear: break;
		case DataKind.level: break;
		case DataKind.npc:
			//targetCurCount = PlayerData.Instance.playerDic[targetKey];
			break;
		default: break;
		}
	}

	public GoalInfo( int id, string kind, string goalName, string targetName, int targetKey, int targetCount )
	{
		this.targetCurCount = 0;
		this.id = id;
		this.goalName = goalName;
		this.targetName = targetName;
		this.targetKey = targetKey;
		this.targetCount = targetCount;

		this.goalCount = 0; //playerdic에서 받아옴
		this.targetIdList = new List<int>( );

		switch( kind )
		{
		case "kill":
			this.kind = DataKind.kill; break;
		case "item":
			this.kind = DataKind.item; break;
		case "clear":
			this.kind = DataKind.clear; break;
		case "level":
			this.kind = DataKind.level; break;
		case "gold":
			this.kind = DataKind.gold; break;
		case "exp":
			this.kind = DataKind.exp; break;
		case "npc":
			this.kind = DataKind.npc; break;
		default:
			this.kind = DataKind.clear; break;
		}
	}
}

[System.Serializable]
public struct Reward
{
	public string targetName;
	public string targetKey;
	public int targetCount;

	public Reward( string targetName, string targetKey, int targetCount )
	{
		//switch( kind )
		//{
		//case "kill":
		//	this.kind = DataKind.kill; break;
		//case "item":
		//	this.kind = DataKind.item; break;
		//case "clear":
		//	this.kind = DataKind.clear; break;
		//case "level":
		//	this.kind = DataKind.level; break;
		//case "gold":
		//	this.kind = DataKind.gold; break;
		//case "exp":
		//	this.kind = DataKind.exp; break;
		//default:
		//	this.kind = DataKind.clear; break;
		//}
		this.targetName = targetName;
		this.targetKey = targetKey;
		this.targetCount = targetCount;
	}
}

public enum QuestState
{
	disable,
	enable,
	ing,
	clear,
	complete
}

public enum DataKind
{
	kill = 0,
	item = 1,
	clear = 2,
	level = 3,
	exp = 4,
	gold = 5,
	npc = 6
}