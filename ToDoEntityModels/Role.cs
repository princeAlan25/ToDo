using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToDoEntityModels
{
    public class Role
    {
        [Required]
        [Key]
        public int RoleId { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = "guest";
        public Guid UserId { get; set; }
    }
}
