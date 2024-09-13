using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    // 현재 씬을 찾고 반환하는 프로퍼티
    //현재 씬에 있는 BaseScene 객체를 찾음
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    // 씬을 로드하는 메서드
    // Define.Scene 타입을 받아 그에 해당하는 씬을 로드합니다.
    public void LoadScene(Define.Scene type)
    {
        // 현재 씬의 내용을 정리하는 메서드 호출
        Managers.Clear();
        // 다음 씬을 로드
        SceneManager.LoadScene(GetSceneName(type));
    }

    // Define.Scene 타입을 받아 그에 해당하는 씬 이름을 반환하는 메서드
    // Enum.GetName을 사용하여 Enum 값을 문자열로 변환
    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

}
