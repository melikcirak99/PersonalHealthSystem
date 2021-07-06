using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHS.Models
{
    public class ViewModel
    {
        public IEnumerable<tbl_Kisiler> Kisiler { get; set; }
        public IEnumerable<tbl_Randevular> Randevular { get; set; }
        public IEnumerable<tbl_admin> Admin { get; set; }
    }
}