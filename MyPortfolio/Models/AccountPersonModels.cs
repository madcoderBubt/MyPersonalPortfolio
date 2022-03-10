using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolio.Models
{
    public class AccountPersonModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public int NickName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string PermanentAddress { get; set; }
        public string CurrentAddress { get; set; }
        public int NationalID { get; set; }
        public string Nationality { get; set; }

        public string UserID { get; set; }
        //public ApplicationUser User { get; set; }
    }
}