using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Instance = null;
        public Transform Target = null;
        public Vector3 Offset = Vector3.zero;
        public Vector3 RotationEuler = Vector3.zero;
        public Vector3 Speed = Vector3.one;
        [SerializeField] private bool freezeX = false;
        [SerializeField] private bool freezeY = false;
        [SerializeField] private bool freezeZ = false;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
        }

        private void FixedUpdate()
        {
            /*if (Target)
                Follow();*/
        }

        private void LateUpdate()
        {
            if (Target)
            {
                Follow();
            }


        }

        private void Follow()
        {
            Vector3 pos = Target.position + Offset;
            if (freezeX)
                pos.x = transform.position.x;
            if (freezeY)
                pos.y = transform.position.y;
            if (freezeZ)
                pos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, transform.position + (pos - transform.position).x * Vector3.right, Time.deltaTime * Speed.x);
            transform.position = Vector3.Lerp(transform.position, transform.position + (pos - transform.position).y * Vector3.up, Time.deltaTime * Speed.y);
            transform.position = Vector3.Lerp(transform.position, transform.position + (pos - transform.position).z * Vector3.forward, Time.deltaTime * Speed.z);
        }

        public void TakeRotation()
        {
            transform.LeanRotate(RotationEuler, 1);
        }

    }

}
