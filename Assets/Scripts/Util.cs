using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

static class Util
{
    public static GrabableGameObject AddGrabableComponents(this GameObject go)
    {
        go.AddComponent<MeshCollider>().convex = true;
        Rigidbody rigid = go.AddComponent<Rigidbody>();
        XRGrabInteractable grab = go.AddComponent<XRGrabInteractable>();
        grab.attachTransform = go.transform.Find("Attatch Transform");

        return new GrabableGameObject
        {
            GameObject = go, 
            XrGrab = grab,
            Rigidbody = rigid
        };
    }
}

public struct GrabableGameObject
{
    public GameObject GameObject;
    public XRGrabInteractable XrGrab;
    public Rigidbody Rigidbody;
}