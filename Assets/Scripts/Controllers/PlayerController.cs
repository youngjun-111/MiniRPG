using UnityEngine;



public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);

    [SerializeField]
    PlayerStat _stat;



    //���콺 ������� �̵� �ϱ����� �� ����
    //bool _moveToDest = false;
    bool _stopSkill = false;


    //UI_Button uiPopup;
    //float wait_run_ratio = 0;
    Animator anim;

    void Start()
    {
        Init();
        //���� ������ �ẻ�͵� UI�� ����� ����
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
        //������ ������ ���� UI_Button�� ������Ų��.
        //Managers.Resources.Instantiate("UI/UI_Button");
        //������ ������ ���� UI_Inven�� ���� ��Ų��.
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

        //Ű���� ����̱� ������ Ű�Է°��� 1�� 0���� �༭ �ִϸ��̼��� ��� �Ϸ��� �Ҷ� ���� �ڵ�
        //����� �Ķ���͸� �̿��Ͽ� ����ϸ� _state�� ����Ͽ� ���¸� �������� ����ϱ⶧���� �ʿ� ������
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
        //Ű���� �̵� ��ǲ ����
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        //���콺 �̵� ��ǲ ����
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    protected override void UpdateMoving()
    {
        //���Ͱ� �� �����Ÿ����� ������ ����
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
        //����
        Vector3 dir = _destPos - transform.position;
        Debug.Log(dir);


        //�Ÿ� distance
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            //_moveToDest = false;
        }
        else
        {
            //��Ϸ��̾� ��� idle�� ���� ����
            //��, �̵� �� �� ���� �����̴ϱ� ����
            if (Physics.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                {
                    State = Define.State.Idle;
                }
                return;
            }
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            //�÷��̾� �̵�
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
            //���Ϳ� �����ϰ� Stat���� �����Ͽ� ���
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

    #region Ű���� ������
    void OnKeyboard()
    {
        //��,��, ��,�� �̵�
        //����
        if (Input.GetKey(KeyCode.W))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            // == ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);

            //������ ���������� �����θ� �����ߴµ�..
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);

            //Ʈ��������Ʈ(������ǥ)�� ���������� �ٲ���
            //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.position += Vector3.forward * Time.deltaTime * _stat.MoveSpeed;
        }
        //����
        if (Input.GetKey(KeyCode.S))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            // == ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);

            //transform.Translate(Vector3.back * Time.deltaTime * _speed);

            //Ʈ���� ����Ʈ(������ǥ)�� ���������� �ٲ���
            transform.position += Vector3.back * Time.deltaTime * _stat.MoveSpeed;
        }
        //��
        if (Input.GetKey(KeyCode.A))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            // == ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);

            //transform.Translate(Vector3.left *Time.deltaTime * _speed);

            //Ʈ���� ����Ʈ(������ǥ)�� ���������� �ٲ���
            transform.position += Vector3.left * Time.deltaTime * _stat.MoveSpeed;
        }
        //��
        if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.right);
            // == ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);

            //transform.Translate(Vector3.right * Time.deltaTime * _speed);

            //Ʈ���� ����Ʈ(������ǥ)�� ���������� �ٲ���
            transform.position += Vector3.right * Time.deltaTime * _stat.MoveSpeed;
        }
        //_moveToDest = false;//Ŭ�� ������� �̵� �Ұ�
        anim.SetFloat("speed", _stat.MoveSpeed);

        if (Input.GetKeyUp(KeyCode.None))
        {
            _state = Define.State.Idle;
        }
    }
    #endregion

    #region ���콺 ������
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
        //Press�ϰ��� �۵� �ȵǰԲ�(�׳� �ӽ÷� ó���� �� �ְ�..)
        //������ ����� ����ϰ� �ʹٸ� ����
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
            _destPos = hit.point;//Ŭ���� ������ �������� ����
            State = PlayerState.Moving;
            _moveToDest = true; //Ŭ�� ������� �̵� ���� �ϰ�.
        }
        if(hit.collider.gameObject.layer == (int)Define.Layer.Monster)
        {
            Debug.Log("����");
        }
        else if(hit.collider.gameObject.layer == (int)Define.Layer.Ground)
        {
            Debug.Log("�ٴ�");
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
                        //�����̵� ��Ʈ�Ǹ� �ϴ� �̵�
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false; // ���� ���̵����� �������� ��

                        //�浹�Ѱ� �����̸� Ÿ���� ���ͷ� �׷��� ������ Ÿ�� ����
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
            case Define.MouseEvent.Press: // ������ �ִµ� Ÿ���� ������ Ÿ������ �̵�
                {
                    /*
                    if(_lockTarget != null)
                    {
                        _destPos = _lockTarget.transform.position;
                    }
                    else if(raycastHit) // Ÿ���� ������ Ŭ���� ������
                    {
                        _destPos = hit.point;
                    }
                    */
                    //���ʿ��� �κ��� �־� ����
                    if (_lockTarget == null && raycastHit)
                    {
                        State = Define.State.Moving;
                        _destPos = hit.point;
                    }

                }
                break;

            case Define.MouseEvent.PointerUp:
                _stopSkill = true; //�´ٸ� �ѹ� Ŭ������ ����ó��
                break;
        }
    }
}
