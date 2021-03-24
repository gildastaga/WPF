using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.Model {

    public enum Type {
        True,
        False
    }

    public class Proposition {
        public int PropositionId { get; set; }
        public string Body { get; set; }
        public Type type { get; set; } = Type.True;

        public Proposition(string body) {
            Body = body;
        }
    }
}
