﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{

    public class DiscordEmbedModel
    {
        public string type { get; set; }
        public string title { get; set; }
        public Thumbnail thumbnail { get; set; }
        public Image image { get; set; }
        public Footer footer { get; set; }
        public List<Field> fields { get; set; }
        public string description { get; set; }
        public int color { get; set; }
        public Author author { get; set; }
    }



    public class Author
    {
        public string proxy_icon_url { get; set; }
        public string name { get; set; }
        public string icon_url { get; set; }
    }

    public class Field
    {
        public string value { get; set; }
        public string name { get; set; }
        public bool inline { get; set; }
    }

    public class Footer
    {
        public string text { get; set; }
        public string proxy_icon_url { get; set; }
        public string icon_url { get; set; }
    }

    public class Image
    {
        public int width { get; set; }
        public string url { get; set; }
        public string proxy_url { get; set; }
        public int height { get; set; }
    }

    public class Thumbnail
    {
        public int width { get; set; }
        public string url { get; set; }
        public string proxy_url { get; set; }
        public int height { get; set; }
    }

}
