using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class TrashableBehavior : MonoBehaviour
{
    public string nameForTaskboard;

    private TaskManager _taskManager;
    private TaskManager.Task _deleteTask;

    // Start is called before the first frame update
    void Start()
    {
        _taskManager = FindObjectOfType<TaskManager>();
        _deleteTask = _taskManager.CreateTask($"Delete {nameForTaskboard ?? "object"}");
    }

    public void FinishTask()
    {
        _taskManager.FinishTask(_deleteTask);
    }
}
