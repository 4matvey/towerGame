using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject _camera;
    public static CameraController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    //slides camera up smoothly
    //partial movements are spread out over time to prevent sharp jumps
    public IEnumerator MoveCamera()
    {
        float totalMovement = 3f;      //totalMovment is the full distance the camera will move                                 
        float partialMovement = 0.06f; //totalMovement will be split up into smaller partialMovements
        while (totalMovement > 0)
        {
            totalMovement -= partialMovement;
            _camera.transform.Translate(0, partialMovement, 0, Space.World);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
