using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineHelper
{
    // Scene 관련 enums
    public enum eSceneName             // Scene 이름들
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

    // 캐릭터 enum
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

    #region [Character 행동 관련]
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
    #endregion [Character 행동 관련]

    #region [각 Game Stage 상황]
    public enum eGameState
    {
        Init,
        Ready,
        Play,
        Event,
        End,
        Result
    }

    #endregion [각 Game Stage 상황]

    #region [사운드]
    public enum eEffectSound
    {
        // Lobby
        LobbyHover,         // Hover시 나오는 소리

        // Galaxy
        SwingPlanet,        // 행성 넘길때 나오는 소리
        MusicOn,            // 소리를 킬 때 나오는 소리
        MusicOff,           // 소리를 끌 때 나오는 소리

        // SpaceShip
        WindowOpen,       // 게임 선택 창이 열릴 때 나오는 소리
        WindowClose,

        // Stadium
        AlarmTime,          // 1초당 시간 알려주는 소리
        CharacterPick,    // 캐릭터 변환 시 소리
        TimerStart,         // Ready 하는 소리
        TimerStop,          // Ready 푸는 소리

        // Loading
        BoxDown,            // Box Down 소리
        BoxUp,              // Box Up 소리


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
    #endregion [사운드]

    #region[UI]

    public enum ePopup
    {
        ResultWnd,

        Count
    }

    #endregion[UI]
}
