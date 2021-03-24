using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.Model {
    public class Category {
        public int CategoryId { get; set; }
        public string Title { get; set; }

        public Category(string title) {
            Title = title;
        }
    }
}
