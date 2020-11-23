using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class NpcTalkNode
{
    public List<NpcTalkOption> options;//옵션
    [TextArea(5, 5)]
    public string mainText;//본문 대사
    public NpcTalkResult Execute(int option)
    {
        switch (option)
        {
            case -2:
                //메시지를 띄워주어야 함
                NpcManager.Instance.CreateInteractWindow(mainText, options);
                return new NpcTalkResult(-1, false);
            case -1:
                return new NpcTalkResult(-1, false);
            //대기 아무것도 안함
            default:
                //0~n
                //옵션 선택 성공 다음 상태값 반환 및 커스텀 함수 실행
                NpcManager.Instance.CloseInteractWindow();
                return new NpcTalkResult(options[option].nextState, options[option].isLast);
        }
    }
}