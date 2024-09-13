using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    //타겟(플레이어) 으로 부터의 거리
    [SerializeField]
    Vector3 _delta = new Vector3(0f, 6f, -5f);
    //타겟
    [SerializeField]
    GameObject _player = null;

    //업데이트보다 늦게 시작
    private void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            RaycastHit hit;
            //플레이어에서 카메라 방향으로 레이를 발사(플레이어위치, 카메라위치, out hit,방향, 충돌 가능한 레이어)
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;//거리를 0.8를 곱해서 줄여줌
                transform.position = _player.transform.position + _delta.normalized * dist;//카메라의 위치 변경
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }

    //카메라의 시야가 차단되었을 경우 사야를 확보 할수 있게



    //나중에 쿼터뷰를 코드로 구현하고 싶다면 이런식으로 함수를 만들면 된다.
    //(무기모드 변경하듯이, 또는 VR에서 이동 방법 변경하듯이 처리하면 된다.)
    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
