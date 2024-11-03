namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedDefaultUsers3 : DbMigration
    {
        public override void Up()
        {
            // Exemplo de hash gerado (use a classe PasswordHasher para obter um valor real)
            var passwordHash = "AOVqemQtNzWqD+0RaPaRv9Ib/tREd52Zj+adATwQSkKAclgROtQ8xJdPPrg/0Nm5FA=="; // #Joao5000
            var securityStamp = Guid.NewGuid().ToString();
            var concurrencyStamp = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid(); // Novo ID do usuário

            Sql($@"
                INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, Admin)
                VALUES (
                    '{userId}', 
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
                    'defaultuser@example.com',
                    1
                );
            ");

            Sql($@"
                INSERT INTO Authors (FirstName, LastName, Nacionality, Image, Bibliography, Pseudonym, EmailContact, UserId)
                VALUES (
                    'Admin', 
                    'Admin', 
                    'Brasileiro', 
                    'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSg_H-BZgmjZYpmp3QDlppUbkyUX2OBbpG0Ug&s', 
                    'Sou admin', 
                    'admin', 
                    'defaultuser@example.com', 
                    '{userId}'
                );
            ");
        }

        public override void Down()
        {
            // Remova o autor padrão se necessário
            Sql("DELETE FROM Authors WHERE EmailContact = 'defaultuser@example'");

            // Remova o usuário padrão se necessário
            Sql("DELETE FROM AspNetUsers WHERE UserName = 'defaultuser@example.com'");
        }
    }
}
