namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedDefaultUsers : DbMigration
    {
        public override void Up()
        {
            // Exemplo de hash gerado (use a classe PasswordHasher para obter um valor real)
            var passwordHash = "AOVqemQtNzWqD+0RaPaRv9Ib/tREd52Zj+adATwQSkKAclgROtQ8xJdPPrg/0Nm5FA=="; // Use PasswordHasher aqui
            var securityStamp = Guid.NewGuid().ToString();
            var concurrencyStamp = Guid.NewGuid().ToString();

            Sql($@"
                INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName)
                VALUES (
                    '{Guid.NewGuid()}', 
                    'defaultuser@example.com', 
                    0,  -- EmailConfirmed
                    '{passwordHash}', 
                    '{securityStamp}', 
                    NULL,  -- PhoneNumber
                    0,  -- PhoneNumberConfirmed
                    0,  -- TwoFactorEnabled
                    NULL,  -- LockoutEndDateUtc
                    1,  -- LockoutEnabled
                    0,  -- AccessFailedCount
                    'defaultuser@example.com'
                );
            ");
        }

        public override void Down()
        {
            // Remova o usuário padrão se necessário
            Sql("DELETE FROM AspNetUsers WHERE UserName = 'defaultuser@example.com'");
        }
    }
}
