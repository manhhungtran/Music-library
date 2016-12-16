﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTO;
using BL.Filters;
using BL.Utilities.SortEnum;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace PL.Models
{
    public class PlaylistListModel
    {
        public IEnumerable<PlaylistDTO> Playlists { get; set; }
    }
}