namespace GameStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PegiRating = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        ImagePath = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                        GenreId = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Genre", t => t.GenreId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Age = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.ApplicationUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RentalGame",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RentedDate = c.DateTime(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        GameId = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Game", t => t.GameId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.ApplicationUserInRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserInRole", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ApplicationUserInRole", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.RentalGame", "GameId", "dbo.Game");
            DropForeignKey("dbo.RentalGame", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogin", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Game", "GenreId", "dbo.Genre");
            DropForeignKey("dbo.Genre", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Game", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaim", "UserId", "dbo.ApplicationUsers");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.ApplicationUserInRole", new[] { "RoleId" });
            DropIndex("dbo.ApplicationUserInRole", new[] { "UserId" });
            DropIndex("dbo.RentalGame", new[] { "GameId" });
            DropIndex("dbo.RentalGame", new[] { "ApplicationUserId" });
            DropIndex("dbo.ApplicationUserLogin", new[] { "UserId" });
            DropIndex("dbo.Genre", new[] { "ApplicationUserId" });
            DropIndex("dbo.ApplicationUserClaim", new[] { "UserId" });
            DropIndex("dbo.ApplicationUsers", "UserNameIndex");
            DropIndex("dbo.Game", new[] { "GenreId" });
            DropIndex("dbo.Game", new[] { "ApplicationUserId" });
            DropTable("dbo.Roles");
            DropTable("dbo.ApplicationUserInRole");
            DropTable("dbo.RentalGame");
            DropTable("dbo.ApplicationUserLogin");
            DropTable("dbo.Genre");
            DropTable("dbo.ApplicationUserClaim");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.Game");
        }
    }
}
