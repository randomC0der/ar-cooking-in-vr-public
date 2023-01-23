using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHider : MonoBehaviour
{
    public GameObject handObject = null;

    [SerializeField]
    private XRDirectInteractor interactor = null;

    private void Awake()
    {
        interactor ??= GetComponent<XRDirectInteractor>();
    }

    private void OnEnable()
    {
        interactor.selectEntered.AddListener(Hide);
        interactor.selectExited.AddListener(Show);
    }

    private void OnDisable()
    {
        interactor.selectEntered.RemoveListener(Hide);
        interactor.selectExited.RemoveListener(Show);
    }

    private void Show(SelectExitEventArgs e)
    {
        handObject.SetActive(true);
    }

    private void Hide(SelectEnterEventArgs e)
    {
        handObject.SetActive(false);
    }
}
