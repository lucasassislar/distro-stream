using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistroStream {
    public class SMRequest {
        public string request;
        public string id;
        public Dictionary<string, string[]> events;

        public SMRequest() {
            events = new Dictionary<string, string[]>();
        }
    }
}
