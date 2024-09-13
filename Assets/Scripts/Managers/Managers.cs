using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Managers : MonoBehaviour
{
    //유일한 매니저를 갖고온다. // 프로퍼티 // 읽기 전용 // 은닉성 // 싱글톤 프로퍼티
    //매니저를 관리하는 매니저
    //싱글톤을 은닉하고 프로퍼티로하여 사용 하기 위해 private로 만들어줌
    static Managers s_instance;//유일성이 보장된다.
    public static Managers Instance { get { Init(); return s_instance; } }//읽기 전용 값만 불러옴
    //이제 매니저스는 자신이 뭔가를 직접하기 보단 다른 매니저들을 관리(싱글톤을 사용하게)해주는 치프매니저라고 생각하면 된다.
    //그래서 직접 외부에서 직급 할필요가 없어서 private으로 만들었음.
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();
    //플레이어 인풋 매니저
    public static InputManager Input { get { return Instance._input; } }
    //리소스 매니저
    public static ResourceManager Resources { get { return Instance._resource; } }
    //유아이 매니저
    public static UIManager UI { get { return Instance._ui; } }
    //씬 매니저
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    //사운드 매니저
    public static SoundManager Sound { get { return Instance._sound; } }
    //풀 매니저
    public static PoolManager Pool { get { return Instance._pool; } }
    //데이타 매니저
    public static DataManager Data { get { return Instance._data; } }
    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();//인풋매니저의 OnUpdate()실행
    }

    static void Init()
    {
        //널이라면 
        if (s_instance == null)
        {
            //일단 @Managers를 찾아
            GameObject go = GameObject.Find("@Managers");
            //근데 또 @Managers가 없으면
            if (go == null)//go 가 없으면
            {
                //@Managers를 코드상으로해서 빈게임 오브젝트를 만들어주고
                go = new GameObject { name = "@Managers" };
                //@Managers에다가 이 스크립트를 추가해서 생성해줌
                go.AddComponent<Managers>();
            }
            //만들어준 이 Managers는 파괴되면 안되기때문에 파괴되지않게 설정
            DontDestroyOnLoad(go);
            //s_instance라는 프로퍼티를 사용하기 위해 빈게임오브젝트를 만들어주고 스크립트를 추가해준 이 오브젝트의
            //Managers컴포넌트를 가져와줌
            s_instance = go.GetComponent<Managers>();
            //사운드오브젝트를 생성시켜주는것을 실행 시켜줌
            s_instance._sound.Init();
            //오브젝트 풀링하는 함수를 실행
            s_instance._pool.Init();
            //용량이 엄청 크다고 볼수 없고 거의 항상 갖고있을 테니 삭제는 안시켜줌
            s_instance._data.Init();
        }
    }

    //씬을 이동할 때 끄냥 Clear()를 호출하면 되지만 다른것도 클리어 해줄게 있으니 함수로 만들어줌
    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();

        //선택적클리어
        Pool.Clear();
    }
}
