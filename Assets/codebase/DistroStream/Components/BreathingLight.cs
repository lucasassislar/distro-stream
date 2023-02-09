using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.HighDefinition;

namespace DistroStream {
    public class BreathingLight : MonoBehaviour {
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float duration = 10;
        public float maxIntensity = 2;
        public float minIntensity = 1;
        private HDAdditionalLightData lightData;
        private float timer;

        private void Awake() {
            lightData = GetComponent<HDAdditionalLightData>();
        }

        private void Update() {
            timer += Time.deltaTime;

            if (timer > duration) {
                timer -= duration;
            }

            float lerp = timer / duration;
            float curveResult = curve.Evaluate(lerp);

            float intensity = Mathf.Lerp(minIntensity, maxIntensity, curveResult);
            lightData.intensity = intensity;
        }
    }
}
