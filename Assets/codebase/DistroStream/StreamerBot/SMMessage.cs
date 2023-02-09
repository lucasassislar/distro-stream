using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroStream {
    //{"timeStamp":"2023-02-06T23:12:56.7844459-03:00","event":{"source":"Twitch","type":"Follow"},"data":{"user_id":"51155528","user_login":"ttvlsl","user_name":"TTVLSL","followed_at":"2023-02-07T02:12:56.7844459Z"}}
    public class SMMessage {
        public string timeStamp;

        [JsonProperty("event")]
        public SMEvent _event;

        public SMEventData data;

        public SMMessage() {
        }
    }
}
