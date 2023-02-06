using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    public float maxHeightOverBase = 1;
    public float ySpeed = 9f;
    public float rotateDegreesPerSecond = 60;
    public float scale;

    private float _originalY;
    private float _scale;

    // Start is called before the first frame update
    void Start()
    {
        _originalY = gameObject.transform.position.y;
        _scale = .5f * scale;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y > _originalY + maxHeightOverBase)
        {
            Vector3 position = gameObject.transform.position;
            position = new Vector3(position.x, _originalY, position.z);
            gameObject.transform.position = position;
            _scale = .5f * scale;
        }

        gameObject.transform.position += Time.deltaTime * Vector3.up / ySpeed;
        gameObject.transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotateDegreesPerSecond));

        _scale += 9 * Time.deltaTime;

        gameObject.transform.localScale = _scale * Vector3.one;
    }
}