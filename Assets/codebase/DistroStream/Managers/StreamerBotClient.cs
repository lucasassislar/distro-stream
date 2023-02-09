using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace DistroStream {
    public class StreamerBotClient : MonoBehaviour {
        private WebSocket ws;
        private Guid guid;

        public FollowBadgeManager followBadgeManager;

        public FollowEventManager followManager;
        public SubEventManager subManager;

        private void Awake() {
            ws = new WebSocket("ws://127.0.0.1:8080/");
            ws.Connect();

            ws.OnOpen += Ws_OnOpen;
            ws.OnMessage += OnMessage;
            ws.OnError += Ws_OnError;

            guid = Guid.NewGuid();

            SMRequest request = new SMRequest();
            request.request = "Subscribe";
            request.id = guid.ToString();
            request.events.Add("Twitch", new string[] { "Follow", "Sub", "Resub" });

            string requestStr = JsonConvert.SerializeObject(request);

            ws.Send(requestStr);
        }

        private void Ws_OnError(object sender, ErrorEventArgs e) {
            Debug.Log("OnError");
        }

        private void Ws_OnOpen(object sender, System.EventArgs e) {
            Debug.Log("OnOpen");
        }

        private void OnMessage(object sender, MessageEventArgs args) {
            WebSocket socket = (WebSocket)sender;
            Debug.Log("Message Received from " + socket.Url + ", Data : " + args.Data);

            try {
                SMMessage message = JsonConvert.DeserializeObject<SMMessage>(args.Data);

                if (message._event.type == "Follow") {
                    string userName = message.data.user_name;

                    followBadgeManager?.StartNewFollow(userName);
                    followManager?.StartNewFollow(userName);
                } else if (message._event.type == "Sub") {
                    string userName = message.data.displayName;
                    subManager.StartNewSub(userName);
                } else if (message._event.type == "ReSub") {
                    string userName = message.data.displayName;
                    subManager.StartPreviousSub(userName, message.data.cumulativeMonths);
                }
            } catch (Exception ex) {
                Debug.Log($"Error: {ex.Message}");
            }
        }
    }
}