using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static TaskManager;

public class GameBehavior : MonoBehaviour
{
    [SerializeField]
    private TaskManager _taskBoard;

    private List<Task> currentTasks = new List<Task>();

    [SerializeField]
    private TaskBehavior[] _tasks;

    private Dictionary<TaskBehavior, TaskBoardTask> _startedTasks = new Dictionary<TaskBehavior, TaskBoardTask>();

    void Start()
    {
        if(_taskBoard == null)
        {
            _taskBoard = GameObject.Find("TaskBoardCamera").GetComponent<TaskManager>();
        }
        _tasks = GetComponents<TaskBehavior>();      

        foreach(TaskBehavior task in _tasks) {
            task.Init();
        }

        StartNextTask(null);
    }

    public void StartTask(Task task)
    {
        TaskBehavior t = _tasks.SingleOrDefault(x => x.Task == task);
        if (t == null)
        {
            return;
        }

        t.StartTask();
        var tbt = _taskBoard.CreateTask(t.TaskText);
        _startedTasks.Add(t, tbt);
    }

    public void FinishTask(Task task)
    {
        TaskBehavior t = _tasks.SingleOrDefault(x => x.Task == task);
        if (t == null || !_startedTasks.ContainsKey(t))
        {
            return;
        }

        t.FinishTask();
        _taskBoard.FinishTask(_startedTasks[t]);
        _startedTasks.Remove(t);

        StartNextTask(task);
    }

    void StartNextTask(Task? lastTask)
    {
        switch (lastTask)
        {
            case null:
                StartTask(Task.CleanUp);
                break;
            case Task.CleanUp:
                StartTask(Task.Cutting);
                StartTask(Task.Frying);
                break;
            case Task.Cutting:
                StartTask(Task.Stacking);
                break;
            case Task.Frying:
                StartTask(Task.Stacking);
                break;
            case Task.Stacking:
                // Finished
                break;
        }
    }
}

public enum Task
{
    CleanUp = 1,
    Cutting = 2,
    Frying = 3,
    Stacking = 4,
}
