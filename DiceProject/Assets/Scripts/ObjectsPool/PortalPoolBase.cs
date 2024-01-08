using System;
using UnityEngine;

namespace ObjectsPool
{
    public class PortalPoolBase: MonoBehaviour
    {
        public static PortalPoolBase Instance;
        [SerializeField] private PortalEffect portalEffectPrefab;
        public GameObjectPool PortalEffectPool { get; private set; }
        

        private void Awake()
        {
            Instance = this;
            if(Instance != null) Destroy(this);

            PortalEffectPool = new GameObjectPool(portalEffectPrefab.gameObject, 5);
        }
    }
}