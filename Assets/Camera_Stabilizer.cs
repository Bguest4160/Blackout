using UnityEngine;

public class Camera_Stabilizer : MonoBehaviour
{
    public Transform camera;
    public Transform character;
    public Vector3 offset;
    Vector3 desiredPos;


    void Start()
    {
        desiredPos = character.position + offset;
        camera.position = desiredPos;
    }

    void Update()
    {
        desiredPos = character.position + offset;
        camera.position = desiredPos;
        camera.rotation = character.rotation;
    }
}