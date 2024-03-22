using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T> : MonoBehaviour
{
	T owner;    //	���� ������..
	IFSMState<T> beforeState = null;   //	���� ����..
	IFSMState<T> currentState = null;   //	���� ����..
	IFSMState<T>[] states;

	public IFSMState<T> BeforetState { get { return beforeState; } }
	public IFSMState<T> CurrentState { get { return currentState; } }

	/// <summary>
	/// �ʱ� ������ �Ѵ�.
	/// </summary>
	/// <param name="owner"> ���� ������ ������ </param>
	/// <param name="initialState"> ���� ���� </param>
	public void InitState(T owner, IFSMState<T> nowstate)
	{
		this.owner = owner;
		ChangeState(nowstate);
	}

	/// <summary>
	/// ���� ������ Exit�� �����ϰ� ���� ������ ������ Enter�� ����.
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
	/// ���� ������ Execute�� �����Ѵ�. Controller�� Update���� ��������.
	/// </summary>
	public void FSMUpdate()
	{
		if (currentState != null) currentState.Execute(owner);
	}
}