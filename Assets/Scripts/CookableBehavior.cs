using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CookableBehavior : MonoBehaviour
{
    public GameObject cookedItem;
    public GameObject fire;
    public float cookingTime = 10;
    public float overCookingTime = 10;

    [Tooltip("Looping sound clip that is played during cooking")]
    public AudioClip cookingClip;

    [Tooltip("Sound clip that is played once when the food gets burned")]
    public AudioClip ignitionClip;

    [Tooltip("Sound clip that is played during burning")]
    public AudioClip burningClip;

    public bool PlayBurning { get; set; }

    private GameObject _parent;
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

        GrabableGameObject grabable = gameObject.AddGrabableComponents();
        grabable.XrGrab.interactionLayers = InteractionLayerMask.GetMask("Default", "Cookable");

        if (transform.parent is null)
        {
            _parent = new GameObject("Cookable Parent");
            _parentBehavior = _parent.AddComponent<CookingParentBehavior>()
                .PassParameter(gameObject, cookedItem, fire, cookingTime, overCookingTime, cookingClip, burningClip, ignitionClip);
            transform.parent = _parent.transform;
            CookingAudioSource.clip = cookingClip;
            _burningAudioSource.clip = burningClip;
            _ignitionAudioSource.clip = ignitionClip;
        }
        else
        {
            _parent = transform.parent.gameObject;
            _parentBehavior = _parent.GetComponent<CookingParentBehavior>();
            grabable.XrGrab.attachTransform = _parentBehavior.AttatchTransform;
            CookingAudioSource.clip = _parentBehavior.cookingClip;
            _burningAudioSource.clip = _parentBehavior.burningClip;
            _ignitionAudioSource.clip = _parentBehavior.ignitionClip;
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
        _parent.transform.position = transform.position;
        transform.localPosition = Vector3.zero;

        if (PlayBurning && !_burningAudioSource.isPlaying)
        {
            _ignitionAudioSource.PlayOneShot(_ignitionAudioSource.clip, 3f);
            _burningAudioSource.Play();
            PlayBurning = false;
        }
    }
}