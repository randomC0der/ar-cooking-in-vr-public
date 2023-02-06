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
    public Action<CookingParentBehavior> OnCookingStatusChanged { get; set; }

    public float PassedTime { get; private set; } // zu einer public variable machen, wenn es notwendig ist, Werte in Designer anzupassen

    void Start()
    {
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
            var cooked = Instantiate(_cookedItem, transform);
            Destroy(_rawItem);
            OnCookingStatusChanged?.Invoke(this);
        }

        if (PassedTime > cookingTime + overCookingTime && Done.HasValue)
        {
            Done = null;
            var burnt = Instantiate(_burntItem, transform);
            Destroy(_cookedItem);
            OnCookingStatusChanged?.Invoke(this);

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
