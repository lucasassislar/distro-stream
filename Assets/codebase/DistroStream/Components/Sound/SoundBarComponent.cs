using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DistroStream {
    public class SoundBarComponent : MonoBehaviour {
        public float bias = 0.05f;
        public float factor = 5f;
        public float timeToBeat = 0.05f;
        public float cooldown = 0.15f;
        public float falloffSpeed = 0.2f;

        private float startMaxSize = 6.7f;
        private float maxSize;

        private float timer = 0;
        private bool beat = false;
        
        private void Update() {
            timer += Time.deltaTime;

            float size;
            if (beat) {
                size = Mathf.Lerp(0, maxSize, timer / timeToBeat);
                if (timer > timeToBeat) {
                    beat = false;
                }
            } else {
                size = transform.localScale.y;
                size -= falloffSpeed * Time.deltaTime;
                size = Math.Max(0, size);
            }

            transform.localScale = new Vector3(0.95f, Math.Max(0.2f, size), 0.16857f);
        }

        public void ReceiveSample(float sample) {
            if (beat) {
                return;
            } else if (sample < bias) {
                return;
            }

            maxSize = Math.Min(startMaxSize, startMaxSize * sample * factor);

            beat = true;
            timer = 0;
        }
    }
}
