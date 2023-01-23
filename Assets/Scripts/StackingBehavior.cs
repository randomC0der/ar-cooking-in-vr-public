using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StackingBehavior : MonoBehaviour
{
    private GameObject _snapSpace;

    // Start is called before the first frame update
    void Start()
    {
        _snapSpace = Resources.Load<GameObject>("Snap Space");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectEnter(SelectEnterEventArgs e)
    {
        var go = e.interactableObject.transform;
        var snapSpace = Instantiate(_snapSpace, go);
        snapSpace.transform.localPosition = Vector3.zero;
        snapSpace.transform.position = e.interactorObject.transform.position; //+ new Vector3(0, go.GetComponent<MeshRenderer>().bounds.size.y);
        var stack = snapSpace.AddComponent<StackingBehavior>();
        var socket = snapSpace.GetComponent<XRSocketInteractor>();
        var event1 = new SelectEnterEvent();
        event1.AddListener(stack.SelectEnter);
        socket.selectEntered = event1;
        Debug.Log(go.name);
    }

    public void SelectExit(SelectExitEventArgs e)
    {
        var go = e.interactableObject.transform.gameObject;
        var c = go.transform.Find("Snap Space");
        Destroy(c.gameObject);
        Debug.Log(c);
    }
}
