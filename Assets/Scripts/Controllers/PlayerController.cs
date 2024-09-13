using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerStat _stat;
    //마우스 방식으로 이동 하기위한 불 변수
    bool _moveToDest = false;

    Vector3 _destPos;

    UI_Button uiPopup;
    //float wait_run_ratio = 0;
    Animator anim;
    NavMeshAgent navAgent;
    void Start()
    {
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        navAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //키보드 이동 인풋 구독
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        //마우스 이동 인풋 구독
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

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

   
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    //초기 상태는 무조건 idle
    PlayerState _state = PlayerState.Idle;

    void Update()
    {
        switch (_state)
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
        if (_moveToDest)
        {
            //방향
            Vector3 dir = _destPos - transform.position;


           
            //거리 distance
            if (dir.magnitude < 0.1f)
            {
                _state = PlayerState.Idle;
                _moveToDest = false;
            }
            else
            {
                float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
                //블록레이어 라면 idle로 변경 해줌
                //즉, 이동 할 수 없는 구역이니까 멈춤
                if (Physics.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Block")))
                {
                    _state = PlayerState.Idle;
                    return;
                }
                Debug.DrawRay(transform.position, dir.normalized, Color.green);
                //transform.position += dir.normalized * moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
                navAgent.Move(dir.normalized * moveDist);
            }
            anim.SetFloat("speed", _stat.MoveSpeed);
        }
    }
    void UpdateIdle()
    {
        anim.SetFloat("speed", 0);
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
        anim.SetFloat("speed", _stat.MoveSpeed);

        if (Input.GetKeyUp(KeyCode.None))
        {
            _state = PlayerState.Idle;
        }
    }
    #endregion

    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);
    #region 마우스 움직임
    void OnMouseClicked(Define.MouseEvent evt)
    {
        //Press일경우는 작동 안되게끔(그냥 임시로 처리할 수 있게..)
        //프레스 기능을 사용하고 싶다면 삭제
        //if (evt != Define.MouseEvent.Click)
        //    return;
        if (_state == PlayerState.Die)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, _mask))
        {
            _destPos = hit.point;//클릭된 지점을 목적지로 지정
            _state = PlayerState.Moving;
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
    }
    #endregion

}
