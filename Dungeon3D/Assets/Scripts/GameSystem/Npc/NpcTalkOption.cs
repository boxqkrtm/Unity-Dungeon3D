[System.Serializable]
public class NpcTalkOption
{
    public NpcTalkOption(string optionName, int nextState, bool isLast)
    {
        this.optionName = optionName;
        this.nextState = nextState;
        this.isLast = isLast;
    }
    public string optionName;//옵션 이름
    public int nextState;//이동 할 상태
    public bool isLast = false;
}