using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TrashableBehavior : MonoBehaviour
{
    void Start()
    {
        gameObject.AddGrabableComponents();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Trashcan")
        {
            Destroy(gameObject);
        }
    }
}