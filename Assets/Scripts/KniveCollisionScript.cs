using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KniveCollisionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _knive;

    private XRSocketInteractor _socketInteractor;
    private AudioSource _audioSource;
    bool _insideCuttingBoard = false; 

    private void Start()
    {
        _socketInteractor = GetComponent<XRSocketInteractor>();
        _audioSource = GetComponent<AudioSource>();

        _socketInteractor.hoverEntered.AddListener(HoverEntered);
        _socketInteractor.hoverExited.AddListener(HoverExited);
    }

    private void HoverEntered(HoverEnterEventArgs e)
    {
        var cuttingBehavior = e.interactableObject.transform.gameObject.GetComponent<CuttingBehavior>();
        if (cuttingBehavior != null)
        {
            cuttingBehavior.OnTriggerEnterAction = OnObjectTriggerEnter;
            cuttingBehavior.OnTriggerExitAction = OnObjectTriggerExit;
        }
    }

    private void HoverExited(HoverExitEventArgs e)
    {
        var cuttingBehavior = e.interactableObject.transform.gameObject.GetComponent<CuttingBehavior>();
        if (cuttingBehavior != null)
        {
            cuttingBehavior.OnTriggerEnterAction = null;
            cuttingBehavior.OnTriggerExitAction = null;
        }
    }

    private void OnObjectTriggerEnter(Collider other)
    {
        if(other.gameObject.name == _knive.name)
        {
            EnterCutCollision(_socketInteractor.firstInteractableSelected);
        }
    }

    private void OnObjectTriggerExit(Collider other)
    {
        if (other.gameObject.name == _knive.name)
        {
            ExitCutCollision();
        }
    }

    public void EnterCutCollision(IXRInteractable iteractable)
    {
        if (_insideCuttingBoard || iteractable == null)
        {
            return;
        }

        CuttingBehavior cuttingBehavior = iteractable.transform.gameObject.GetComponent<CuttingBehavior>();
        if (cuttingBehavior != null)
        {
            _audioSource?.PlayOneShot(_audioSource.clip);
            cuttingBehavior.Cut();
        }
        _insideCuttingBoard = true;
    }

    public void ExitCutCollision()
    {
        _insideCuttingBoard = false;
    }
}
