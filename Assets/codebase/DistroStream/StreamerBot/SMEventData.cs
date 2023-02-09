using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroStream {
    //"event":{"source":"Twitch","type":"Follow"},"data":{"user_id":"51155528","user_login":"ttvlsl","user_name":"TTVLSL","followed_at":"2023-02-07T02:12:56.7844459Z"}}

    //"data":{"subTier":0,"color":"#008D99","emotes":[],"badges":[{"name":"subscriber","version":"0","imageUrl":"https://static-cdn.jtvnw.net/badges/v1/5d9f2208-5dd8-11e7-8513-2ff4adfae661/3"
    //"data":{ "cumulativeMonths":3,"shareStreak":true,"streakMonths":1,"subTier":0,"color":"#FF4500","emotes":[],"badges":[{ "name":"subscriber","version":"0","imageUrl":"https://static-cdn.jtvnw.net/badges/v1/5d9f2208-5dd8-11e7-8513-2ff4adfae661/3"},{ "name":"premium","version":"1","imageUrl":"https://static-cdn.jtvnw.net/badges/v1/bbbe0db0-a598-423e-86d0-f9fb98ca1933/3"}],"message":"","userId":"104698895","userName":"distrolucas","displayName":"DistroLucas","role":1}}
    
    public class SMEventData {
        public int cumulativeMonths;
        public bool shareStreak;
        public int streakMonths;

        public int subTier;
        public string color;
        public string[] emotes;
        public string message;
        public string userId;
        public string userName;
        public string displayName;
        public int role;

        public string user_id;
        public string user_login;
        public string user_name;
        public string followed_at;
    }
}
