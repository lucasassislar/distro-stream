using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DistroStream {
    public class FollowEventManager : MonoBehaviour {
        public float firstDuration = 1;
        public float duration = 3;
        public TMP_Text text;
        
        private int nextFollower = -1;
        private List<string> newFollowers;

        public AnimationCurve firstCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve secondCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private float timer;
        private string nextUserName;

        private void Awake() {
            timer = duration;
            newFollowers = new List<string>();
        }

        private void Update() {
            if (nextFollower == -1 && newFollowers.Count != 0) {
                timer = 0;

                nextFollower = 0;
                nextUserName = newFollowers[0];
                newFollowers.RemoveAt(0);
            }

            timer += Time.deltaTime;
            if (timer >= duration) {
                nextFollower = -1;
                return;
            }

            float lerp = 0;
            if (timer < firstDuration) {
                lerp = timer / firstDuration;
                lerp = firstCurve.Evaluate(lerp);

                text.color = new Color(1, 1, 1, 1 - lerp);
            } else if (timer < duration) {
                lerp = (timer - firstDuration) / duration;
                lerp = secondCurve.Evaluate(lerp);

                text.text = nextUserName;
                text.color = new Color(1, 1, 1, lerp);
            }

        }

        public void StartNewFollow(string userName) {
            newFollowers.Add(userName);
        }
    }
}
