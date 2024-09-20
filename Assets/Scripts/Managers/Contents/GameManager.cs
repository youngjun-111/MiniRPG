using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    //ContentManager
    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    //int에 할당
    public Action<int> OnSpawnEvent;

    //플레이어를 하나만 생성해주기위한 프로퍼티
    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resources.Instantiate(path, parent);
        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                 if(OnSpawnEvent != null)
                {
                    //1을 전달해서 구독자에게 1마리 생성 됐다고 전달
                    OnSpawnEvent.Invoke(1);
                }
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }
        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
        {
            return Define.WorldObject.Unknown;
        }
        return bc.WorldObjetTpye;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);
        switch (type)
        {
            case Define.WorldObject.Monster:
                if (_monsters.Contains(go))
                {
                    _monsters.Remove(go);
                    if(OnSpawnEvent != null)
                    {
                        //-1을 전달해서 구독자에게 1마리 제거 했다고 알려줌
                        OnSpawnEvent.Invoke(-1);
                    }
                }
                break;
            case Define.WorldObject.Player:
                if (_player == go)
                {
                    _player = null;
                }
                break;
        }
        Managers.Resources.Destroy(go);
    }
}
