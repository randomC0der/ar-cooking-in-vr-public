using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject _taskBoard;
    [SerializeField]
    private GameObject _directionMarkerPrefab;

    private List<Task> currentTasks = new List<Task>();

    [SerializeField]
    private TaskBehavior[] _tasks;

    void Start()
    {
        if(_taskBoard == null)
        {
            _taskBoard = GameObject.Find("TaskBoard");
        }

        StartTask(Task.CleanUp);

        _tasks = GetComponents<TaskBehavior>();

        foreach(TaskBehavior task in _tasks) {
            task.Init();
        }
    }

    void StartTask(Task task)
    {
        TaskBehavior t = _tasks.SingleOrDefault(x => x.Task == task);
        if(t != null)
        {
            t.StartTask();
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
