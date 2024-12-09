using UnityEngine;

public class Camera_Stabilizer : MonoBehaviour
{
    public Camera camera;
    public Transform character;


    void Start()
    {
        camera.transform.position = character.position;
        camera.transform.rotation = character.rotation;
    }

    void Update()
    {
        camera.transform.position = character.position;
        camera.transform.rotation = character.rotation;
    }
}