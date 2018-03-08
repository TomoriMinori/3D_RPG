using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickCtrl : MonoBehaviour
{
	public Transform Stick;
	public static Vector3 axis;

	float radius;
	Vector3 defaultCenter;
	Touch myTouch;

	void Start( )
	{
		radius = GetComponent<RectTransform>( ).sizeDelta.y * 0.25f;
		defaultCenter = Stick.position;
	}

	public void Move( )
	{
		Vector3 touchPos = Input.mousePosition;
		axis = ( touchPos - defaultCenter ).normalized;
		float distance = Vector3.Distance(touchPos,defaultCenter);
		if( distance > radius ) Stick.position = defaultCenter + axis * radius;
		else Stick.position = defaultCenter + axis * distance;
	}

	public void End( )
	{
		axis = Vector3.zero;
		Stick.position = defaultCenter;
	}
}