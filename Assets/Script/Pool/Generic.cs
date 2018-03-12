
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if( _instance == null )
			{
				_instance = FindObjectOfType( typeof( T ) ) as T;
				if( _instance == null )
				{
					Debug.LogError( "There's no active " + typeof( T ) + " in this scene" );
				}
			}
			return _instance;
		}
	}
}

//public enum CalculationSign
//{
//	plus,
//	minus,
//	multiple,
//	division
//}

//public struct MathValues
//{
//	public List<MathValue> mathValues;
//	public bool IsParenthess;           //괄호 여부
//}

//public struct MathValue
//{
//	public int value;
//	public CalculationSign sign;
//}

//public class Mathdddd
//{
//	public List<MathValue> mathValues = new List<MathValue>();

//	public void gogogogo( )
//	{
//		int result = 0;
//		for( int i = 0 ; i < mathValues.Count -1 ; ++i )
//		{			
//		}
//	}
//	public int Result( int _left, CalculationSign _sign, int _right )
//	{
//		return 0;
//	}

//	//출력용
//	//부호별 리턴. label의 string이나 sprite name 등
//	public string GetCalculationSign( CalculationSign _sign )
//	{
//		switch( _sign )
//		{
//		case CalculationSign.plus: return "+";
//		case CalculationSign.minus: return "-";
//		case CalculationSign.multiple: return "X";
//		case CalculationSign.division: return "/";
//		}
//		return "";
//	}
//}