using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myroompal_api.Modules.UserManagement.Models
{
    public class UserVm
    {
        public string Auth0Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }   
        public string PhoneNumber { get; set; }

        public String Country { get; set; }
        public String City { get; set; }
        public String StreetName { get; set; }
        public String PostalCode { get; set; }
    }
}