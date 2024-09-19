using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    Texture2D _attackIcon;
    Texture2D _handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType _cursorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resources.Load<Texture2D>("Texture/Cursor/Attack");
        _handIcon = Managers.Resources.Load<Texture2D>("Texture/Cursor/Hand");
    }
    void Update()
    {
        UpdateMouseCursor();
    }

    void UpdateMouseCursor()
    {
        //마우스를 누르고 있으면 변경처리를 하지 않고 그냥 빠져나가면
        //락온 한 상태에서 좌표가 이동돼도 커서가 그대로되어 있게
        if (Input.GetMouseButtonDown(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
                else
                {
                    if (_cursorType != CursorType.Hand)
                    {
                        Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                        _cursorType = CursorType.Hand;
                    }
                }
            }
        }
    }
}
