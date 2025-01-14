﻿using EventsWebApp.API.Domain.Entities;

namespace EventsWebApp.API.Domain
{
    public class Participant
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
