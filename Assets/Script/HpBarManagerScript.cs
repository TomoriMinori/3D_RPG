using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManagerScript : MonoBehaviour
{
	public 
	// Use this for initialization
	void Start( )
	{

	}

	public Transform target;
	void Update( )
	{
		Vector3 BeforeUIPos = Camera.main.WorldToViewportPoint(target.position);
		Vector3 AfterUIPos = Camera.main.ViewportToWorldPoint(BeforeUIPos);
	}
}
