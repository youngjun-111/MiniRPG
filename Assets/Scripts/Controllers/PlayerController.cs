using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerStat _stat;
    //마우스 방식으로 이동 하기위한 불 변수
    bool _moveToDest = false;

    Vector3 _destPos;

    //UI_Button uiPopup;
    //float wait_run_ratio = 0;
    NavMeshAgent navAgent;

    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }

    bool _stopSkill = false;
    //커서컴트롤러로 이동
    //Texture2D _attackIcon;
    //Texture2D _handIcon;

    //enum CursorType
    //{
    //    None,
    //    Attack,
    //    Hand,
    //}

    //CursorType _cursorType = CursorType.None;
    void Start()
    {
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        navAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
        //_attackIcon = Managers.Resources.Load<Texture2D>("Texture/Cursor/Attack");
        //_handIcon = Managers.Resources.Load<Texture2D>("Texture/Cursor/Hand");
        //키보드 이동 인풋 구독
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        //마우스 이동 인풋 구독
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        //hpbar 생성
        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        //시험 용으로 써본것들 UI는 제대로 나옴
        //for (int i = 0; i < 5; i++)
        //{
        //    uiPopup = Managers.UI.ShowPopupUI<UI_Button>();
        //}

        //uiPopup = Managers.UI.ShowPopupUI<UI_Button>();
        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    Managers.UI.CloseAllPopupUI();
        //}
        //Managers.UI.ShowSceneUI<UI_Inven>();
        //프리팹 폴더를 만들어서 UI_Button을 생성시킨다.
        //Managers.Resources.Instantiate("UI/UI_Button");
        //프리팹 폴더를 만들어서 UI_Inven을 생성 시킨다.
        //Managers.Resources.Instantiate("UI/UI_Inven");

    }

    //새로 스크립트를 작성
    //void UpdateMouseCursor()
    //{
    //    //마우스를 누르고 있으면 변경처리를 하지 않고 그냥 빠져나가면
    //    //락온 한 상태에서 좌표가 이동돼도 커서가 그대로되어 있게
    //    if (Input.GetMouseButtonDown(0))
    //        return;

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, 100f, _mask))
    //    {
    //        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
    //        {
    //            if (_cursorType != CursorType.Attack)
    //            {
    //                Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
    //                _cursorType = CursorType.Attack;
    //            }
    //            else
    //            {
    //                if(_cursorType != CursorType.Hand)
    //                {
    //                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
    //                    _cursorType = CursorType.Hand;
    //                }
    //            }
    //        }
    //    }
    //}
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    //초기 상태는 무조건 idle
    PlayerState _state = PlayerState.Idle;

    void Update()
    {
        switch (State)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;
        }
        //키보드 방식이긴 하지만 키입력값을 1과 0으로 줘서 애니메이션을 재생 하려고 할때 쓰인 코드
        //현재는 파라미터를 이용하여 재생하며 _state를 사용하여 상태를 구분지어 줘야하기때문에 필요 없어짐
        //if (_moveToDest)
        //{
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10f * Time.deltaTime);
        //    anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //    anim.Play("RUN");
        //}
        //else
        //{
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10f * Time.deltaTime);
        //    anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //    anim.Play("WAIT");
        //}
    }

    void UpdateDie()
    {
        //아무것도 못하게
    }

    void UpdateMoving()
    {
        //몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        if (_moveToDest)
        {
            //방향
            Vector3 dir = _destPos - transform.position;
            //거리 distance
            if (dir.magnitude < 0.1f)
            {
                State = PlayerState.Idle;
                _moveToDest = false;
            }
            else
            {
                float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
                //블록레이어 라면 idle로 변경 해줌
                //즉, 이동 할 수 없는 구역이니까 멈춤
                if (Physics.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Block")))
                {
                    if (Input.GetMouseButton(0) == false)
                    {
                        State = PlayerState.Idle;
                    }
                    return;
                }
                //transform.position += dir.normalized * moveDist;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
                navAgent.Move(dir.normalized * moveDist);
                Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            }
            //Animator anim = GetComponent<Animator>();
            //anim.SetFloat("speed", _stat.MoveSpeed);
        }
    }
    void UpdateSkill()
    {
        //Animator anim = GetComponent<Animator>();
        Debug.Log("공격!");
        //anim.SetBool("Attack", true);
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }
    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");
        //_Temp
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            Stat myStat = GetComponent<Stat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defence);
            Debug.Log("Hit");
            targetStat.Hp -= damage;
        }
        //Animator anim = GetComponent<Animator>();
        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
        //anim.SetBool("Attack", false);
    }
    void UpdateIdle()
    {
        //Animator anim = GetComponent<Animator>();
        //anim.SetFloat("speed", 0);
    }

    #region 키보드 움직임
    void OnKeyboard()
    {
        //좌,우, 전,후 이동
        //전진
        if (Input.GetKey(KeyCode.W))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            // == 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);

            //방향을 정해줬으니 앞으로만 가게했는데..
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);

            //트렌스레이트(로컬좌표)라서 포지션으로 바꿔줌
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.position += Vector3.forward * Time.deltaTime * _stat.MoveSpeed;
        }
        //후진
        if (Input.GetKey(KeyCode.S))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            // == 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);

            //transform.Translate(Vector3.back * Time.deltaTime * _speed);

            //트렌스 레이트(로컬좌표)를 포지션으로 바꿔줌
            transform.position += Vector3.back * Time.deltaTime * _stat.MoveSpeed;
        }
        //좌
        if (Input.GetKey(KeyCode.A))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            // == 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);

            //transform.Translate(Vector3.left *Time.deltaTime * _speed);

            //트렌스 레이트(로컬좌표)를 포지션으로 바꿔줌
            transform.position += Vector3.left * Time.deltaTime * _stat.MoveSpeed;
        }
        //우
        if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.right);
            // == 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);

            //transform.Translate(Vector3.right * Time.deltaTime * _speed);

            //트렌스 레이트(로컬좌표)를 포지션으로 바꿔줌
            transform.position += Vector3.right * Time.deltaTime * _stat.MoveSpeed;
        }
        _moveToDest = false;//클릭 방식으로 이동 불가
        //Animator anim = GetComponent<Animator>();
        //anim.SetFloat("speed", _stat.MoveSpeed);

        if (Input.GetKeyUp(KeyCode.None))
        {
            State = PlayerState.Idle;
        }
    }
    #endregion
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
                //마우스를 띄었을 때
                if(evt == Define.MouseEvent.PointerUp)
                {
                    _stopSkill = true;
                }
                break;
        }
    }

    GameObject _lockTarget;
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    #region 마우스 움직임
    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        //Press일경우는 작동 안되게끔(그냥 임시로 처리할 수 있게..)
        //프레스 기능을 사용하고 싶다면 삭제
        //if (evt != Define.MouseEvent.Click)
        //    return;
        if (State == PlayerState.Die)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        RaycastHit hit;

        bool raycastHit = Physics.Raycast(ray, out hit, 100f, _mask);

        switch (evt)
        {
            //눌렀을때 히트된게 뭐든 간에 이동은 하게
            case Define.MouseEvent.PointerDown:
                if (raycastHit)
                {
                    _destPos = hit.point;
                    State = PlayerState.Moving;
                    _stopSkill = false;//이때는 아직 아이들일지 재공격일지는 모름...
                }
                //충돌한게 몬스터이면 타겟을 몬스터로 그렇지 않으면 타겟 없음
                if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                {
                    _lockTarget = hit.collider.gameObject;
                }
                else
                {
                    _lockTarget = null;
                }
                break;
            //누르고 있는데 타겟이 있으면 타겟으로 이동
            case Define.MouseEvent.Press:
                //if(_lockTarget != null)
                //{
                //    _destPos = _lockTarget.transform.position;
                //}
                ////근데 충돌은 했으면
                //else if (raycastHit)
                //{
                //    //총돌한 곳으로
                //    _destPos = hit.point;
                //}
                if (_lockTarget == null && raycastHit)
                {
                    _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                //마우스를 땟으면 그냥 타겟을 잃어버리게
                //_lockTarget = null;
                //띄었다면 클릭으로 한번 공격처리
                _stopSkill = true;
                break;
        }

        if (Physics.Raycast(ray, out hit, 100f, _mask))
        {
            _destPos = hit.point;//클릭된 지점을 목적지로 지정
            State = PlayerState.Moving;
            _moveToDest = true; //클릭 방식으로 이동 가능 하게.
        }
        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
        {
            Debug.Log("몬스터");
        }
        else if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
        {
            Debug.Log("바닥");
        }
    }
    #endregion
}
