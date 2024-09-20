using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static void BindUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindUIEvent(go, action, type);
    }
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
    //플레이어가 사라졋으면 오류가 나지 않게 하기위한 작업
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
