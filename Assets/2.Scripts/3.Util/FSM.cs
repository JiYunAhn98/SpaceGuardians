using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T> : MonoBehaviour
{
	T owner;    //	상태 소유자..
	IFSMState<T> beforeState = null;   //	현재 상태..
	IFSMState<T> currentState = null;   //	현재 상태..
	IFSMState<T>[] states;

	public IFSMState<T> BeforetState { get { return beforeState; } }
	public IFSMState<T> CurrentState { get { return currentState; } }

	/// <summary>
	/// 초기 설정을 한다.
	/// </summary>
	/// <param name="owner"> 현재 상태의 소유주 </param>
	/// <param name="initialState"> 현재 상태 </param>
	public void InitState(T owner, IFSMState<T> nowstate)
	{
		this.owner = owner;
		ChangeState(nowstate);
	}

	/// <summary>
	/// 현재 상태의 Exit를 실행하고 새로 시작할 상태의 Enter로 들어간다.
	/// </summary>
	/// <param name="newState"></param>
	public void ChangeState(IFSMState<T> newState)
	{
		if (currentState != null) currentState.Exit(owner);

		beforeState = currentState;
		currentState = newState;

		if (currentState != null) currentState.Enter(owner);
	}

	/// <summary>
	/// 현재 상태의 Execute를 살행한다. Controller의 Update에서 실행하자.
	/// </summary>
	public void FSMUpdate()
	{
		if (currentState != null) currentState.Execute(owner);
	}
}