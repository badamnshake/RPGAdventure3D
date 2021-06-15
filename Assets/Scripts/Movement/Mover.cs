using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Atrributes;
using GameDevTV.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Health health;
        [SerializeField] float maxNavPathLength = 40f;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }
        void Start()
        {
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            // local velocity filters out global values
            Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            // here z axis velocity is the velocity of the player
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        // start move action starts movement and stops the lock was on target
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 point, float speedFraction)
        {

            navMeshAgent.isStopped = false;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.destination = point;
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        // Saving system
        public object CaptureState()
        {
            // to save its needed an object to be serialized
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            // object casting throws and exception if problems
            SerializableVector3 position = (SerializableVector3)state;

            // restoring position

            navMeshAgent.enabled = false;
            // szable to normal vector3
            transform.position = position.ToVector();
            navMeshAgent.enabled = true;
        }
        public bool CanMoveTo(Vector3 target) {
            
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

    }
}