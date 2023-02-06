using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private List<Task> _tasks = new List<Task>();
    [SerializeField]
    private TextMeshProUGUI _text;

    // sonst ist es nicht anders möglich auf die property zuzugreifen
    private PropertyInfo _taskFinished;

    private void Start()
    {
        UpdateText();
        _taskFinished = typeof(Task).GetProperty(nameof(Task.Finished));
    }

    // Only update text if needed for better performance
    private void UpdateText()
    {
        _text.text = string.Join("\n", _tasks.Select(x =>
        {
            if (x.Finished)
            {
                return $"<s>{x.Text}</s>";
            }
            return x.Text;
        }));
        _text.ForceMeshUpdate(true);
    }

    public void FinishTask(Task task)
    {
        task.Finished = true;
        UpdateText();
    }

    public Task CreateTask(string text)
    {
        var task = new Task(text);
        _tasks.Add(task);
        UpdateText();

        return task;
    }

    public void RemoveTask(Task task)
    {
        _tasks.Remove(task);
        UpdateText();
    }

    public class Task
    {
        public string Text { get; }
        public bool Finished { get; internal set; }

        public Task(string text)
        {
            Text = text;
        }

    }
}
