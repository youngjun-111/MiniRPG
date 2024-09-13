using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        //게임 오브젝트 타입을 바인딩
        Bind<GameObject>(typeof(GameObjects));

        //기존에 테스트를 위해 넣어두었던 슬롯을 삭제한다.
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform)
        {
            Managers.Resources.Destroy(child.gameObject);
        }
        //실제로는 실제 인벤토리 정보를 참고해서 만들어야 하지만 지금은 테스트니깐 그냥 만들어 본것임
        for (int i = 0; i < 12; i++)
        {
            GameObject item = Managers.Resources.Instantiate("UI/Scene/UI_Inven_Item");
            item.transform.SetParent(gridPanel.transform);

            //GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>()

            //Util.GetOrAddComponent<UI_Inven_Item>(item);
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
            invenItem.SetInfo($"테스트{i}번");
        }
    }
}
