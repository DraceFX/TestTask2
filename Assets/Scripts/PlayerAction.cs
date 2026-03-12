public class PlayerAction
{
    public ObjectScript Target;
    public SelectActioType ActionType;

    public PlayerAction(ObjectScript obj, SelectActioType type)
    {
        this.Target = obj;
        this.ActionType = type;
    }
}
