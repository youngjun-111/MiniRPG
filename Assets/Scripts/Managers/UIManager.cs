using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;//혹시 모르니깐 여유를 두고 먼저 생성할게 있다면 10보다 작은 수로 팝업할 수 있게 함.
    //가장 마지막에 듸운 팝업이 가장 먼저 사라져야하기 때문에
    //Stack맨 마지막에 들어간게 나오는 것 쌓이고 쌓이는것
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    //팝업 UI를 생성해주는 함수(매개변수의 타입을 미리 결정하지 않고, 사용 시 결정)
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        //T눈 아무 T나 받는게 아니라 무조건 UI팝업을 상속받는 애로 만들자
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = Managers.Resources.Instantiate($"UI/Popup/{name}");//팝업 생성
        T popup = Util.GetOrAddComponent<T>(go);//컴퍼넌트가 붙어있지 않다면 추가
        _popupStack.Push(popup);

        //프로퍼티로 생성 해줌
        //GameObject root = GameObject.Find("@UI_Root");
        //if(root == null)
        //{
        //    root = new GameObject { name = "@UI_Root" };
        //}
        go.transform.SetParent(Root.transform);

        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        //T눈 아무 T나 받는게 아니라 무조건 UI팝업을 상속받는 애로 만들자
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = Managers.Resources.Instantiate($"UI/Scene/{name}");//팝업 생성
        T sceneUI = Util.GetOrAddComponent<T>(go);//컴퍼넌트가 붙어있지 않다면 추가
        _sceneUI = sceneUI;
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        //캔바스 추출
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        //랜더모드는 무조건 ScreenSpaceOverlay(이경우만 소팅되기 때문이)
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //캔버스 안에 캔버스가 중첩해서 있을 때 그 부모가 어떤 값을 가지던 자신은 무조건 내 소팅 오더를 가질꺼야
        //overrideSorting을 통해 혹시라도 중첩캔버스라 자식 캔버스가 있더라도 부모 캔버스가 어떤 값을 가지던
        //나는 내 오더 값을 가지려 할때 true
        canvas.overrideSorting = true;
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;

        }
        else//팝업이랑 상관없는 일반 UI
        {
            canvas.sortingOrder = 0;
        }
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)//Stack에 아무것도 없으면 그냥 리턴
        {
            return;
        }

        UI_Popup popup = _popupStack.Pop();
        Managers.Resources.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }
    public void ClosePopupUI(UI_Popup popup)//삭제할것을 지정할 수 있으니 좀더 안전하게 삭제할 수 있다.
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        //맨 위에서 개체를 제거하지 않고 반환 (꺼내지 않고 엿본다고 생각)
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }
        ClosePopupUI();
    }
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resources.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        return Util.GetOrAddComponent<T>(go);
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
