using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    private void Start()
    {
        Init();
    }
    //상속 받는애만 쓰게하기 위해 protected로 만들어줌
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    //컴포넌트에 연결해줄 함수 형태를 만들자.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects); // Dictionary 에 추가

        // T 에 속하는 오브젝트들을 Dictionary의 Value인 objects 배열의 원소들에 하나하나 추가
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
                Debug.Log($"Failed to bind({names[i]})");
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        //값이 없으면 그냥 리턴
        if (_objects.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        //오브젝츠 인덱스 번호를 추출한 다음에 T로 캐스팅 해줌
        return objects[idx] as T;
    }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    //이런 형태이면 될듯 지금은 OnDragHandler 이것이지만 다른것도 사용하게 될테니까 이넘 타입으로 만들어 둠
    //go : Action이 들어가 있는 스크립트가 있는 게임 오브젝트...
    //action : 구독을 시킬 액션
    //mode(하고싶은)
    public static void BindUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
        evt.OnDragHandler += ((PointerEventData data) => { evt.transform.position = data.position; });

        switch (type)
        {
            //타입이 클릭이라면 드래그는 구독 취소
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            //타입이 드래그라면 클릭은 구독 취소
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}


