using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DistroStream {
    public class SubEventManager : MonoBehaviour {
        public float firstDuration = 1;
        public float duration = 3;

        public Writer writer;
        public Material textMaterial;

        public Transform trParent;

        private int nextSub = -1;
        private List<string> newSubs;
        private List<int> months;

        public AnimationCurve firstCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve secondCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private float timer;
        private string nextUserName;

        public float textSize = 2;
        public float maxRandomWidth = 4;

        private void Awake() {
            timer = duration;
            newSubs = new List<string>();
            months = new List<int>();
        }

        private void Update() {
            if (nextSub == -1 && newSubs.Count != 0) {
                timer = 0;

                nextSub = 0;
                nextUserName = newSubs[0];
                int totalMonths = months[0];
                newSubs.RemoveAt(0);
                months.RemoveAt(0);

                GameObject gNewSub = new GameObject($"Sub_{nextUserName}");
                gNewSub.transform.parent = trParent;
                gNewSub.transform.localPosition = new Vector3(Random.Range(-maxRandomWidth, maxRandomWidth), 0, 0);
                writer.empty = gNewSub;
                writer.Textbox = nextUserName;
                writer.spaceWidth = 0.3f;
                writer.CreateWord();

                BoxCollider boxCollider = gNewSub.AddComponent<BoxCollider>();
                Rigidbody rigidBody = gNewSub.AddComponent<Rigidbody>();
                rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

                Material clonedMat = UnityEngine.Object.Instantiate(textMaterial);
                float intensity = totalMonths * 0.4f;
                clonedMat.SetColor("_EmissiveColor", Color.white * intensity);

                float size = 0.4f * nextUserName.Length;
                foreach (Transform tr in gNewSub.transform) {
                    Destroy(tr.GetComponent<MeshCollider>());

                    tr.Rotate(0, 180, 0);
                    MeshRenderer rend = tr.GetComponent<MeshRenderer>();
                    rend.sharedMaterial = clonedMat;
                }

                boxCollider.center = new Vector3(size / 2.0f, 0.28f, 0);
                boxCollider.size = new Vector3(size, 0.6f, 1);
            }

            timer += Time.deltaTime;
            if (timer >= duration) {
                nextSub = -1;
                return;
            }
        }

        public void StartNewSub(string userName) {
            newSubs.Add(userName);
            months.Add(0);
        }

        public void StartPreviousSub(string userName, int totalMonths) {
            newSubs.Add(userName);
            months.Add(totalMonths);
        }
    }
}
