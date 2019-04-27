﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    /// <summary>
    /// класс для сохранения дескрипторов объекта
    /// </summary>
    public class SaveDescriptionObjectEntry
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Text { get; set; }



        public SaveDescriptionObjectEntry()
        {
            //NewId = null;
        }
    }
}