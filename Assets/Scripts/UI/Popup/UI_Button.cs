using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    [SerializeField]
    Text _text;

    int _score = 0;

    //사용할 UI이름을 enum으로 생성(필요 시 추가)
    //버튼은 버튼enum으로 관리 텍스트는 텍스트enum으로 관리
    enum Buttons
    {
        PointButton,
    }
    enum Texts
    {
        PointText,
        ScoreText,
    }
    enum Images
    {
        Image,
    }
    enum GameObjects
    {
        TestObject,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        //똑같이 Bind해서 타입으로 오브젝트를 찾아줌
        Bind<GameObject>(typeof(GameObjects));

        //이부분을 UI_Base에서 함수형태로 만들어 준다.
        //UI_EventHandler evt = go.GetComponent<UI_EventHandler>();
        //evt.OnDragHandler += ((PointerEventData data) => { go.transform.position = data.position; });

        //이거는 클릭 시 적용될 함수
        //즉, 버튼 누를 때
        GetButton((int)Buttons.PointButton).gameObject.BindUIEvent(OnButtonClicked);

        //이거는 드래그 하여 이미지를 옮길려고 하는 함수
        //즉, 이미지 클릭하여 옮길 때
        GameObject go = GetImage((int)Images.Image).gameObject;
        BindUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }
    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}";
    }

    //UI_Base에 옮겨진것들임 상속해줬음
    //오브젝트는 컴포넌트가 아니기에 따로 만들어준 함수
    //test일뿐 굳이 사용하지 않는다 UI에서 특히하게 GameObject를 사용하는 일이 있을경우 쓸것이다.
    //public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    //{
    //    Transform transform = FindChild<Transform>(go, name, recursive);
    //    if (transform == null)
    //    {
    //        return null;
    //    }
    //    return transform.gameObject;
    //}

    ////컴포넌트에 연결해줄 함수 형태(UI_Base로 이동)
    //Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();//Type은 using System필요
    //void Bind<T>(Type type) where T : UnityEngine.Object
    //{
    //    string[] names = Enum.GetNames(type);

    //    UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
    //    _objects.Add(typeof(T), objects);

    //    for (int i = 0; i < names.Length; i++)
    //    {
    //        //아래 Util에서 만든 함수를 이용해 처리
    //        if (typeof(T) == typeof(GameObject))
    //        {
    //            objects[i] = Util.FindChild(gameObject, names[i], true);
    //        }
    //        else
    //        {
    //            objects[i] = Util.FindChild<T>(gameObject, names[i], true);
    //        }
    //        //잘찾아주고 있는지 체크
    //        if (objects[i] == null)
    //        {
    //            Debug.Log($"Failed to bind({names[i]})");
    //        }
    //    }
    //}

    //T Get<T>(int idx) where T : UnityEngine.Object
    //{
    //    UnityEngine.Object[] objects = null;

    //    //값이 없으면 그냥 리턴(키가 포함되어 있는지 확인)
    //    if (_objects.TryGetValue(typeof(T), out objects) == false)
    //        return null;

    //    //오브젝츠 인덱스 번호를 추출한 다음에 T로 캐스팅 해줌
    //    return objects[idx] as T;
    //}

    ////기존에서 한겹 더 랩핑해서 중간단계를 날리는 용도
    ////즉, 자주 사용하는 것들은 Get<T>울 이용하지 않고 바로 사용할 수 있게 만들어 둔 것.
    //Text GetText(int idx) { return Get<Text>(idx); }
    //Button GetButton(int idx) { return Get<Button>(idx); }
    //Image GetImage(int idx) { return Get<Image>(idx); }
}
