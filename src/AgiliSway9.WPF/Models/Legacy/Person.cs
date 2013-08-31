using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string FabricUri { get; set; }

        public string BirthdayString
        {
            get
            {
                if (Birthday != null)
                    return ((DateTime)Birthday).ToShortDateString();
                else
                    return "";
            }
            set { }
        }
    }
}
