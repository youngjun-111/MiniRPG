using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    //현재 씬 상태는 Unknown으로 지정
    //Define.Scene _sceneType = Define.Scene.Unknown;
    //다른곳에서 사용하기 위해 프로퍼티로 생성
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    protected virtual void Init()
    {
        //타입으로 오브젝트를 찾아보고
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        //찾을 오브젝트가 없으면 
        //경로 Resoureces -> Prefabs -> UI -> EventSystem 을 생성시키고 이름을 @EventSystem으로 설정
        if (obj == null)
        {
            Managers.Resources.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    //여기서 당장 정의해 주지 않을 거라 abstract로
    public abstract void Clear();
}
