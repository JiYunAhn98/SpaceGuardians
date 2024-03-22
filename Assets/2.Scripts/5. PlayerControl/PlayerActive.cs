using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

namespace PlayerActive
{
	#region [ 기본 동작 ]
	public class PlayerIdle : DestroySingleton<PlayerIdle>, IFSMState<Player>
	{
		public void Enter(Player e)
		{
			e.ChangeCharacterAnimation(eAnimState.Idle);
		}

		public void Execute(Player e)
		{
			switch (SceneControlManager._instance._currentScene)
			{
				case eSceneName.Stadium:
					if (StadiumManager._instance._isCountStart)
						e.ChangeState(PlayerRun._instance);
					return;
                default:
					if (e._moveSideMode)
					{
						if (e._isJump)
						{
							e.ChangeState(PlayerJump._instance);
						}
						else if(e._rigidCtr.velocity.y < 0)
						{
							e.ChangeState(PlayerFall._instance);
						}
						else if (e._moveDir != Vector3.zero)
							e.ChangeState(PlayerRun._instance);
					}
                    else
					{
						if (e._isRoll)
						{
							e.ChangeState(PlayerRoll._instance);
						}
						else if (e._moveDir != Vector3.zero)
							e.ChangeState(PlayerRun._instance);
					}
                    break;
            }

		}

		public void Exit(Player e)
		{
		}
	}
	public class PlayerRun : DestroySingleton<PlayerRun>, IFSMState<Player>
	{
		public void Enter(Player e)
		{
			e.ChangeCharacterAnimation(eAnimState.Run);
		}

		public void Execute(Player e)
		{
			switch (SceneControlManager._instance._currentScene)
			{
				case eSceneName.Stadium:
					if (!StadiumManager._instance._isCountStart)
					{
						e.ChangeState(PlayerIdle._instance);
					}
					break;
				default:
					e.MoveToDir();
					if (e._moveSideMode)
					{
						if (e._isJump)
						{
							e.ChangeState(PlayerJump._instance);
						}
						else if(e._isSlide)
						{
							e.ChangeState(PlayerSlide._instance);
						}
						else if (e._rigidCtr.velocity.y < 0)
						{
							e.ChangeState(PlayerFall._instance);
						}
						else if (e._moveDir.magnitude <= 0)
						{
							e._rigidCtr.velocity = Vector2.zero;
							e.ChangeState(PlayerIdle._instance);
						}
					}
                    else
					{
						if (e._isRoll)
						{
							e.ChangeState(PlayerRoll._instance);
						}
						else if(e._moveDir.magnitude <= 0)
                        {
                            e._rigidCtr.velocity = Vector2.zero;
                            e.ChangeState(PlayerIdle._instance);
                        }
                    }
                    break;
			}
		}

		public void Exit(Player e)
		{
		}
	}
	public class PlayerHit : DestroySingleton<PlayerHit>, IFSMState<Player>
	{
		float _time = 0;
		public void Enter(Player e)
		{
			e._isHit = true;
			e.ChangeCharacterAnimation(eAnimState.Hit);
			SoundManager._instance.PlayEffect(eEffectSound.PlayerHit);

			_time = 0;

			switch (SceneControlManager._instance._currentScene)
			{
				case eSceneName.JumpClimb:
					e._floorTriggerCtr.enabled = false;
					break;
				case eSceneName.EscapeRoom:
					GameObject.FindGameObjectWithTag("Manager").GetComponent<EscapeRoomManager>().ProgHeal();
					e._isHit = false;
					break;
			}
		}

		public void Execute(Player e)
		{
			_time += Time.deltaTime;
			switch (SceneControlManager._instance._currentScene)
			{
				case eSceneName.JumpClimb:
					if (_time >= 2)
					{
						e.ChangeState(PlayerFall._instance);
					}
					break;
				case eSceneName.SprintRace:
					if (_time >= 5)
					{
						e.ChangeState(PlayerIdle._instance);
					}
					break;
			}
		}

		public void Exit(Player e)
		{
			e._isHit = false;
			PlayerJump._instance._wasJump = false;
			PlayerDoubleJump._instance._wasDoubleJump = false;

			switch (SceneControlManager._instance._currentScene)
			{
				case eSceneName.JumpClimb:
					e._floorTriggerCtr.enabled = true;
					break;
			}
		}
	}
	public class PlayerDeath : DestroySingleton<PlayerDeath>, IFSMState<Player>
	{
		public void Enter(Player e)
		{
			e._isDeath = true;
			e.ChangeCharacterAnimation(eAnimState.Death);
			SoundManager._instance.PlayEffect(eEffectSound.PlayerDeath);
			e._rigidCtr.velocity = Vector3.zero;

			// 죽었을 때 떨어지는 모습을 원한다면 stiar_Basic을 달아주고 해당 발판은 모두 stair로 표현하자
			e._colCtr.enabled = false;
			e._floorTriggerCtr.enabled = false;
		}
		public void Execute(Player e)
		{
		}
		public void Exit(Player e)
		{
		}
	}
	#endregion [ 기본 동작 ]

	#region [ MoveSideMode 아닐 시 동작 ]
	public class PlayerRoll : DestroySingleton<PlayerRoll>, IFSMState<Player>
	{
		float _currRollingTimeVal;
		Vector3 _rollDir;
		public void Enter(Player e)
		{
			e.ChangeCharacterAnimation(eAnimState.Roll);
			SoundManager._instance.PlayEffect(eEffectSound.PlayerRoll);
			_currRollingTimeVal = 0;
			_rollDir = (e._moveDir == Vector3.zero) ? e._lastMovDir : e._moveDir;
		}

