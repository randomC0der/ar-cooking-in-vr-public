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

        public void EnterCuttingBoardCollision(HoverEnterEventArgs e)
        {
            XRBaseInteractor interactor = e.interactor;
            if (interactor.hasSelection && !_insideCuttingBoard)
            {
                CuttingBehavior cuttingBehavior = interactor.firstInteractableSelected.transform.gameObject.GetComponent<CuttingBehavior>();
                if (!(cuttingBehavior is null)) {
                    cuttingBehavior.Cut();
                }
            }
            _insideCuttingBoard = true;
        }

        public void ExitCuttingBoardCollision()
        {
            _insideCuttingBoard = false;
        }
    }
}
