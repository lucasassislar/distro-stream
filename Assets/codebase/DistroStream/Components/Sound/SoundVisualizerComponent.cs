using B83.MathHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace DistroStream {
    public class SoundVisualizerComponent : MonoBehaviour {
        public Transform trParent;
        public Transform trLightsParent;
        public float preMultiplier = 1;

        private float timer;
        private AudioClip clip;

        private List<SoundBarComponent> components;
        private List<SoundLightComponent> lights;

        private string deviceName;
        private int lastPos;
        private int pos;

        private AudioSource audioSource;
        private float[] samples;
        private Complex[] spec2;

        private void Start() {
            deviceName = "VoiceMeeter Output (VB-Audio VoiceMeeter VAIO)";

            clip = Microphone.Start(deviceName, true, 10, 44100);

            audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.loop = true;

            samples = new float[64];
            spec2 = new Complex[64];

            components = new List<SoundBarComponent>();
            foreach (Transform tr in trParent) {
                components.Add(tr.GetComponent<SoundBarComponent>());
            }

            lights = new List<SoundLightComponent>();
            foreach (Transform tr in trLightsParent) {
                lights.Add(tr.GetComponent<SoundLightComponent>());
            }

        }

        private void Update() {
            if ((pos = Microphone.GetPosition(deviceName)) > 0) {
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }

                if (lastPos > pos) {
                    lastPos = 0;
                }

                if (pos - lastPos > 0) {
                    // Allocate the space for the sample.
                    float[] audioSamples = new float[(pos - lastPos) * 1];

                    // Get the data from microphone.
                    clip.GetData(audioSamples, lastPos);

                    for (int i = 0; i < spec2.Length; i++) {
                        spec2[i] = new Complex(audioSamples[i], 0);
                    }
                    FFT.CalculateFFT(spec2, false);

                    //audioSource.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

                    for (int i = 0; i < components.Count; i++) {
                        //float sample = samples[i];
                        float sample = (float)spec2[i].magnitude;
                        components[i].ReceiveSample(sample * preMultiplier);
                    }

                    for (int i = 0; i < lights.Count; i++) {
                        //float sample = samples[i * 3];
                        float sample = (float)spec2[i * 3].magnitude;
                        lights[i].ReceiveSample(sample * preMultiplier);
                    }

                    lastPos = pos;
                }
            }
        }
    }
}
