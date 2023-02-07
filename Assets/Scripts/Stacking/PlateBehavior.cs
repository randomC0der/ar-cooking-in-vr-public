using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;

// for the performance of the crafting see this video: https://www.youtube.com/watch?v=o4-zpAI7qBc
public class PlateBehavior : MonoBehaviour
{
    [SerializeField] 
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _bottomSnapSpace;
    private readonly List<SnapSpace> _children = new List<SnapSpace>();
    private Receipe[] _receipes;

    [SerializeField]
    private Transform _itemSpawnPosition;

    private int _numberOfBurgerCrafted = 0;
    private GameBehavior _gameBehavior;

    // Start is called before the first frame update
    void Start()
    {
        _gameBehavior = GameObject.Find("GameTaskManager").GetComponent<GameBehavior>();
        TextAsset textAsset = Resources.Load<TextAsset>("Recipes");
        _receipes = JsonUtility.FromJson<ReceipeBook>(textAsset.text).recipes;

        AddSnapSpace(_bottomSnapSpace.GetComponent<SnapBehavior>());
    }

    SnapSpace CreateSnapSpace()
    {
        var snapSpace = Instantiate(_bottomSnapSpace, transform).GetComponent<SnapBehavior>();
        var nextPosition = _bottomSnapSpace.transform.localPosition + new Vector3(0, .035f * _children.Count, 0);
        snapSpace.transform.localPosition = nextPosition;
        return AddSnapSpace(snapSpace);
    }

    private SnapSpace AddSnapSpace(SnapBehavior snapSpace)
    {
        snapSpace.name = $"SnapSpace {_children.Count}";
        snapSpace.AudioSource = _audioSource;

        var child = new SnapSpace
        {
            GameObject = snapSpace.gameObject,
            Interactor = snapSpace.GetComponent<StackingSocketInteractor>()
        };

        var selectEnterEvent = new SelectEnterEvent();
        selectEnterEvent.AddListener(EnableNextLevel);
        selectEnterEvent.AddListener(snapSpace.OnEnterSelection);
        child.Interactor.selectEntered = selectEnterEvent;

        var selectExitEvent = new SelectExitEvent();
        selectExitEvent.AddListener(DisableNextLevel);
        child.Interactor.selectExited = selectExitEvent;

        _children.Add(child);

        return child;
    }

    void EnableNextLevel(SelectEnterEventArgs e)
    {
        Transform t = e.interactableObject.transform;
        var grab = (XRGrabInteractable)e.interactableObject;
        grab.attachTransform = t.Find("Attatch Transform");

        // check for matching crafting recipe
        foreach (Receipe receipe in _receipes.Where(x => x.ingredients.Length == _children.Count(y => y.Interactor.hasSelection))
            .Where(receipe => IsReceipeFinished(receipe)))
        {
            TransformIngredients(receipe);
            return;
        }

        // add more socket interactors 
        if (_children.All(x => x.Interactor.hasSelection))
        {
            var snapSpace = CreateSnapSpace();
            return;
        }

        // enable next sockets
        var nextItem = _children.Select((x, i) => (x, i)).FirstOrDefault(x => !x.x.Interactor.socketActive);
        if (nextItem.x.GameObject == null) // default
        {
            Debug.LogWarning("gameobject is null");
            return;
        }
        _children[nextItem.i].Interactor.socketActive = true;
    }

    bool IsReceipeFinished(Receipe receipe)
    {
        for (int i = 0; i < receipe.ingredients.Length; i++)
        {
            if (_children[i].Interactor.firstInteractableSelected.transform.gameObject
                .GetComponent<StackableBehavior>().ingredient != receipe.ingredients[i])
            {
                return false;
            }
        }
        return true;
    }

    private void TransformIngredients(Receipe receipe)
    {
        foreach (var gameobj in _children.Select(child => child.Interactor.firstInteractableSelected?.transform.gameObject)
            .Where(x => x != null))
        {
            Destroy(gameobj);
        }

        if(++_numberOfBurgerCrafted >= 3)
        {
            _gameBehavior?.FinishTask(Task.Stacking);
        }
        GameObject product = Resources.Load<GameObject>(receipe.product);
        product = Instantiate(product);
        product.transform.position = _itemSpawnPosition.position;
    }

    void DisableNextLevel(SelectExitEventArgs e)
    {
        foreach (var i in _children.Select(x => x.Interactor).Where(x => !x.hasSelection))
        {
            i.socketActive = false;
        }
        _children.First(x => !x.Interactor.socketActive).Interactor.socketActive = true;
    }

    struct SnapSpace
    {
        public GameObject GameObject;
        public StackingSocketInteractor Interactor;
    }
}
