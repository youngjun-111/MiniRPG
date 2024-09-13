using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;
        //Temp_유니티짱 5개 생성 하고 5개 제외
        //List<GameObject> list = new List<GameObject>();
        //for (int i = 0; i < 5; i++)
        //{
        //    list.Add(Managers.Resources.Instantiate("unitychan"));
        //}

        //foreach(GameObject obj in list)
        //{
        //    Managers.Resources.Destroy(obj);
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //그냥 씬을 로드하던걸
            //SceneManager.LoadScene("Game");
            //매니저로 연결해줘서 로드해주기 위해
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }

    public override void Clear()
    {
        Debug.Log("로그인 성공");
    }
}
