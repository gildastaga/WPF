using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace Msn.Model {
    public class Registration : EntityBase<MsnContext> {
        public int RegistrationId { get; set; }

        public enum State { ACTIVE, VALIDE, INACTIVE }

        [Required]
        public virtual Student Student { get; set; }

        [Required]
        public virtual Course Course { get; set; }

        public virtual ICollection<Proposition> ChoosedProposition { get; set; } = new HashSet<Proposition>();
    }
}
