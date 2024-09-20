using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    //GameScene가 꺼져 있어도 작동 시키기 위해 Awake로 변경
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        //Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int, Data.Stat> Dict = Managers.Data.StatDict;
        //Temp_유니티쨩 2개 생성
        //for (int i = 0; i < 2; i++)
        //{
        //    Managers.Resources.Instantiate("unitychan");
        //}
        gameObject.GetOrAddComponent<CursorController>();
        //게임 매니저를 통해 생성을 시켜줌 content영역
        //플레이어를 생성
        GameObject player = Managers.game.Spawn(Define.WorldObject.Player, "unitychan");
        //생성된 플레이어에게 카메라를 붙여줌
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        //몬스터를 소환 해줌
        Managers.game.Spawn(Define.WorldObject.Monster, "Knight");
    }

    public override void Clear()
    {

    }
}
