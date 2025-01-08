using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ObjectExplode : MonoBehaviour
{
    [SerializeField] GameObject particleEffect;
    [SerializeField] GameObject anyObject;
    public ParticleSystem ParticleSystem;
    bool collided = false;
    public MeshRenderer objectMesh;
    public BoxCollider objectCollider;

    // Start is called before the first frame update
    void Start()
    {
        particleEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ParticleSystem.isPlaying == true && collided == true)
        {
            objectMesh.enabled = false;
            objectCollider.enabled = false;
        }
        if (ParticleSystem.isPlaying == false && collided == true)
        {
            anyObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(GameManager.activateCollider);
        if (GameManager.activateCollider == true)
        {
            particleEffect.SetActive(true);
            collided = true;
        }
    }
}
