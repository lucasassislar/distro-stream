using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistroStream {

    public class Spin : MonoBehaviour {
        public float speed = 5;

        // Update is called once per frame
        void Update() {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}