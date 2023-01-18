using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[HideInInspector]
public class CookingParentBehavior : MonoBehaviour
{
    public GameObject rawItem;
    public GameObject cookedItem;
    public GameObject fire;
    public float cookingTime;
    public float overCookingTime;

    private float _timer;
    private bool? _done = false; // null means it's overcooked

    public bool IsCooking { get; set; }

    void Update()
    {
        if (!IsCooking)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer > cookingTime && !_done.GetValueOrDefault(true))
        {
            _done = true;
            Vector3 scale = rawItem.transform.localScale;
            Destroy(rawItem);
            GameObject model = Instantiate(cookedItem, transform);
            model.transform.localScale = scale;
        }

        if (_timer > cookingTime + overCookingTime && _done.HasValue)
        {
            _done = null;
            GameObject model = Instantiate(fire, transform);
            model.AddComponent<FireBehavior>();
            var t = model.transform;
            t.position = transform.position;
            t.Translate(0, 0, .1f, Space.Self);
        }
    }
}

public static class CookingParentBehaviorExtension
{
    public static CookingParentBehavior PassParameter(this CookingParentBehavior behavior,
        GameObject rawItem,
        GameObject cookedItem,
        GameObject fire,
        float cookingTime,
        float overCookingTime)
    {
        behavior.rawItem = rawItem;
        behavior.cookedItem = cookedItem;
        behavior.fire = fire;
        behavior.cookingTime = cookingTime;
        behavior.overCookingTime = overCookingTime;
        return behavior;
    }
}