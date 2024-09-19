using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerStat _stat;
    //���콺 ������� �̵� �ϱ����� �� ����
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
    //Ŀ����Ʈ�ѷ��� �̵�
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
        //Ű���� �̵� ��ǲ ����
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        //���콺 �̵� ��ǲ ����
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        //hpbar ����
        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

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

    //���� ��ũ��Ʈ�� �ۼ�
    //void UpdateMouseCursor()
    //{
    //    //���콺�� ������ ������ ����ó���� ���� �ʰ� �׳� ����������
    //    //���� �� ���¿��� ��ǥ�� �̵��ŵ� Ŀ���� �״�εǾ� �ְ�
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

    //�ʱ� ���´� ������ idle
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

    void UpdateDie()
    {
        //�ƹ��͵� ���ϰ�
    }

    void UpdateMoving()
    {
        //���Ͱ� �� �����Ÿ����� ������ ����
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
            //����
            Vector3 dir = _destPos - transform.position;
            //�Ÿ� distance
            if (dir.magnitude < 0.1f)
            {
                State = PlayerState.Idle;
                _moveToDest = false;
            }
            else
            {
                float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
                //��Ϸ��̾� ��� idle�� ���� ����
                //��, �̵� �� �� ���� �����̴ϱ� ����
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
        Debug.Log("����!");
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
        _moveToDest = false;//Ŭ�� ������� �̵� �Ұ�
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
                //���콺�� ����� ��
                if(evt == Define.MouseEvent.PointerUp)
                {
                    _stopSkill = true;
                }
                break;
        }
    }

    GameObject _lockTarget;
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    #region ���콺 ������
    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        //Press�ϰ��� �۵� �ȵǰԲ�(�׳� �ӽ÷� ó���� �� �ְ�..)
        //������ ����� ����ϰ� �ʹٸ� ����
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
            //�������� ��Ʈ�Ȱ� ���� ���� �̵��� �ϰ�
            case Define.MouseEvent.PointerDown:
                if (raycastHit)
                {
                    _destPos = hit.point;
                    State = PlayerState.Moving;
                    _stopSkill = false;//�̶��� ���� ���̵����� ����������� ��...
                }
                //�浹�Ѱ� �����̸� Ÿ���� ���ͷ� �׷��� ������ Ÿ�� ����
                if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                {
                    _lockTarget = hit.collider.gameObject;
                }
                else
                {
                    _lockTarget = null;
                }
                break;
            //������ �ִµ� Ÿ���� ������ Ÿ������ �̵�
            case Define.MouseEvent.Press:
                //if(_lockTarget != null)
                //{
                //    _destPos = _lockTarget.transform.position;
                //}
                ////�ٵ� �浹�� ������
                //else if (raycastHit)
                //{
                //    //�ѵ��� ������
                //    _destPos = hit.point;
                //}
                if (_lockTarget == null && raycastHit)
                {
                    _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                //���콺�� ������ �׳� Ÿ���� �Ҿ������
                //_lockTarget = null;
                //����ٸ� Ŭ������ �ѹ� ����ó��
                _stopSkill = true;
                break;
        }

        if (Physics.Raycast(ray, out hit, 100f, _mask))
        {
            _destPos = hit.point;//Ŭ���� ������ �������� ����
            State = PlayerState.Moving;
            _moveToDest = true; //Ŭ�� ������� �̵� ���� �ϰ�.
        }
        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
        {
            Debug.Log("����");
        }
        else if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
        {
            Debug.Log("�ٴ�");
        }
    }
    #endregion
}
