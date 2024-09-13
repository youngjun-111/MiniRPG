using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Util
{
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        // T FindChild<T>
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
        {
            return null;
        }
        return transform.gameObject;
    }

    // 최상위 부모, 이름은 비교하지 않고 그 타입에만 해당하면 리턴 ( 컴퍼넌트 이름 ),
    // 재귀적으로 사용, 자식만 찾을건지 자식의 자식도 찾을 건지
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false)
    where T : UnityEngine.Object
    {
        //최상위 객체가 null일 경우
        if (go == null)
        {
            return null;
        }

        if (recursive == false) // 직속 자식만
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                //비어있거나 이름이 같으면 즉, 이름이 틀리지만 않는다면
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    //그 직속 자식의  컴포넌트를 가져와
                    T component = transform.GetComponent<T>();
                    //그 컴포넌트값이 널이 아니면
                    if (component != null)
                    {
                        //그 컴포넌트를 반환해줘
                        return component;
                    }
                }
            }
        }
        else// 자식의 자식까지
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {   // 이름이 비어있거나 내가 찾으려는 이름과 같다면
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }
        return null;
    }

    // 게임오브젝트(go)에 해당 컴포넌트가 없으면 T 컴포넌트 추가
    //프리팹을 넣을 필요가 없어지고 컴포넌트를 일일이 붙여줄 필요가 없어진다.
    //즉, 타입이 컴포넌트라면
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }
}
