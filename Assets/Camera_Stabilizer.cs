using UnityEngine;

public class Camera_Stabilizer : MonoBehaviour
{
    public Transform camera;
    public Transform character;
    Vector3 desiredPos;
    public Vector3 offset;
    


    void Start()
    {
        desiredPos = character.position + offset;
        camera.position = desiredPos;
    }

    void Update()
    {
        desiredPos = character.position + offset;
        camera.position = desiredPos;
        camera.rotation = Quaternion.LookRotation(character.position);
    }
}