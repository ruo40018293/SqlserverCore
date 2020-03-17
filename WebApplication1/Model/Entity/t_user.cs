using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Model.EntityCommon;

namespace Model.Entity
{
    [Table("t_user")]
    public class t_user:EntityBase
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; }

        public int Age { get; set; }

        public int Sex { get; set; }
    }
}
