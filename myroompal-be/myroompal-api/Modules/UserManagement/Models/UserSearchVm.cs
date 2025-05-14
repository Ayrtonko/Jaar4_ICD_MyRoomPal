using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using myroompal_api.Entities.Types;

namespace myroompal_api.Modules.UserManagement.Models
{
    public class UserSearchVm
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserAccountStatusType? Status { get; set; }
    }
}