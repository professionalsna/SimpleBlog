using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SimpleBlog.Migrations
{
[Migration(3)]
    public class _003_AddContentToPost:Migration
    {
       

        public override void Up()
        {
            Create.Column("content").OnTable("posts").AsString(5000).Nullable();
        }

        public override void Down()
        {
            Delete.Column("content").FromTable("posts");
        }
    }
}