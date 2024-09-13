using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }
        Stack<Poolable> _poolStack = new Stack<Poolable>();

        //생성 시켜줌
        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name} _Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        //풀을 생성하고 컴포넌트를 붙여주는 함수(Clone)이라는 이름은 삭제하기 위해 name은 ResourceManager에서
        //Clone을 삭제시켜준 오리지널로 지정해줌
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            //이름에Clone이 안붙게
            go.name = Original.name;
            //컴포넌트를 붙이고 반환
            return go.GetOrAddComponent<Poolable>();
        }

        //풀에 넣어주는 함수
        public void Push(Poolable poolable)
        {
            if (poolable == null)
            {
                return;
            }
            //부모를 아까 지정해준 @Pool_Root의 자식으로 생성
            poolable.transform.parent = Root;
            //StackPush로 1개씩 꺼줌
            poolable.gameObject.SetActive(false);
            //지정해준 풀링이 됐는지 안됐는지 확인
            poolable.isUsing = false;

            _poolStack.Push(poolable);

        }

        //풀에 넣은걸 뽑아오는 함수
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            //Stack에 담아준 오브젝트가 0보다 클 경우에 꺼내서 쓸것이고
            if (_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
            }
            //근데 없으면 1개 생성 시켜줘
            else
            {
                poolable = Create();
            }
            //활성화 시켜주고
            poolable.gameObject.SetActive(true);
            //돈디스트로이온로드에서 빼주고 현재 씬에서 생성 시켜주는 방법
            if (parent == null)
            {
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            }
            //부모를 아까 지정해준 @Pool_Root의 자식으로 생성
            poolable.transform.parent = parent;
            //지정해준 풀링이 됐는지 안됐는지 확인
            poolable.isUsing = true;
            return poolable;
        }
    }
    #endregion

    #region Pool을 Dictionary로 사용
    //각각 풀들은 스트링이라는 키를 이용해 이름(string)을 이용해서 관리
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;
    public void Init()
    {
        if (_root == null)
        {
            //풀링을 할 오브젝트가 있다면 @Pool_Root 산하에 들고 있게 끔 할 예정
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }
    //실제 기능 하도록
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }
        return _pool[original.name].Pop(parent);
    }

    //실제 기능을 하도록 class에서 생성자를 통해 딕셔너리를 생성시켜줌
    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public GameObject GetOriginal(string name)
    {
        //사실 1개만 생성 하고, 생성했던 그 1개를 복사해서 더욱 최적화 시키는?
        //근데 외부적으로 보이는건 별반 차이가 없음
        if (_pool.ContainsKey(name) == false)
        {
            return null;
        }

        return _pool[name].Original;
    }

    //생성 Pop()을 한번도 안하고 풀이 없는 상태에서 Push를 하는 상태
    //에디터에서 드래그엔 드롭으로 뭔가 생성 했을 경우를 대비해서
    //예외적으로 처리할 사항
    //정상적인 루트로 생성된 애는 비활성화 시키는데. 비정상적으로 생성된 애가 생성되고 poolable도 있기도하고
    //그런데 오브젝트풀링으로 사전에 생성 시켜준 애가 아니면 그애는 poolable이 있어도 삭제 시켜버려줘
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (_pool.ContainsKey(name) == false)
        {
            //풀에 넣지 않고 삭제...
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    //Clear 씬이 변결될 때 애 써서 넣어놓은 것을 날려줘야 할지 아니면 그대로 유지되어야 할지 게임마다 다르지만
    //(씬마다 사용하는 오브젝트가 너무다 다르다 싶으면 날려주는 기능이 있어야 하니)
    public void Clear()
    {
        foreach (Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }
        _pool.Clear();
    }
    #endregion
}
