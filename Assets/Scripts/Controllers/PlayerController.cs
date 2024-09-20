using UnityEngine;



public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);

    [SerializeField]
    PlayerStat _stat;



    //마우스 방식으로 이동 하기위한 불 변수
    //bool _moveToDest = false;
    bool _stopSkill = false;


    //UI_Button uiPopup;
    //float wait_run_ratio = 0;
    Animator anim;

    void Start()
    {
        Init();
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

    void Update()
    {
        Debug.Log(State);
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
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

    public override void Init()
    {
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        anim = GetComponent<Animator>();
        WorldObjetTpye = Define.WorldObject.Player;
        //if(gameObject.GetComponentInChildren<UI_HPBar>() == null)
        //{
        //    Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        //}
        //키보드 이동 인풋 구독
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        //마우스 이동 인풋 구독
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    protected override void UpdateMoving()
    {
        //몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }
        //방향
        Vector3 dir = _destPos - transform.position;
        Debug.Log(dir);


        //거리 distance
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            //_moveToDest = false;
        }
        else
        {
            //블록레이어 라면 idle로 변경 해줌
            //즉, 이동 할 수 없는 구역이니까 멈춤
            if (Physics.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                {
                    State = Define.State.Idle;
                }
                return;
            }
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            //플레이어 이동
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

        }
        //anim.SetFloat("speed", _stat.MoveSpeed);
        //if (_moveToDest)
    }

    protected override void UpdateSkill()
    {
        Debug.Log("UpdateSkill");
        Vector3 dir = _lockTarget.transform.position - transform.position;
        Quaternion quat = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
    }

    void OnHitEvent()
    {
        //Debug.Log("OnHitEvent");

        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            //Stat myStat = GetComponent<PlayerStat>();
            //int damage = Mathf.Max(0, myStat.Attack - targetStat.Defence);
            //Debug.Log(damage);
            //targetStat.Hp -= damage;
            //몬스터와 동일하게 Stat에서 참조하여 사용
            targetStat.OnAttacked(_stat);
        }

        if (!_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
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
        //_moveToDest = false;//클릭 방식으로 이동 불가
        anim.SetFloat("speed", _stat.MoveSpeed);

        if (Input.GetKeyUp(KeyCode.None))
        {
            _state = Define.State.Idle;
        }
    }
    #endregion

    #region 마우스 움직임
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                if (evt == Define.MouseEvent.PointerUp)
                {
                    _stopSkill = true;
                }

                break;
        }
        /*
        //Press일경우는 작동 안되게끔(그냥 임시로 처리할 수 있게..)
        //프레스 기능을 사용하고 싶다면 삭제
        //if (evt != Define.MouseEvent.Click)
        //    return;
        if (_state == PlayerState.Die)
        {
            return;
        }

        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch(evt)
        {
            case Define.MouseEvent.PointerDown:

                break;
            case Define.MouseEvent.Press:

                break;
            case Define.MouseEvent.PointerUp:

                break;
        }

        

        if (Physics.Raycast(ray, out hit, 100f, _mask))
        {
            _destPos = hit.point;//클릭된 지점을 목적지로 지정
            State = PlayerState.Moving;
            _moveToDest = true; //클릭 방식으로 이동 가능 하게.
        }
        if(hit.collider.gameObject.layer == (int)Define.Layer.Monster)
        {
            Debug.Log("몬스터");
        }
        else if(hit.collider.gameObject.layer == (int)Define.Layer.Ground)
        {
            Debug.Log("바닥");
        }
        */
    }
    #endregion


    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        //무엇이든 히트되면 일단 이동
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false; // 아직 아이들일지 공격일지 모름

                        //충돌한게 몬스터이면 타겟을 몬스터로 그렇지 않으면 타겟 없음
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            _lockTarget = hit.collider.gameObject;
                        }
                        else
                        {
                            _lockTarget = null;
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press: // 누르고 있는데 타겟이 있으면 타겟으로 이동
                {
                    /*
                    if(_lockTarget != null)
                    {
                        _destPos = _lockTarget.transform.position;
                    }
                    else if(raycastHit) // 타겟이 없으면 클릭한 곳으로
                    {
                        _destPos = hit.point;
                    }
                    */
                    //불필요한 부분이 있어 수정
                    if (_lockTarget == null && raycastHit)
                    {
                        State = Define.State.Moving;
                        _destPos = hit.point;
                    }

                }
                break;

            case Define.MouseEvent.PointerUp:
                _stopSkill = true; //뗏다면 한번 클릭으로 공격처리
                break;
        }
    }
}
