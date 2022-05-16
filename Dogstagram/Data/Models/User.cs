namespace Dogstagram.Server.Data.Models
{
    using Dogstagram.Server.Data.Models.Base;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;

    public class User : IdentityUser, IEntity
    {
        public Profile Profile { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public IEnumerable<Dog> Dogs { get; } = new HashSet<Dog>();
    }
}
