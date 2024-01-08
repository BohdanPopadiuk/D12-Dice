using System.Collections;
using DG.Tweening;
using ObjectsPool;
using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1.5f;
    [SerializeField] private Ease portalEase = Ease.OutQuad;
    private void OnEnable()
    {
        StartCoroutine(ShowPortalEffect());
    }

    private IEnumerator ShowPortalEffect()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.2f).SetEase(portalEase);
        yield return new WaitForSeconds(timeToDestroy);
        transform.DOScale(Vector3.zero, 0.2f).SetEase(portalEase);
        yield return new WaitForSeconds(0.25f);
        PortalPoolBase.Instance.PortalEffectPool.Return(gameObject);
    }
}
