namespace receitasall.Migrations
{
    using Antlr.Runtime.Misc;
    using Microsoft.SqlServer.Server;
    using receitasall.Models;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationseed : DbMigration
    {
        public override void Up()
        {
            // Usuario 1
            // Exemplo de hash gerado (use a classe PasswordHasher para obter um valor real)
            var passwordHash1 = "ALD5M4T2m1d4tLKSEPisKZhr5O6G0qEm8KjrlIRxrtUkAQ9gqGbqSjOt3ZPp9EGVfw=="; // #Maria5000
            var securityStamp1 = Guid.NewGuid().ToString();
            var userId1 = Guid.NewGuid(); // Novo ID do usuárioS

            Sql($@"
                INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, Admin)
                VALUES (
                    '{userId1}', 
                    'maria@gmail.com', 
                    0,  -- EmailConfirmed
                    '{passwordHash1}', 
                    '{securityStamp1}', 
                    NULL,  -- PhoneNumber
                    0,  -- PhoneNumberConfirmed
                    0,  -- TwoFactorEnabled
                    NULL,  -- LockoutEndDateUtc
                    1,  -- LockoutEnabled
                    0,  -- AccessFailedCount
                    'maria@gmail.com',
                    0
                );
            ");

            Sql($@"
                INSERT INTO Authors (FirstName, LastName, Nacionality, Image, Bibliography, Pseudonym, EmailContact, UserId)
                VALUES (
                    'Maria', 
                    'da Silva', 
                    'Brasileira', 
                    'https://img.freepik.com/fotos-premium/cozinheira-muito-feliz-mulher-com-fundo-branco_1042814-56143.jpg', 
                    'Uma mulher que adora cozinhar e compartilhar suas receitas',
                    'mary', 
                    'maria@gmail.com', 
                    '{userId1}'
                );
            ");

            // Usuario 2
            // Exemplo de hash gerado (use a classe PasswordHasher para obter um valor real)
            var passwordHash2 = "AJiS0cXV15BVasdIhLKZtSdUIHN3HOoqUcK3UypP0R+nWeTrHXJXZ8q7Q7yEsMerpA=="; // #Jose5000
            var securityStamp2 = Guid.NewGuid().ToString();
            var userId2 = Guid.NewGuid(); // Novo ID do usuárioS

            Sql($@"
                INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, Admin)
                VALUES (
                    '{userId2}', 
                    'jose@gmail.com', 
                    0,  -- EmailConfirmed
                    '{passwordHash2}', 
                    '{securityStamp2}', 
                    NULL,  -- PhoneNumber
                    0,  -- PhoneNumberConfirmed
                    0,  -- TwoFactorEnabled
                    NULL,  -- LockoutEndDateUtc
                    1,  -- LockoutEnabled
                    0,  -- AccessFailedCount
                    'jose@gmail.com',
                    0
                );
            ");

            Sql($@"
                INSERT INTO Authors (FirstName, LastName, Nacionality, Image, Bibliography, Pseudonym, EmailContact, UserId)
                VALUES (
                    'Jose', 
                    'Aguiar', 
                    'Brasileiro', 
                    'https://img.freepik.com/fotos-premium/um-homem-cozinhando-comida-em-uma-frigideira-em-uma-cozinha_1072138-193393.jpg', 
                    'Um homem que descobriu o prazer de cozinhar e compartilhar suas receitas',
                    'jota', 
                    'jose@gmail.com', 
                    '{userId2}'
                );
            ");

            // Receita 1 - Bolo de Chocolate
            Sql($@"
                INSERT INTO Recipes (Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
                VALUES (
                    'Bolo de Chocolate', 
                    'Como não pensar em um bolo de chocolate com morango e não salivar, certo? O bolo sensação é o doce que conquista o paladar de muita gente, já que o morango quebra o gosto adocicado do chocolate.', 
                    'https://www.estadao.com.br/resizer/v2/FIVYQFU6J5ND3PYRA6XQHR4NW4.jpg?quality=80&auth=04a93b8f4c288302da64fd8a96da7bb7cc11dff70430e4ba66587218d5b6011f&width=720&height=503&focal=0,0', 
                    1,  -- Dificuldade: Fácil
                    0,  -- Público
                    45,  -- Tempo de preparação
                    '8 porções', 
                    (SELECT Id FROM Authors WHERE UserId = '{userId1}'), 
                    '2024-10-03 08:15:33', 
                    '2024-10-04 09:12:33', 
                    '#8B4513'
                );
            ");

            // Receita 2 - Bolo de Cenoura
            Sql($@"
                INSERT INTO Recipes (Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
                VALUES (
                    'Bolo de cenoura com cobertura de chocolate', 
                    'Se um bolo de cenoura com massa fofinha e cobertura de chocolate craquelada é o seu sonho, ele está prestes a ser realizado. Este bolo de cenoura com cobertura de chocolate é hit do Panelinha. Experimente!', 
                    'https://static.itdg.com.br/images/1200-675/339036ef8ed4fad2ba6dc7290696c5de/254790-shutterstock-1709330689-1-1-1.jpg', 
                    2,  -- Dificuldade: Média
                    0,  -- Público
                    60,  -- Tempo de preparação
                    '10 porções', 
                    (SELECT Id FROM Authors WHERE UserId = '{userId2}'), 
                    '2024-10-01 03:22:33', 
                    '2024-11-03 08:15:33', 
                    'rgb(255, 202, 42)'
                );
            ");

            // Receita 3 - Bolo de Laranja
            Sql($@"
                INSERT INTO Recipes (Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
                VALUES (
                    'Bolo de Laranja', 
                    'Bolo cítrico e aromático de laranja.', 
                    'https://i.panelinha.com.br/i1/64-bk-9645-bolo-laranja.webp', 
                    1,  -- Dificuldade: Fácil
                    1,  -- Privado
                    50,  -- Tempo de preparação
                    '8 porções', 
                    (SELECT Id FROM Authors WHERE UserId = '{userId1}'), 
                    '2024-11-01 08:15:33', 
                    '2024-11-01 10:10:33', 
                    'rgb(187, 89, 63)'
                );
            ");

            // Adicionando ingredientes para a receita com RecipeId = 1
            Sql(@" 
                INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
                (1, '2 xícaras de farinha de trigo', 1),
                (2, '2 xícaras de açúcar', 1),
                (3, '1 xícara de leite', 1),
                (4, '6 colheres(sopa) de chocolate em pó', 1),
                (5, '1 colher(sopa) de fermento em pó', 1),
                (6, '6 ovos', 1),
                (7, 'Cobertura', 1),
                (8, '2 colheres(sopa) de manteiga', 1),
                (9, '2 xícaras de leite', 1),
                (10, '1 xícara de chocolate em pó', 1);
            ");


            // adicionar Steps para a receita com RecipeId = 1
            Sql(@" 
                INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
                (1, 'Em uma batedeira, bata as claras em neve.', 1),
                (2, 'Acrescente as gemas, o açúcar e bata novamente.', 1),
                (3, 'Adicione a farinha, o chocolate em pó, o fermento, o leite e bata por mais alguns minutos.', 1),
                (4, 'Despeje a massa em uma forma untada e leve para assar em forno médio(180° C), preaquecido, por 40 minutos.', 1),
                (5, 'Cobertura: Em uma panela, leve a fogo médio o chocolate em pó, a manteiga e o leite, deixe até ferver.', 1),
                (6, 'Despeje quente sobre o bolo já assado.', 1);
            ");


            // Adicionando ingredientes para a receita com RecipeId = 2
            Sql(@"
                INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
                (1, '3 cenouras médias(cerca de 360g ou 2 ¼ xícaras(chá) de cenoura descascada e ralada)', 2),
                (2, '4 ovos em temperatura ambiente', 2),
                (3, '1 xícara(chá) de óleo', 2),
                (4, '1½ xícara(chá) de açúcar', 2),
                (5, '2 xícaras(chá) de farinha de trigo', 2),
                (6, '1 colher(sopa) de fermento em pó', 2),
                (7, '1 pitada de sal', 2),
                (8, 'manteiga e farinha de trigo para untar e polvilhar a fôrma', 2);
            ");

            // adicionar Steps para a receita com RecipeId = 2
            Sql(@"
                INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
                (1, 'Preaqueça o forno a 180 ºC(temperatura média).Unte com manteiga uma fôrma retangular de 30 cm x 20 cm e 5 cm de altura.Polvilhe farinha de trigo, chacoalhe para cobrir todo o fundo e as laterais e bata na pia para tirar o excesso.', 2),
                (2, 'Numa tigela, coloque a farinha, o sal e o fermento, passando pela peneira.Misture e reserve.', 2),
                (3, 'Lave e descasque as cenouras.Descarte a ponta da rama. Corte cada uma em rodelas e transfira para o liquidificador — a cenoura cortada em rodelas é triturada mais facilmente.', 2),
                (4, 'Junte o óleo às cenouras cortadas.Numa tigela pequena, quebre um ovo de cada vez e transfira para o liquidificador — se algum estiver estragado, você não perde a receita. Acrescente o açúcar e bata bem até ficar liso, por cerca de 5 minutos.', 2),
                (5, 'Transfira a mistura líquida para uma tigela grande e adicione os secos em 3 etapas, passando pela peneira.Misture delicadamente com um batedor de arame a cada adição para incorporar.', 2),
                (6, 'Transfira a massa para a fôrma e leve ao forno para assar por cerca de 45 minutos.Para saber se o bolo está pronto, espete um palito na massa: se sair limpo, pode tirar do forno; caso contrário, deixe assar por mais alguns minutos.', 2),
                (7, 'Retire o bolo do forno e deixe esfriar por 15 minutos antes de preparar a cobertura — o bolo deve estar morno na hora de colocar a cobertura de chocolate.', 2);
            ");

            // Adicionando ingredientes para a receita com RecipeId = 3

            Sql(@"
                INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
                (1, '2 laranjas pêra', 3),
                (2, '2 ovos', 3),
                (3, '1 xícara(chá) de óleo', 3),
                (4, '1½ xícara(chá) de açúcar', 3),
                (5, '2 xícaras(chá) de farinha de trigo', 3),
                (6, '2 colheres(chá) de fermento em pó', 3),
                (7, 'manteiga e farinha de trigo para untar e polvilhar a fôrma', 3);
            ");

            Sql(@"
                 INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
                (1, 'Preaqueça o forno a 180 °C(temperatura média).Com um pedaço de papel toalha, unte com manteiga uma fôrma com furo no meio de 24 cm de diâmetro – espalhe uma camada bem fina e uniforme.Polvilhe com farinha e chacoalhe bem para espalhar. Bata sobre a pia para retirar o excesso.', 3),
                (2, 'Descasque uma das laranjas, eliminando toda a parte branca.Lave bem a outra sob água corrente e corte e descarte as pontas, mantendo a casca(se preferir um sabor menos amargo, descasque e elimine a parte branca das duas laranjas).Corte as laranjas em quatro gomos.Descarte o miolo branco e os caroços, corte cada gomo em três pedaços e transfira para o liquidificador.', 3),
                (3, 'Numa tigela pequena quebre um ovo de cada vez e transfira para o liquidificador – se um estiver estragado você não perde a receita.Junte o óleo, o açúcar e bata bem até ficar liso.', 3),
                (4, 'Numa tigela média, misture a farinha com o fermento em pó.', 3),
                (5, 'Transfira a laranja batida com os líquidos para uma tigela grande e acrescente a farinha com o fermento aos poucos, passando pela peneira.Misture delicadamente com o batedor de arame a cada adição para incorporar.', 3),
                (6, 'Transfira a massa do bolo para a fôrma e leve ao forno para assar por cerca de 50 minutos.Para verificar se o bolo está pronto: espete um palito na massa, se sair limpo pode retirar do forno; caso contrário, deixe por mais alguns minutos, até que asse completamente.', 3),
                (7, 'Retire o bolo do for no e deixe esfriar por 30 minutos antes de desenformar – cuidado, o bolo pode rachar se estiver quente ao ser desenformado. Cubra a fôrma com um prato e vire de uma só vez para desenformar. Atenção: o bolo deve estar completamente frio antes de cobrir com o glacê.', 3);
            ");

        }

        public override void Down()
        {
            // Remova o autor padrão se necessário
            Sql("DELETE FROM Authors WHERE EmailContact = 'maria@gmail.com'");

            // Remova o usuário padrão se necessário
            Sql("DELETE FROM AspNetUsers WHERE UserName = 'maria@gmail.com'");

            // Remova os ingredientes inseridos na migração
            Sql("DELETE FROM Ingredients WHERE RecipeId = 1");

            // Remova as receitas inseridas na migração
            Sql("DELETE FROM Recipes WHERE Title = 'Bolo de Chocolate'");
            Sql("DELETE FROM Recipes WHERE Title = 'Bolo de cenoura com cobertura de chocolate'");
            Sql("DELETE FROM Recipes WHERE Title = 'Bolo de Laranja'");

            // Remova os ingredientes inseridos na migração
            Sql("DELETE FROM Ingredients WHERE RecipeId = 1");
            Sql("DELETE FROM Ingredients WHERE RecipeId = 2");
            Sql("DELETE FROM Ingredients WHERE RecipeId = 3");

            // Remova os passos inseridos na migração
            Sql("DELETE FROM Steps WHERE RecipeId = 1");
            Sql("DELETE FROM Steps WHERE RecipeId = 2");
            Sql("DELETE FROM Steps WHERE RecipeId = 3");


        }
    }
}
