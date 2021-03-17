using System;
using System.ComponentModel.DataAnnotations;
using PRBD_Framework;

namespace prbd_msn_tuto {
    public class Message : EntityBase<Model> {
        public int MessageId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Body { get; set; }
        public bool IsPrivate { get; set; }

        [Required]
        public virtual Member Author { get; set; }
        [Required]
        public virtual Member Recipient { get; set; }

        public Message(Member author, Member recipient, string body, bool isPrivate = false, DateTime? datetime = null) {
            Author = author;
            author?.MessagesSent.Add(this);
            Recipient = recipient;
            recipient?.MessagesReceived.Add(this);
            Body = body;
            IsPrivate = isPrivate;
            DateTime = datetime ?? DateTime.Now;
        }

        protected Message() { }
    }
}