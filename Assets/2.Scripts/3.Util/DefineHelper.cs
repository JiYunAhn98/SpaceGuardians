using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineHelper
{
    // Scene ���� enums
    public enum eSceneName             // Scene �̸���
    {
        Start = -6,
        Lobby,
        Stadium,
        SpaceShip,
        Galaxy,
        StadiumResult,

        LaserRoom = 0,
        NeverStop,
        JumpClimb,

        EscapeRoom,
        CleanRoom,
        SprintRace,


        Count
    }

    // ĳ���� enum
    public enum eCharacterName
    {
        FFrogg,
        Burn,
        Toroko,
        Chemi,

        Count
    }
    public enum ePlanetName
    {
        SSeaa,
        Hoateta,
        Noasis,
        Radiocty,

        Count
    }
    public enum eNeverStopBlocks
    {
        EmptyMiddleBlock,
        TurtleMiddleBlock,
        ThornUpBlock, 
        RabbitBlock,
        BirdMiddleBlock,
        FinishBlock,
        DoubleFinishBlock,

        Count
    }

    #region [Character �ൿ ����]
    public enum eAnimState
    {
        Idle,
        Run,
        Jump,
        Roll,
        Fall,
        Slide,

        Hit = 998,
        Death = 999,
    }
    #endregion [Character �ൿ ����]

    #region [�� Game Stage ��Ȳ]
    public enum eGameState
    {
        Init,
        Ready,
        Play,
        Event,
        End,
        Result
    }

    #endregion [�� Game Stage ��Ȳ]

    #region [����]
    public enum eEffectSound
    {
        // Lobby
        LobbyHover,         // Hover�� ������ �Ҹ�

        // Galaxy
        SwingPlanet,        // �༺ �ѱ涧 ������ �Ҹ�
        MusicOn,            // �Ҹ��� ų �� ������ �Ҹ�
        MusicOff,           // �Ҹ��� �� �� ������ �Ҹ�

        // SpaceShip
        WindowOpen,       // ���� ���� â�� ���� �� ������ �Ҹ�
        WindowClose,

        // Stadium
        AlarmTime,          // 1�ʴ� �ð� �˷��ִ� �Ҹ�
        CharacterPick,    // ĳ���� ��ȯ �� �Ҹ�
        TimerStart,         // Ready �ϴ� �Ҹ�
        TimerStop,          // Ready Ǫ�� �Ҹ�

        // Loading
        BoxDown,            // Box Down �Ҹ�
        BoxUp,              // Box Up �Ҹ�


        // PlayerActive
        PlayerJump,
        PlayerHit,
        PlayerDeath,
        PlayerRoll,

        // NeverStop
        SpeedUp,

        // LaserRoom
        LaserShoot,
        GageAlarm,

        // EscapeRoom
        MazeChange,

        // CleanRoom
        TorokoShoot,
        TorokoBoom,
        FFroggShoot,
        FFroggBoom,
        ChemiShoot,
        ChemiBoom,
        BurnShoot,
        BurnBoom,
        SlimeDevide,

        // StadiumResult
        RoundResult,
        TotalResult,

        Count
    }
    #endregion [����]

    #region[UI]

    public enum ePopup
    {
        ResultWnd,

        Count
    }

    #endregion[UI]
}
