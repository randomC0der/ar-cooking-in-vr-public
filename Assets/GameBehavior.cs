using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject _taskBoard;
    [SerializeField]
    private GameObject _directionMarkerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _taskBoard ??= GameObject.Find("TaskBoard");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Task
{
    CleanUp = 1,
    Cutting = 2,

}
