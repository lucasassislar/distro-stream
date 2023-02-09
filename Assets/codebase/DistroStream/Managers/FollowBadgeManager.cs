using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace DistroStream {
    public class FollowBadgeManager : MonoBehaviour {
        public PlayableDirector director;
        public Writer writer;
        public Transform trNomeSeguidor;
        public Material matText;

        private List<string> queuedNames;
        private GameObject objName;

        private void Awake() {
            queuedNames = new List<string>();
        }

        private void Update() {
            if (queuedNames.Count > 0) {
                if (director.state != PlayState.Playing) {
                    if (objName) {
                        GameObject.Destroy(objName);
                    }

                    string queuedName = queuedNames[0];
                    queuedNames.RemoveAt(0);

                    objName = new GameObject();
                    objName.transform.parent = trNomeSeguidor;
                    objName.transform.localRotation = Quaternion.identity;

                    objName.transform.localPosition = Vector3.zero;
                    //float size = 0.0005f * queuedName.Length;
                    //objName.transform.localPosition = new Vector3(-size, 0, 0);

                    writer.empty = objName;
                    writer.Textbox = queuedName;
                    writer.CreateWord();

                    foreach (Transform tr in objName.transform) {
                        Destroy(tr.GetComponent<MeshCollider>());
                        MeshRenderer rend = tr.GetComponent<MeshRenderer>();
                        rend.sharedMaterial = matText;
                    }

                    director.time = 0;
                    director.Stop();
                    director.Evaluate();
                    director.Play();
                }
            }
        }

        public void StartNewFollow(string userName) {
            queuedNames.Add(userName);
        }
    }
}
