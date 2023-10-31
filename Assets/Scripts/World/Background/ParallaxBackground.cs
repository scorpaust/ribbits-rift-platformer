using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform theCam;

    [SerializeField] private Transform theSky;

    [SerializeField] private Transform theTreeline;

    [SerializeField][Range(0f, 1f)] private float parallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        theSky.position = new Vector3(theCam.position.x, theCam.position.y, theSky.position.z);

        theTreeline.position = new Vector3(theCam.position.x * parallaxSpeed, theCam.position.y * parallaxSpeed, theTreeline.position.z);
    }
}
