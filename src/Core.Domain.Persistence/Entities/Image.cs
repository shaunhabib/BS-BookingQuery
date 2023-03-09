﻿using Core.Domain.Persistence.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Persistence.Entities
{
    public class Image : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string ImageUrl { get; set; }
    }
}
