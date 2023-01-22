using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private List<Task> _tasks = new List<Task>();
    [SerializeField]
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _tasks.Add(new Task("This is a test"));
    }

    private void Update()
    {
        _text.text = string.Join("\n", _tasks.Select(x => x.Text));
    }

    public void FinishTask(Task task)
    {

    }

    public Task CreateTask(string text)
    {
        var task = new Task(text);
        _tasks.Add(task);
        return task;
    }

    public class Task
    {
        public string Text { get; }
        public bool Finished { get; private set; } = true;

        public Task(string text)
        {
            Text = text;
        }

        public void FinishTask()
        {
            Finished = false;
        }

    }
}
