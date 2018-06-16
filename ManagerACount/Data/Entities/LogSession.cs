using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerACount.Data.Entities
{
    public partial class LogSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogSessionId { get; set; }
        public string Token { get; set; }
        public string IP { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool State { get; set; }
        public System.DateTime ExpirationDate { get; set; }
    }
}
