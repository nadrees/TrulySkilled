using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrulySkilled.Web.ViewModels.Game
{
    public class PlayGameViewModel
    {
        public Guid GameId { get; set; }
        public int PlayerId { get; set; }
    }
}