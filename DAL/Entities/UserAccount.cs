using System;
using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    /// <summary>
    /// Base class for application user, 
    /// all declared properties MUST be virtual
    /// 
    /// Please note this class needs to be referenced even from PL,
    /// (AuthenticationManager), therefore its placed apart from 
    /// other entities in DAL.
    /// </summary>
    public class UserAccount : RelationalUserAccount, IEntity<Guid>
    {
        [MaxLength(64)]
        public virtual string FirstName { get; set; }

        [MaxLength(64)]
        public virtual string LastName { get; set; }

        [MaxLength(1024)]
        public virtual string Address { get; set; }

        public virtual DateTime BirthDate { get; set; } = new DateTime(1980, 1, 1);
    }
}
