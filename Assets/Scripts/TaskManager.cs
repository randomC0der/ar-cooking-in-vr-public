using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private List<TaskBoardTask> _tasks = new List<TaskBoardTask>();
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Start()
    {
        UpdateText();
    }

    // Only update text if needed for better performance
    private void UpdateText()
    {
        _text.text = string.Join("\n", _tasks.Select((x, y) =>
        {
            var taskText = $"Aufgabe {y+1}: {x.Text}";

            return x.Finished ? $"<s>{taskText}</s>" : taskText;
        }));
        _text.ForceMeshUpdate(true);
    }

    public void FinishTask(TaskBoardTask task)
    {
        task.Finished = true;
        UpdateText();
    }

    public TaskBoardTask CreateTask(string text)
    {
        var task = new TaskBoardTask(text);
        _tasks.Add(task);
        UpdateText();

        return task;
    }

    public void RemoveTask(TaskBoardTask task)
    {
        _tasks.Remove(task);
        UpdateText();
    }

    public class TaskBoardTask
    {
        public string Text { get; }
        public bool Finished { get; internal set; }

        public TaskBoardTask(string text)
        {
            Text = text;
        }

    }
}
