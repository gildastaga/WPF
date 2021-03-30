using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {

    public enum Type {
        True,
        False
    }

    public class Proposition : EntityBase<ModelSchool04> {
        public int PropositionId { get; set; }
        public string Body { get; set; }
        public Type type { get; set; } = Type.True;

        public Proposition(string body) {
            Body = body;
        }
    }
}
