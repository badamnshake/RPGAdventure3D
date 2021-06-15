using System.Collections;
using System.Collections.Generic;
using RPG.Atrributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target = null;
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent OnHit;

        GameObject instigator = null;

        float damage = 0;

        private void Start()
        {
            transform.LookAt(target.transform.position);
        }
        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(target.transform.position);
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        public void SetTarget(Health target, GameObject instigator , float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (target.IsDead()) return;
            if (other.GetComponent<Health>() != target) return;
            target.TakeDamage(damage, instigator);
            speed = 0;
            OnHit.Invoke();
            if (hitEffect != null)
            {
                Instantiate(hitEffect, other.transform.position, transform.rotation);
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
        // private Vector3 GetAimLocation()
        // {
        //     CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        //     if (targetCapsule == null) return target.transform.position;
        //     return target.transform.position + Vector3.up * targetCapsule.height / 2;
        // }
    }
}