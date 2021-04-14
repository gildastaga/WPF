﻿using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Category : EntityBase<ModelSchool04> {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Question> Questions {
            get; set;
        } = new HashSet<Question>();
        public Category() {
        }
        public Category(string title) {
            Title = title;
        }
    }
}