namespace ZakaraiMe.Data.Entities.Implementations
{
    using Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Message : IBaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual User Sender { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }

        public virtual User Receiver { get; set; }

        [ForeignKey(nameof(Receiver))]
        public int ReceiverId { get; set; }
    }
}
