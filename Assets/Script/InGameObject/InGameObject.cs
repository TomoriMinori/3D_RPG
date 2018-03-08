using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TimeCtrlState
{
	Defalut,
	DefaultToChange,
	StopToCur
}
[ExecuteInEditMode]
[AddComponentMenu( "InGameObject" )]
public class InGameObject : MonoBehaviour
{
	public Animator i_Animator;
	public Rigidbody i_Rigidbody;
	public NavMeshAgent i_NavMeshAgent;
	public ParticleSystem i_ParticleSystem;
	public AudioSource i_AudioSource;
	public WaitForSeconds i_WaitForSeconds = new WaitForSeconds(0.02f);
	public WaitForSeconds FSM_WaitForSeconds = new WaitForSeconds(0.1f);

	protected float i_TimeScale = 1f;
	protected float i_TimeDelta = 0.02f;

	private float defaultTimeScale;

	private float defaultAnimatorSpeed;
	private float defaultNavMeshAgentSpeed;
	private float defaultParticleSystemSpeed;
	private float defaultAudioSourcePitch;

	private Coroutine lastRoutine;

	private bool i_IsChange = false;
	public bool I_IsChange
	{
		get { return i_IsChange; }
		set { i_IsChange = value; }
	}

	protected void Awake( )
	{
		//Debug.Log( "출력되나?" );
		AwakeInit( );
		SaveDefaultScale( );
	}

	protected void AwakeInit( )
	{
		if( i_Animator == null )
			i_Animator = GetComponent<Animator>( );
		if( i_Rigidbody == null )
			i_Rigidbody = GetComponent<Rigidbody>( );
		if( i_NavMeshAgent == null )
			i_NavMeshAgent = GetComponent<NavMeshAgent>( );
		if( i_ParticleSystem == null )
			i_ParticleSystem = GetComponent<ParticleSystem>( );
		if( i_AudioSource == null )
			i_AudioSource = GetComponent<AudioSource>( );
	}

	protected void SaveDefaultScale( )
	{
		defaultTimeScale = i_TimeScale;
		if( i_Animator != null )
			defaultAnimatorSpeed = i_Animator.speed;
		if( i_NavMeshAgent != null )
			defaultNavMeshAgentSpeed = i_NavMeshAgent.speed;
		if( i_ParticleSystem != null )
			defaultParticleSystemSpeed = i_ParticleSystem.main.simulationSpeed;
		if( i_AudioSource != null )
			defaultAudioSourcePitch = i_AudioSource.pitch;
	}

	public void StopObject( )
	{
		if( i_AudioSource != null )
			i_AudioSource.Pause( );
	}

	public void ResumeObject( )
	{
		if( i_AudioSource != null )
			i_AudioSource.Play( );
	}

	//시간제 조작. 0f면 영구 조작
	public void SetTimeScale( float _timeScale, float _duration = 0f )
	{
		if( lastRoutine != null )
			StopCoroutine( lastRoutine );
		Debug.Log( "조작시작" );
		i_TimeScale = defaultTimeScale * _timeScale;
		i_TimeDelta = 0.02f / i_TimeScale;
		i_WaitForSeconds = new WaitForSeconds( i_TimeDelta );
		FSM_WaitForSeconds = new WaitForSeconds( i_TimeDelta * 2f );
		TimeScaleCtrl( );
		if( _duration != 0f )
			lastRoutine = StartCoroutine( CoTimeScaleBack( _duration ) );
	}

	//조작 해제
	public void TimeScaleBack( )
	{
		if( lastRoutine != null )
			StopCoroutine( lastRoutine );
		Debug.Log( "조작해제" );
		I_IsChange = false;
		i_TimeScale = defaultTimeScale;
		i_TimeDelta = 0.02f / i_TimeScale;
		i_WaitForSeconds = new WaitForSeconds( i_TimeDelta );
		FSM_WaitForSeconds = new WaitForSeconds( i_TimeDelta * 2f );
		if( i_Animator != null )
			i_Animator.speed = defaultAnimatorSpeed;
		if( i_NavMeshAgent != null )
			i_NavMeshAgent.speed = defaultNavMeshAgentSpeed;
		if( i_AudioSource != null )
			i_AudioSource.pitch = defaultAudioSourcePitch;
		if( i_ParticleSystem != null )
		{
			var a = i_ParticleSystem.main;
			a.simulationSpeed = defaultParticleSystemSpeed;
		}
	}

	private IEnumerator CoTimeScaleBack( float _duration = 0f )
	{
		I_IsChange = true;
		while( _duration >= 0f )
		{
			_duration -= Time.deltaTime;
			yield return null;
		}
		TimeScaleBack( );
		Debug.Log( "조작끝" );
	}

	protected void TimeScaleCtrl( )
	{
		i_IsChange = true;
		if( i_Animator != null )
			i_Animator.speed = defaultAnimatorSpeed * i_TimeScale;
		if( i_NavMeshAgent != null )
			i_NavMeshAgent.speed = defaultNavMeshAgentSpeed * i_TimeScale;
		if( i_AudioSource != null )
			i_AudioSource.pitch = defaultAudioSourcePitch * i_TimeScale;
		if( i_ParticleSystem != null )
		{
			var a = i_ParticleSystem.main;
			a.simulationSpeed = defaultParticleSystemSpeed * i_TimeScale;
		}
	}
}