namespace ZakaraiMe.Web.Models.Messages
{
    public class MessageViewModel : ListViewModel
    {
        public string Text { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }
    }
}
