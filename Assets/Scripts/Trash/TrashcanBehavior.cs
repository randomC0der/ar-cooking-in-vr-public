using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrashcanBehavior : MonoBehaviour
{
    private AudioSource _audioSource;
    private GameBehavior _gameBehavior;

    private int _taskCounter = 0;
      

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _gameBehavior = GameObject.Find("GameTaskManager").GetComponent<GameBehavior>();
    }

    public void EmptyTrashcan(SelectEnterEventArgs e)
    {
        _audioSource.PlayOneShot(_audioSource.clip);
        Transform t = e.interactableObject.transform;

        var trashable = t.GetComponent<TrashableBehavior>();
        if (trashable != null)
        {
            _taskCounter++;
            if (_taskCounter >= 1)
            {
                _gameBehavior.FinishTask(Task.CleanUp);
            } 
        }

        var cookable = t.GetComponent<CookableBehavior>();
        Destroy(t.gameObject);
        if (cookable != null)
        {
            Destroy(cookable.Parent);
        }
    }
}