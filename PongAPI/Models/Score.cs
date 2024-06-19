using Microsoft.EntityFrameworkCore;
using System;

namespace CyberPong.PongAPI.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; } // Adicionado o using para DateTime
    }
}
