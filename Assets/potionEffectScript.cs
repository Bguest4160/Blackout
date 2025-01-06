using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class potionEffectScript : MonoBehaviour
{
    [SerializeField] LayerMask players;
    [SerializeField] GameObject particleEffect;
    [SerializeField] GameObject potion;
    public ParticleSystem ParticleSystem;
    bool collided = false;
    public MeshRenderer potionMesh;
    public CapsuleCollider potionCollider;

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
            potionMesh.enabled = false;
            potionCollider.enabled = false;
        }
        if (ParticleSystem.isPlaying == false && collided == true)
        {
            potion.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(GameManager.activateCollider);
        if (GameManager.activateCollider == true)
        {
            particleEffect.SetActive(true);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, players);
            //particleEffect.SetActive(false);

            foreach (Collider c in colliders)
            {
                if (c.GetComponent<MovementScript>())
                {
                    c.GetComponent<MovementScript>().ChangeStats();
                }
            }
            collided = true;
        }
    }
}
