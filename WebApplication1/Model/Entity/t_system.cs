using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Model.EntityCommon;

namespace Model.Entity
{
    [Table("t_system")]
    public class t_system:EntityBase
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string SystemName { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; }
    }
}
