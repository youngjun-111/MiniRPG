using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    //대표로 입력을 체크한 다음에 실제로 입력이 있으면 그것을 이벤트로 전파를 해주는 형식으로 구현(리스너 패턴)
    //이렇게 하면 플레이어 컨트롤러가 100개가 되든 1000개가 되든 이 루프마다 한번씩만 체크해가지고 그 이벤트를 전파하는방식으로 구현 된 것임.
    //이렇게 관리하면 또 좋은 것은 어디서 키보드 입력을 받았는지 찾기가 어려운 문제가 해소된다.

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
            //프레스일 경우
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)//프레스 된적이 없거나
                {
                    //처음 프레스 되었을때
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else //클릭일 경우 (만약에 한번이라도 프레스를 했으면 click 이라는 이벤트 발생)
            {
                if (_pressed)
                {
                    //프레스는 되었는데 그 시간이 클릭 정도
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    //프레스 되었던 시간이 클릭 시간 이상이면
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }

        //이게 없으면 눌렀어? 눌렀어? 눌렀어? 눌렀어? 가 아니라 아 안눌렀구나 라고 생각하고 있다가 누르면 아 눌렀네! 하는거
        //안눌렀음 빠져나오고 눌렀을때는 실행 시켜줘
        //if (Input.anyKey == false)
        //    return;

        ////작동 방법일뿐
        //if (KeyAction != null)
        //    KeyAction.Invoke();

        //하나의 Form을 다른 thread에서 접근하게 될 경우에 기존의 Form과 충돌이 날 수 있다.
        //이 때 invoke를 사용하여 실행하려고 하는 메소드의 대리자(delegate)를 실행시키면 된다.

    }
    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
