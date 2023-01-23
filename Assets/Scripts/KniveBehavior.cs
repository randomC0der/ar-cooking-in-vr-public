using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

class KniveBehavior : MonoBehaviour
{
    bool _insideCuttingBoard = false;

    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource= GetComponent<AudioSource>();
    }

    public void EnterCuttingBoardCollision(IXRInteractable iteractable)
    {
        if (_insideCuttingBoard || iteractable == null) // || !isKnive
        {
            return;
        }

        CuttingBehavior cuttingBehavior = iteractable.transform.gameObject.GetComponent<CuttingBehavior>();
        if (cuttingBehavior != null)
        {
            //_audioSource?.PlayOneShot(_audioSource.clip);
            cuttingBehavior.Cut();
        }
        _insideCuttingBoard = true;
    }

    public void ExitCuttingBoardCollision()
    {
        _insideCuttingBoard = false;
    }
}
