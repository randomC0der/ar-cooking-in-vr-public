using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts
{
    class KniveBehavior : MonoBehaviour
    {
        bool _insideCuttingBoard;

        private AudioSource _audioSource;
        private Transform _attatch;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _attatch = new GameObject("Attatch Transform").transform;
            _attatch.transform.parent = gameObject.transform;
            _attatch.localPosition = new Vector3(0f, 0.16f, 0f);
            
        }

        public void EnterCuttingBoardCollision(HoverEnterEventArgs e)
        {
            XRBaseInteractor interactor = e.interactor;
            bool isKnive = e.interactableObject.transform.gameObject == gameObject;
            if (!interactor.hasSelection)
            {
                // sorgt dafür, dass die Preview korrekt gerendert wird
                gameObject.GetComponent<XRGrabInteractable>().attachTransform = _attatch;
                return;
            }
            if (!_insideCuttingBoard && isKnive)
            {
                CuttingBehavior cuttingBehavior = interactor.firstInteractableSelected.transform.gameObject.GetComponent<CuttingBehavior>();
                if (!(cuttingBehavior is null))
                {
                    _audioSource.PlayOneShot(_audioSource.clip);
                    cuttingBehavior.Cut();
                }
                _insideCuttingBoard = true;
            }
        }

        public void ExitCuttingBoardCollision()
        {
            gameObject.GetComponent<XRGrabInteractable>().attachTransform = null;
            _insideCuttingBoard = false;
        }
    }
}
