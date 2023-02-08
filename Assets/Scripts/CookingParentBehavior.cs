using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Annahmen: <see cref="_rawItem"/> und <see cref="_cookedItem"/> müssen die gleiche Skalierung haben.
/// </summary>
[RequireComponent(typeof(StackableBehavior), typeof(XRGrabInteractable))]
public class CookingParentBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _rawItem;
    [SerializeField] private GameObject _cookedItem;
    [SerializeField] private GameObject _burntItem;
    [SerializeField] private GameObject _fire;

    public float cookingTime;
    public float overCookingTime;

    [SerializeField]
    [Tooltip("Sound clip that is played once when the food gets burned")]
    private AudioClip _ignitionAudioSource;

    [SerializeField]
    [Tooltip("Sound clip that is played during burning")]
    private AudioClip _burningAudioSource;

    [SerializeField]
    [Tooltip("Looping sound clip that is played during cooking")]
    private AudioSource _cookingAudioSource;

    [field: FormerlySerializedAs("_xrGrab")]
    [field: SerializeField]
    public XRGrabInteractable XrGrab { get; private set; }

    public bool? Done { get; private set; } = false; // null means it's overcooked
    public Action<CookingParentBehavior, bool> OnCookingStatusChanged { get; set; }

    public float PassedTime { get; private set; } // zu einer public variable machen, wenn es notwendig ist, Werte in Designer anzupassen

    private StackableBehavior _stackableBehavior;
    private GameBehavior _gameBehavior;

    void Start()
    {
        _stackableBehavior = GetComponent<StackableBehavior>();
        _gameBehavior = FindObjectOfType<GameBehavior>();
    }

    void Update()
    {
        if (!_cookingAudioSource.isPlaying)
        {
            return;
        }

        PassedTime += Time.deltaTime;

        if (PassedTime > cookingTime && !Done.GetValueOrDefault(true))
        {
            Done = true;
            _rawItem.SetActive(false);
            _cookedItem.SetActive(true);
            _burntItem.SetActive(false);
            _stackableBehavior.ingredient = "patty";
            OnCookingStatusChanged?.Invoke(this, true);
            _gameBehavior.AddObjectToTask(gameObject, Task.Stacking);
        }

        if (PassedTime > cookingTime + overCookingTime && Done.HasValue)
        {
            Done = null;
            _rawItem.SetActive(false);
            _cookedItem.SetActive(false);
            _burntItem.SetActive(true);
            _stackableBehavior.ingredient = "burned-patty";
            OnCookingStatusChanged?.Invoke(this, true);
            _gameBehavior.RemoveObjectFromTask(gameObject, Task.Stacking);

#if false // aktuell nicht gewünscht
            GameObject model = Instantiate(fire, transform);
            _ignitionAudioSource.PlayOneShot(_ignitionAudioSource.clip, 3f);
            .Play();
#endif
        }
    }

    [ContextMenu(nameof(StartCooking))]
    public void StartCooking()
    {
        _cookingAudioSource.Play();
    }

    [ContextMenu(nameof(StopCooking))]
    public void StopCooking()
    {
        _cookingAudioSource.Stop();
    }
}
