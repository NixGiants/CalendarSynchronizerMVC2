using Microsoft.AspNetCore.Authorization;

namespace CalendarSynchronizerWeb.Authorization
{
    public class UserNameRequirement : IAuthorizationRequirement
    {
        public UserNameRequirement(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
