using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace DistroStream {
    public class SoundLightComponent : MonoBehaviour {
        public float bias = 0.05f;
        public float factor = 5f;
        public float timeToBeat = 0.05f;
        public float cooldown = 0.15f;
        public float falloffSpeed = 0.2f;

        private float startSize;
        private float maxSize;

        private float timer = 0;
        private bool beat = false;
        private HDAdditionalLightData lightData;
        private float startIntensity;

        private Vector3 startPosition;

        private void Awake() {
            lightData = GetComponent<HDAdditionalLightData>();
            startIntensity = lightData.intensity;
            startPosition = transform.position;
            startSize = lightData.shapeHeight;
        }

        private void Update() {
            timer += Time.deltaTime;

            float size;
            if (beat) {
                size = Mathf.Lerp(0, maxSize, timer / timeToBeat);

                if (timer > timeToBeat) {
                    beat = false;
                }
            } else {
                size = lightData.shapeHeight;
                size -= falloffSpeed * Time.deltaTime;
                size = Math.Max(0, size);
            }

            lightData.intensity = Mathf.Lerp(0, startIntensity, size / startSize);

            lightData.shapeHeight = Math.Max(0.5f, size);

            transform.position = startPosition - new Vector3(0, (startSize - lightData.shapeHeight) / 2.0f, 0);
        }

        public void ReceiveSample(float sample) {
            if (beat) {
                return;
            } else if (sample < bias) {
                return;
            }

            maxSize = Math.Min(startSize, startSize * sample * factor);

            beat = true;
            timer = 0;
        }
    }
}
