using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum CameraMode
    {
        //ī�޶� ��尡 �������� ��� ���⼭ �߰������� �߰����ָ� �ȴ�.
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
        //���� ����
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
        MaxCount,//���� ������ ������ �������ؼ� �߰�
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
