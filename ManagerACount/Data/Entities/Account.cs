using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerACount.Data.Entities
{
    public partial class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public String UserEmail { get; set; }
        public String UserPassword { get; set; }
        public System.DateTime? CreationDate { get; set; }
        public int UserId { get; set; }
        public int StateId { get; set; }
        public int RoleId { get; set; }
    }
}
