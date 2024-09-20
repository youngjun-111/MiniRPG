using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    //��ǥ�� �Է��� üũ�� ������ ������ �Է��� ������ �װ��� �̺�Ʈ�� ���ĸ� ���ִ� �������� ����(������ ����)
    //�̷��� �ϸ� �÷��̾� ��Ʈ�ѷ��� 100���� �ǵ� 1000���� �ǵ� �� �������� �ѹ����� üũ�ذ����� �� �̺�Ʈ�� �����ϴ¹������ ���� �� ����.
    //�̷��� �����ϸ� �� ���� ���� ��� Ű���� �Է��� �޾Ҵ��� ã�Ⱑ ����� ������ �ؼҵȴ�.

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if (MouseAction != null)
        {
            //�������� ���
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)//������ ������ ���ų�
                {
                    //ó�� ������ �Ǿ�����
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else //Ŭ���� ��� (���࿡ �ѹ��̶� �������� ������ click �̶�� �̺�Ʈ �߻�)
            {
                if (_pressed)
                {
                    //�������� �Ǿ��µ� �� �ð��� Ŭ�� ����
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    //������ �Ǿ��� �ð��� Ŭ�� �ð� �̻��̸�
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }

        //�̰� ������ ������? ������? ������? ������? �� �ƴ϶� �� �ȴ������� ��� �����ϰ� �ִٰ� ������ �� ������! �ϴ°�
        //�ȴ����� ���������� ���������� ���� ������
        //if (Input.anyKey == false)
        //    return;

        ////�۵� ����ϻ�
        //if (KeyAction != null)
        //    KeyAction.Invoke();

        //�ϳ��� Form�� �ٸ� thread���� �����ϰ� �� ��쿡 ������ Form�� �浹�� �� �� �ִ�.
        //�� �� invoke�� ����Ͽ� �����Ϸ��� �ϴ� �޼ҵ��� �븮��(delegate)�� �����Ű�� �ȴ�.

    }
    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
