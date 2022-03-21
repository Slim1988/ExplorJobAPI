using System;

namespace ExplorJobAPI.Domain.Models.AccountUsers
{
    public class AccountUserMessage
    {
        public string Id { get; set; }
        public string EmitterId { get; set; }
        public bool FromOwner { get; set; }
        public bool FromInterlocutor { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime Date { get; set; }

        public AccountUserMessage(
            string id,
            string emitterId,
            bool fromOwner,
            bool fromInterlocutor,
            string content,
            bool read,
            DateTime date
        ) {
            Id = id;
            EmitterId = emitterId;
            FromOwner = fromOwner;
            FromInterlocutor = fromInterlocutor;
            Content = content;
            Read = read;
            Date = date;
        }
    }
}
