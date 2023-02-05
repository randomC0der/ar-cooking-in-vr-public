using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Annahmen: <see cref="rawItem"/> und <see cref="cookedItem"/> müssen die gleiche Skalierung haben.
/// </summary>
public class CookingParentBehavior : MonoBehaviour
{
    public GameObject rawItem;
    public GameObject cookedItem;
    public GameObject burntItem;
    public GameObject fire;
    public float cookingTime;
    public float overCookingTime;
    public string ingredientTag;

    public AudioClip cookingClip;
    public AudioClip burningClip;
    public AudioClip ignitionClip;

    private float _timer;
    private bool? _done = false; // null means it's overcooked

    public bool IsCooking { get; set; }

    /// <summary>
    /// Transformation, damit das Objekt richtig an den XR controller / sockets attatched wird
    /// </summary>
    public Transform AttatchTransform { get; private set; }

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
            CookableBehavior cookedGrabable = Instantiate(cookedItem, transform).AddComponent<CookableBehavior>();
            AttatchTransform = rawItem.transform.Find("Attatch Transform").AttatchTo(cookedGrabable.transform);
            Destroy(rawItem);
            //IsCooking = true; // still cooking
            cookedGrabable.transform.localScale = scale;
            cookedItem = cookedGrabable.gameObject;
        }

        if (_timer > cookingTime + overCookingTime && _done.HasValue)
        {
            _done = null;

            Vector3 scale = cookedItem.transform.localScale;
            CookableBehavior burntGrabable = Instantiate(burntItem, transform).AddComponent<CookableBehavior>();
            AttatchTransform = cookedItem.transform.Find("Attatch Transform").AttatchTo(burntGrabable.transform);
            Destroy(cookedItem);
            burntGrabable.transform.localScale = scale;
            burntItem = burntGrabable.gameObject;
            burntGrabable.Burnt = true;

#if false // aktuell nicht gewünscht
            GameObject model = Instantiate(fire, transform);
            burntGrabable.PlayBurning = true;
#endif
        }
    }
}

public static class CookingParentBehaviorExtension
{
    /// <summary>
    /// Hilfsmethode, um vom Child zum Parent Information zu übertragen
    /// </summary>
    public static CookingParentBehavior PassParameter(this CookingParentBehavior behavior,
        GameObject rawItem,
        GameObject cookedItem,
        GameObject burntItem,
        GameObject fire,
        float cookingTime,
        float overCookingTime,
        AudioClip cookingClip,
        AudioClip burningClip,
        AudioClip ignitionClip,
        string ingredientTag)
    {
        behavior.rawItem = rawItem;
        behavior.cookedItem = cookedItem;
        behavior.burntItem = burntItem;
        behavior.fire = fire;
        behavior.cookingTime = cookingTime;
        behavior.overCookingTime = overCookingTime;
        behavior.cookingClip = cookingClip;
        behavior.burningClip = burningClip;
        behavior.ignitionClip = ignitionClip;
        behavior.ingredientTag = ingredientTag;
        return behavior;
    }
}