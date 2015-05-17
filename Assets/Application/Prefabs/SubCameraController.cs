using UnityEngine;
using System.Collections;

public class SubCameraController : MonoBehaviour {

    static SubCameraController subCamera;
    public static SubCameraController Instance
    {
        get
        {
            return subCamera;
        }
    }

    [SerializeField]
    public Camera
        _camera;

    void Awake()
    {
        if(subCamera == null)
        {
            subCamera = this;
        }
    }
}
