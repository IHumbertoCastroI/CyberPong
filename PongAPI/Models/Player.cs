using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CyberPong.PongAPI.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Score> Scores { get; set; } // Adicionado o using para List<>
    }
}
