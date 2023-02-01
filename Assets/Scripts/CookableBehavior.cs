using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CookableBehavior : MonoBehaviour
{
    public GameObject cookedItem;
    public GameObject burntItem;
    public GameObject fire;
    public float cookingTime = 10;
    public float overCookingTime = 10;
    public string ingredientTag;

    [Tooltip("Looping sound clip that is played during cooking")]
    public AudioClip cookingClip;

    [Tooltip("Sound clip that is played once when the food gets burned")]
    public AudioClip ignitionClip;

    [Tooltip("Sound clip that is played during burning")]
    public AudioClip burningClip;

    public bool PlayBurning { get; set; }

    public GameObject Parent { get; private set; }
    private CookingParentBehavior _parentBehavior;

    public AudioSource CookingAudioSource { get; private set; }
    private AudioSource _burningAudioSource;
    private AudioSource _ignitionAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        CookingAudioSource = gameObject.AddComponent<AudioSource>();
        CookingAudioSource.loop = true;

        _burningAudioSource = gameObject.AddComponent<AudioSource>();
        _burningAudioSource.loop = true;

        _ignitionAudioSource = gameObject.AddComponent<AudioSource>();

        var stackable = gameObject.AddComponent<StackableBehavior>();

        GrabableGameObject grabable = gameObject.AddGrabableComponents(false);
        grabable.XrGrab.interactionLayers = InteractionLayerMask.GetMask("Default", "Cookable");
        grabable.XrGrab.useDynamicAttach = true;
        grabable.XrGrab.matchAttachPosition = false;

        if (transform.parent is null) // aus dem Designer
        {
            Parent = new GameObject("Cookable Parent");
            _parentBehavior = Parent.AddComponent<CookingParentBehavior>()
                .PassParameter(gameObject, cookedItem, burntItem, fire, cookingTime, overCookingTime, cookingClip, burningClip, ignitionClip, ingredientTag);
            transform.parent = Parent.transform;
            CookingAudioSource.clip = cookingClip;
            _burningAudioSource.clip = burningClip;
            _ignitionAudioSource.clip = ignitionClip;
            stackable.ingredient = ingredientTag;
        }
        else // aus dem Parent
        {
            Parent = transform.parent.gameObject;
            _parentBehavior = Parent.GetComponent<CookingParentBehavior>();
            grabable.XrGrab.attachTransform = _parentBehavior.AttatchTransform;
            CookingAudioSource.clip = _parentBehavior.cookingClip;
            _burningAudioSource.clip = _parentBehavior.burningClip;
            _ignitionAudioSource.clip = _parentBehavior.ignitionClip;
            grabable.XrGrab.interactionLayers = InteractionLayerMask.GetMask("Default", "Cookable", "Stackable");
            stackable.ingredient = _parentBehavior.ingredientTag;
        }
    }

    [ContextMenu(nameof(StartCooking))]
    public void StartCooking()
    {
        _parentBehavior.IsCooking = true;
    }

    [ContextMenu(nameof(StopCooking))]
    public void StopCooking()
    {
        _parentBehavior.IsCooking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Parent.transform.position = transform.position;
        transform.localPosition = Vector3.zero;

        if (PlayBurning && !_burningAudioSource.isPlaying)
        {
            _ignitionAudioSource.PlayOneShot(_ignitionAudioSource.clip, 3f);
            _burningAudioSource.Play();
            PlayBurning = false;
        }
    }
}