using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Unity.Netcode;

public class ObjectExplode : NetworkBehaviour
{
    [SerializeField] GameObject object1;
    public GameObject particle;
    ObjectGrabable objectGrabable;
    


    // Start is called before the first frame update
    void Start()
    {
        objectGrabable = GetComponent<ObjectGrabable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (objectGrabable.GetactivateCollier() == true) {
            ExplodeServerRpc();
            ExplodeClientRpc();
        }
        objectGrabable.SetActivateCollider(false);
    }

    [ServerRpc]
    private void ExplodeServerRpc() {
        soundManager.PlaySound(SoundType.IMPACT);
        GameObject particleInstance = Instantiate(particle, transform.position, Quaternion.Euler(90, 0, 0));
        ParticleSystem parts = particle.GetComponent<ParticleSystem>();
        float totalDuration = parts.duration + parts.startLifetime;
        object1.SetActive(false);
        Destroy(particleInstance, totalDuration);
    }

    [ClientRpc]
    private void ExplodeClientRpc() {
        soundManager.PlaySound(SoundType.IMPACT);
        GameObject particleInstance = Instantiate(particle, transform.position, Quaternion.Euler(90, 0, 0));
        ParticleSystem parts = particle.GetComponent<ParticleSystem>();
        float totalDuration = parts.duration + parts.startLifetime;
        object1.SetActive(false);
        Destroy(particleInstance, totalDuration);
    }
}
