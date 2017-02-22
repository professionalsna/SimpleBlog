using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Bytecode.CodeDom;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace SimpleBlog.Models
{
    public class Role
    {
        public virtual int Id { get; set; }
        public virtual string  Name { get; set; }

        public virtual IList<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }

    }

    public class RoleMap:ClassMapping<Role>
    {
        public RoleMap()
        {
            Table("roles");
            Id(x => x.Id, x => x.Generator(Generators.Identity));
            Property(x => x.Name, x => x.NotNullable(true));

            Bag(x => x.Users, x =>
            {
                x.Table("role_users");
                x.Key(k => k.Column("role_id"));

            }, x => x.ManyToMany(k => k.Column("user_id")));

        }

        
    }
}