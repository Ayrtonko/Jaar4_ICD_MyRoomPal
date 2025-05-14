using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using myroompal_api.Entities.Types;

namespace myroompal_api.Modules.UserManagement.Models
{
    public class UserRoleDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRoleType RoleName { get; set; }
    }
}