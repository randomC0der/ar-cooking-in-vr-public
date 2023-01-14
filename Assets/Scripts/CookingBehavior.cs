using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingBehavior : MonoBehaviour
{
    public GameObject rawItem;
    public GameObject cookedItem;
    public GameObject fire;
    public float cookingTime = 10;
    public float overCookingTime = 10;

    private float _timer;
    private bool _cooking;
    private bool? _done = false; // null means it's overcooked

    // Start is called before the first frame update
    void Start()
    {
        StartCooking();
    }

    [ContextMenu("StartCooking")]
    public void StartCooking()
    {
        _cooking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_cooking)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer > cookingTime && !_done.GetValueOrDefault(true))
        {
            _done = true;
            Destroy(rawItem);
            Object model = Instantiate(cookedItem, transform);
        }

        if (_timer > cookingTime + overCookingTime && _done.HasValue)
        {
            _done = null;
            GameObject model = Instantiate(fire, transform);
            model.transform.position = transform.position;
            model.transform.Translate(0, 0, .1f, Space.Self);
        }
    }
}