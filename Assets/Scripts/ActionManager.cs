using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    [SerializeField] private ActionSequence _sequence;
    [SerializeField, Min(0)] private int _allowedShift = 1;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TextToPanel _prefabText;

    private List<PlayerAction> playerActions = new List<PlayerAction>();

    private void Awake()
    {
        Instance = this;
        _canvas.gameObject.SetActive(false);
    }

    public void RegisterAction(ObjectScript obj, SelectActioType type)
    {
        if (playerActions.Count >= _sequence.Steps.Length) return;

        playerActions.Add(new PlayerAction(obj, type));

        if (playerActions.Count >= _sequence.Steps.Length)
        {
            ShowResults();
        }
    }

    // private void ShowResults()
    // {
    //     for (int i = 0; i < _sequence.Steps.Length; i++)
    //     {
    //         var expected = _sequence.Steps[i];
    //         var actual = playerActions[i];

    //         string result = $"{i + 1} клик по \"{actual.Target.name}\" {actual.ActionType}, " + $"должно быть - {expected.ActionType} по \"{expected.ObjectId.name}\"";

    //         // Debug.Log(result);
    //         GenerateResultUi(result);
    //     }
    //     _canvas.gameObject.SetActive(true);
    // }

    private void ShowResults()
    {
        var results = AnalyzeActions();

        for (int i = 0; i < results.Count; i++)
        {
            var r = results[i];

            string expectedName = r.ExpectedAction != null ? r.ExpectedAction.ObjectId.name : "нет ожидаемого действия";
            string expectedType = r.ExpectedAction != null ? r.ExpectedAction.ActionType.ToString() : "-";

            string result = $"{i + 1} клик по \"{r.PlayerAction.Target.name}\" {r.PlayerAction.ActionType}, " + $"должно быть - {expectedType} по \"{expectedName}\"";

            // Debug.Log(result);
            GenerateResultUi(result);
        }
        _canvas.SetActive(true);
    }

    private void GenerateResultUi(string result)
    {
        var textPanel = Instantiate(_prefabText, _prefabText.transform.parent);

        textPanel.gameObject.SetActive(true);
        textPanel.SetText(result);
    }

    private List<StepResult> AnalyzeActions()
    {
        var results = new List<StepResult>();

        int i = 0;
        int j = 0;

        while (i < _sequence.Steps.Length && j < playerActions.Count)
        {
            var expected = _sequence.Steps[i];
            var actual = playerActions[j];

            bool match = expected.ObjectId == actual.Target && expected.ActionType == actual.ActionType;

            if (match)
            {
                results.Add(new StepResult { PlayerAction = actual, ExpectedAction = expected, IsCorrect = true });

                i++;
                j++;
                continue;
            }

            bool shiftFound = false;

            for (int s = 1; s <= _allowedShift; s++)
            {
                if (j + s >= playerActions.Count) break;

                var future = playerActions[j + s];

                bool shiftMatch = expected.ObjectId == future.Target && expected.ActionType == future.ActionType;

                if (shiftMatch)
                {
                    for (int k = 0; k < s; k++)
                    {
                        results.Add(new StepResult { PlayerAction = playerActions[j + k], ExpectedAction = expected, IsCorrect = false });
                    }

                    j += s;
                    shiftFound = true;
                    break;
                }
            }

            if (shiftFound) continue;

            results.Add(new StepResult { PlayerAction = actual, ExpectedAction = expected, IsCorrect = false });

            i++;
            j++;
        }

        return results;
    }
}

public class StepResult
{
    public PlayerAction PlayerAction;
    public ActionStep ExpectedAction;
    public bool IsCorrect;
}
