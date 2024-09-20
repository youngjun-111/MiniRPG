using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum CameraMode
    {
        //카메라 모드가 여러개일 경우 여기서 추가적으로 추가해주면 된다.
        QuarterView, testView,
    }

    public enum MouseEvent
    {
        Press, 
        PointerDown,
        PointerUp,
        Click,
    }

    public enum UIEvent
    {
        //이하 동일
        Click, Drag,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,//현재 사운드의 갯수를 새기위해서 추가
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }
}
