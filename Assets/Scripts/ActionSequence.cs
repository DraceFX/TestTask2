using System;
using UnityEngine;

public class ActionSequence : MonoBehaviour
{
    public ActionStep[] Steps = new ActionStep[10];
}

[Serializable]
public class ActionStep
{
    public ObjectScript ObjectId;
    public SelectActioType ActionType;
}

public enum SelectActioType
{
    Click,
    Hold
}
