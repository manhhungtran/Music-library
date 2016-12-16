using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTO;

namespace PL.Models
{
    public class AddSongToPlaylistModel
    {
        public int IdSong { get; set; }

        public int Playlist { get; set; }
    }
}