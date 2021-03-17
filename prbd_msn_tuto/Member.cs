using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;

namespace prbd_msn_tuto {
    public class Member : EntityBase<Model> {
        [Key]
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public string Profile { get; set; }
        public bool IsAdmin { get; set; }
        public string PicturePath { get; set; }

        [NotMapped]
        public string AbsolutePicturePath {
            get { return PicturePath != null ? App.IMAGE_PATH + "\\" + PicturePath : null; }
        }

        public virtual ICollection<Member> Followees { get; set; } = new HashSet<Member>();
        public virtual ICollection<Member> Followers { get; set; } = new HashSet<Member>();

        public virtual ICollection<Message> MessagesSent { get; set; } = new HashSet<Message>();
        public virtual ICollection<Message> MessagesReceived { get; set; } = new HashSet<Message>();

        public Member(string pseudo, string password, string profile = "", bool isAdmin = false, string picturePath = null) {
            Pseudo = pseudo;
            Password = password;
            Profile = profile;
            IsAdmin = isAdmin;
            PicturePath = picturePath;
        }

        protected Member() { }

        public override string ToString() {
            return $"<Member: Pseudo={Pseudo}, " +
                $"#followees={Followees.Count}, " +
                $"#followers={Followers.Count}, " +
                $"#sent={MessagesSent.Count}, " +
                $"#received={MessagesReceived.Count}>";
        }

        public void Follow(Member member) {
            Followees.Add(member);
            member.Followers.Add(this);
            Context.SaveChanges();
        }

        public void Unfollow(Member member) {
            Followees.Remove(member);
            member.Followers.Remove(this);
            Context.SaveChanges();
        }

        public Message Send(Member recipient, string body, bool isPrivate = false) {
            var msg = new Message(this, recipient, body, isPrivate);
            Context.Messages.Add(msg);
            Context.SaveChanges();
            return msg;
        }

        public void Delete() {
            Followees.Clear();
            Followers.Clear();
            // Supprime les messages envoyés ou reçus
            Context.Messages.RemoveRange(MessagesSent);
            MessagesSent.Clear();
            Context.Messages.RemoveRange(MessagesReceived);
            MessagesReceived.Clear();
            // Supprime le membre lui-même
            Context.Members.Remove(this);
            Context.SaveChanges();
        }
    }
}