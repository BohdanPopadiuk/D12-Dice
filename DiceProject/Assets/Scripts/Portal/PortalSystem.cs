using System.Collections;
using DG.Tweening;
using ObjectsPool;
using UnityEngine;
public class PortalSystem : MonoBehaviour
{
    [SerializeField] private Transform topTeleportPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dice"))
        {
            StartCoroutine(Teleportation(other));
        }
    }

    IEnumerator Teleportation(Collider col)
    {
        Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
        Vector3 defaultScale = rb.transform.localScale;
        Vector3 velocity = rb.velocity;
        
        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;
        
        Transform portalEffect1 = PortalPoolBase.Instance.PortalEffectPool.Get().transform;
        portalEffect1.position = rb.transform.position + Vector3.up * 0.8f;
        portalEffect1.localEulerAngles = new Vector3(0, Quaternion.LookRotation(rb.velocity.normalized).y, 0);

        rb.transform.DOScale(Vector3.zero, .4f);
        
        yield return new WaitForSeconds(.5f);
        
        Transform portalEffect2 = PortalPoolBase.Instance.PortalEffectPool.Get().transform;
        portalEffect2.position = topTeleportPos.position;
        portalEffect2.rotation = Quaternion.LookRotation(Vector3.down.normalized);
            
        rb.transform.position = topTeleportPos.position;
        
        rb.transform.DOScale(defaultScale, .2f);
        
        yield return new WaitForSeconds(.25f);
        
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
        rb.velocity = velocity / 5;
        rb.AddTorque(30000,30000,30000);
    }
}