		public void Execute(Player e)
		{
			e.Roll(_rollDir);

			if (_currRollingTimeVal >= 0.3f)
				e.ChangeState(PlayerIdle._instance);
			else
			{
				_currRollingTimeVal += Time.deltaTime;
			}
		}

		public void Exit(Player e)
		{
			e._isRoll = false;
			e._moveDir = Vector2.zero;
		}
	}
	#endregion [ MoveSideMode 아닐 시 동작 ]

	#region [ MoveSideMode 시 동작 ]
	public class PlayerSlide : DestroySingleton<PlayerSlide>, IFSMState<Player>
	{
		public void Enter(Player e)
		{
			SoundManager._instance.PlayEffect(eEffectSound.PlayerRoll);
			e.ChangeCharacterAnimation(eAnimState.Slide);
			e._rigidCtr.velocity = Vector2.down * 20;
			e._floorTriggerCtr.offset += new Vector2(0.05f, 0);
		}

		public void Execute(Player e)
		{
			if (!e._isSlide)
			{
				if (e._isJump && !PlayerJump._instance._wasJump)
				{
					e.ChangeState(PlayerJump._instance);
				}
				else if (e._isDoubleJump && !PlayerDoubleJump._instance._wasDoubleJump)
				{
					e.ChangeState(PlayerDoubleJump._instance);
				}
				else
				{
					e._isJump = false;
					e.ChangeState(PlayerFall._instance);
				}
			}

			if (e._rigidCtr.IsTouchingLayers() && (e._rigidCtr.velocity.y > 0))
			{
				e._rigidCtr.velocity = Vector2.zero;
			}

			e.MoveToDir();

		}

		public void Exit(Player e)
		{
			e.transform.position += Vector3.up * 0.6f;
			e._floorTriggerCtr.offset -= new Vector2(0.05f, 0);
		}
	}
	public class PlayerJump : DestroySingleton<PlayerJump>, IFSMState<Player>
	{
		public bool _wasJump { get; set; }
		public void Enter(Player e)
		{
			e.ChangeCharacterAnimation(eAnimState.Jump);
			e._rigidCtr.velocity = Vector3.zero;
			e.Jump();
			_wasJump = true;
			e._isJump = false;
			SoundManager._instance.PlayEffect(eEffectSound.PlayerJump);
		}

		public void Execute(Player e)
		{
			if (e._rigidCtr.velocity.y <= 0)
			{
				e.ChangeState(PlayerFall._instance);
			}
			else if(e._isJump)
			{
				e.ChangeState(PlayerDoubleJump._instance);
			}
			else if(e._isSlide)
			{
				e.ChangeState(PlayerSlide._instance);
			}
			e.MoveToDir();
		}
		public void Exit(Player e)
		{
		}
	}
	public class PlayerDoubleJump : DestroySingleton<PlayerDoubleJump>, IFSMState<Player>
	{
		public bool _wasDoubleJump { get; set; }
		public void Enter(Player e)
		{
			if (SceneControlManager._instance._currentScene == eSceneName.Galaxy)
			{
				e.transform.localPosition = Vector2.zero;
				e._floorTriggerCtr.enabled = false;
				e.ChangeSortingLayer("Default");
			}

			e.ChangeCharacterAnimation(eAnimState.Roll);
			SoundManager._instance.PlayEffect(eEffectSound.PlayerJump);
			e._rigidCtr.velocity = Vector3.zero;
			e.Jump();
			_wasDoubleJump = true;
			e._isJump = false;
		}

		public void Execute(Player e)
		{
			if (e._rigidCtr.velocity.y < 0)
			{
				e._isFall = true;
				e.ChangeState(PlayerFall._instance);
			}
			if (e._moveSideMode)
			{
				if (e._isSlide)
				{
					e.ChangeState(PlayerSlide._instance);
				}
				e.MoveToDir();
			}
		}

		public void Exit(Player e)
		{
			if (SceneControlManager._instance._currentScene == eSceneName.Galaxy)
				e.ChangeSortingLayer("Player");
		}
	}
	public class PlayerFall : DestroySingleton<PlayerFall>, IFSMState<Player>
	{
		public void Enter(Player e)
		{
			e.ChangeCharacterAnimation(eAnimState.Fall);

			e._isFall = true;
		}

		public void Execute(Player e)
		{
			e.MoveToDir();

			if (e._isJump && !PlayerJump._instance._wasJump)
			{
				e.ChangeState(PlayerJump._instance);
			}
			else if (e._isJump && !PlayerDoubleJump._instance._wasDoubleJump)
			{
				e.ChangeState(PlayerDoubleJump._instance);
			}
			else if (e._isSlide)
			{
				e.ChangeState(PlayerSlide._instance);
			}
			else if (e._rigidCtr.IsTouchingLayers() && (e._rigidCtr.velocity.y >= 0))
			{
				PlayerJump._instance._wasJump = false;
				PlayerDoubleJump._instance._wasDoubleJump = false;

				if (e._moveDir.magnitude <= 0)
					e.ChangeState(PlayerIdle._instance);
				else
					e.ChangeState(PlayerRun._instance);
			}
			else
			{
				e._isJump = false;
				e._isSlide = false;
			}

		}

		public void Exit(Player e)
		{
			e._rigidCtr.velocity = Vector3.zero;

			e._isFall = false;
		}
	}
	#endregion [ MoveSideMode 시 동작 ]

}
