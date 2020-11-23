public class NpcTalkResult
{
    //next state 값
    //0~n 일반 응답
    //-1 플레이어 선택 기다리는 중
    //-2~-n 커스텀 함수 영역
    public int nextState;

    public bool isLast;//마지막 대사, true이면 이후 state전환 후 대사창을 닫음
    public NpcTalkResult(int nextState, bool isLast)
    {
        this.nextState = nextState;
        this.isLast = isLast;
    }
}