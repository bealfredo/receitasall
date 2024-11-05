# Tópicos em Programação III - Trabalho

**Aluno**: Alfredo de Souza Aguiar Neto

## Descrição da Aplicação: ReceitasAll

<!-- ASP.NET MVC 5 com logon OAuth2 no Facebook e Google -->

A aplicação consiste em um sistema que simula um blog de receitas. O sistema permite que o usuário crie uma conta, faça login, crie, edite e exclua suas receitas. Além disso, o usuário pode visualizar as receitas públicas de outros usuários.

Também é possível criar, editar e excluir livro de receitas, que são uma coleção de receitas do usuário, permitindo que um autor crie várias coleções de receitas, organizando suas criações em diferentes livros. O usuário pode adicionar e remover receitas de um livro de receitas, além de visualizar as receitas de um livro de receitas seu ou de outro usuário.

A aplicação foi desenvolvida utilizando ASP.NET com o framework MVC 5 e Entity Framework. O sistema utiliza autenticação externa com Facebook e Google, além de autenticação com e-mail e senha padrão. A aplicação de dados foi gerada pelo ORM Entity Framework utilizando code-first.

## Passos para Execução

1. Clone o repositório para sua máquina local: [ReceitasAll](https://github.com/bealfredo/receitasall)
2. Abra o projeto no Visual Studio
3. Abra o arquivo `Web.config` e altere a string de conexão para o seu banco de dados local
4. Abra o Package Manager Console e execute o comando `Install-Package EntityFramework` para instalar o Entity Framework
5. Execute o comando `add-migration inicial` para criar a migração inicial
6. No arquivo de migração criado, na linha 78, substitua `.ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)` por `.ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: false)` 
7. Abra o Package Manager Console e execute o comando `Update-Database` para criar o banco de dados
8. Execute o projeto

### Adicionando o usuario administrador

- Crie uma nova migration com o comando `add-migration defaultuser`
- No arquivo de migração criado, subtitua os métodos `Up` e `Down` pelo seguinte código:

<details>

<summary>Código da Migration</summary>

```csharp
public override void Up()
        {
            // Exemplo de hash gerado (use a classe PasswordHasher para obter um valor real)
            var passwordHash = "AOVqemQtNzWqD+0RaPaRv9Ib/tREd52Zj+adATwQSkKAclgROtQ8xJdPPrg/0Nm5FA=="; // #Joao5000
            var securityStamp = Guid.NewGuid().ToString();
            var concurrencyStamp = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid(); // Novo ID do usuárioS

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
```

</details>

<br/>

- Execute o comando `Update-Database` para adicionar o usuário administrador ao banco de dados
  - **Usuário**: defaultuser@example.com
  - **Senha**: #Joao5000

### Popular o banco de dados (opcional)

- Crie uma nova migration com o comando `add-migration seed`
- No arquivo de migração criado, subtitua os métodos `Up` pelo seguinte código:

<details>

<summary>Código da Migration</summary>

```csharp
public override void Up()
{
    var baseId = 100;

    Sql($@"DELETE FROM Cookbooks WHERE ID = {baseId + 6}");
    Sql($@"DELETE FROM Cookbooks WHERE ID = {baseId + 7}");

    // Remova as receitas inseridas na migração
    Sql($@"DELETE FROM Recipes WHERE ID = {baseId + 1}");
    Sql($@"DELETE FROM Recipes WHERE ID = {baseId + 2}");
    Sql($@"DELETE FROM Recipes WHERE ID = {baseId + 3}");
    Sql($@"DELETE FROM Recipes WHERE ID = {baseId + 4}");
    Sql($@"DELETE FROM Recipes WHERE ID = {baseId + 5}");


    // Remova os ingredientes inseridos na migração
    Sql($@"DELETE FROM Ingredients WHERE RecipeId = {baseId + 1}");
    Sql($@"DELETE FROM Ingredients WHERE RecipeId = {baseId + 2}");
    Sql($@"DELETE FROM Ingredients WHERE RecipeId = {baseId + 3}");
    Sql($@"DELETE FROM Ingredients WHERE RecipeId = {baseId + 4}");
    Sql($@"DELETE FROM Ingredients WHERE RecipeId = {baseId + 5}");

    // Remova os passos inseridos na migração
    Sql($@"DELETE FROM Steps WHERE RecipeId = {baseId + 1}");
    Sql($@"DELETE FROM Steps WHERE RecipeId = {baseId + 2}");
    Sql($@"DELETE FROM Steps WHERE RecipeId = {baseId + 3}");
    Sql($@"DELETE FROM Steps WHERE RecipeId = {baseId + 4}");
    Sql($@"DELETE FROM Steps WHERE RecipeId = {baseId + 5}");

    // Remova o autor padrão se necessário
    Sql("DELETE FROM Authors WHERE EmailContact = 'maria3@gmail.com'");
    Sql("DELETE FROM Authors WHERE EmailContact = 'jose3@gmail.com'");

    // Remova o usuário padrão se necessário
    Sql("DELETE FROM AspNetUsers WHERE UserName = 'maria3@gmail.com'");
    Sql("DELETE FROM AspNetUsers WHERE UserName = 'jose3@gmail.com'");


    // Usuario 1
    // Exemplo de hash gerado (use a classe PasswordHasher para obter um valor real)
    var passwordHash1 = "ALD5M4T2m1d4tLKSEPisKZhr5O6G0qEm8KjrlIRxrtUkAQ9gqGbqSjOt3ZPp9EGVfw=="; // #Maria5000
    var securityStamp1 = Guid.NewGuid().ToString();
    var userId1 = Guid.NewGuid(); // Novo ID do usuárioS
    Sql($@"
        INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, Admin)
        VALUES (
            '{userId1}', 
            'maria3@gmail.com', 
            0,  -- EmailConfirmed
            '{passwordHash1}', 
            '{securityStamp1}', 
            NULL,  -- PhoneNumber
            0,  -- PhoneNumberConfirmed
            0,  -- TwoFactorEnabled
            NULL,  -- LockoutEndDateUtc
            1,  -- LockoutEnabled
            0,  -- AccessFailedCount
            'maria3@gmail.com',
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
            'maria3@gmail.com', 
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
            'jose3@gmail.com', 
            0,  -- EmailConfirmed
            '{passwordHash2}', 
            '{securityStamp2}', 
            NULL,  -- PhoneNumber
            0,  -- PhoneNumberConfirmed
            0,  -- TwoFactorEnabled
            NULL,  -- LockoutEndDateUtc
            1,  -- LockoutEnabled
            0,  -- AccessFailedCount
            'jose3@gmail.com',
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
            'jose3@gmail.com', 
            '{userId2}'
        );
    ");


    // Receita 1 - Bolo de Chocolate
    Sql($@"
        SET IDENTITY_INSERT Recipes ON;

        INSERT INTO Recipes (ID, Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
        VALUES (
            {baseId + 1},
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

        INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
        (1, '2 xícaras de farinha de trigo', {baseId + 1}),
        (2, '2 xícaras de açúcar', {baseId + 1}),
        (3, '1 xícara de leite', {baseId + 1}),
        (4, '6 colheres(sopa) de chocolate em pó', {baseId + 1}),
        (5, '1 colher(sopa) de fermento em pó', {baseId + 1}),
        (6, '6 ovos', {baseId + 1}),
        (7, 'Cobertura', {baseId + 1}),
        (8, '2 colheres(sopa) de manteiga', {baseId + 1}),
        (9, '2 xícaras de leite', {baseId + 1}),
        (10, '1 xícara de chocolate em pó', {baseId + 1});

        INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
        (1, 'Em uma batedeira, bata as claras em neve.', {baseId + 1}),
        (2, 'Acrescente as gemas, o açúcar e bata novamente.', {baseId + 1}),
        (3, 'Adicione a farinha, o chocolate em pó, o fermento, o leite e bata por mais alguns minutos.', {baseId + 1}),
        (4, 'Despeje a massa em uma forma untada e leve para assar em forno médio(180° C), preaquecido, por 40 minutos.', {baseId + 1}),
        (5, 'Cobertura: Em uma panela, leve a fogo médio o chocolate em pó, a manteiga e o leite, deixe até ferver.', {baseId + 1}),
        (6, 'Despeje quente sobre o bolo já assado.', {baseId + 1});

        SET IDENTITY_INSERT Recipes OFF;
    ");
    // Receita 2 - Bolo de Cenoura
    Sql($@"
        SET IDENTITY_INSERT Recipes ON;

        INSERT INTO Recipes (ID, Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
        VALUES (
            {baseId + 2},
            'Bolo de cenoura com cobertura de chocolate2222', 
            'Se um bolo de cenoura com massa fofinha e cobertura de chocolate craquelada é o seu sonho, ele está prestes a ser realizado. Este bolo de cenoura com cobertura de chocolate é hit do Panelinha. Experimente!', 
            'https://static.itdg.com.br/images/1200-675/339036ef8ed4fad2ba6dc7290696c5de/254790-shutterstock-1709330689-1-1-1.jpg', 
            2,  -- Dificuldade: Média
            0,  -- publico
            60,  -- Tempo de preparação
            '10 porções', 
            (SELECT Id FROM Authors WHERE UserId = '{userId1}'), 
            '2024-10-01 03:22:33', 
            '2024-11-03 08:15:33', 
            '#ffca2a'
        );

        INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
        (1, '3 cenouras médias(cerca de 360g ou 2 ¼ xícaras(chá) de cenoura descascada e ralada)', {baseId + 2}),
        (2, '4 ovos em temperatura ambiente', {baseId + 2}),
        (3, '1 xícara(chá) de óleo', {baseId + 2}),
        (4, '1½ xícara(chá) de açúcar', {baseId + 2}),
        (5, '2 xícaras(chá) de farinha de trigo', {baseId + 2}),
        (6, '1 colher(sopa) de fermento em pó', {baseId + 2}),
        (7, '1 pitada de sal', {baseId + 2}),
        (8, 'manteiga e farinha de trigo para untar e polvilhar a fôrma', {baseId + 2});

        INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
        (1, 'Preaqueça o forno a 180 ºC(temperatura média).Unte com manteiga uma fôrma retangular de 30 cm x 20 cm e 5 cm de altura.Polvilhe farinha de trigo, chacoalhe para cobrir todo o fundo e as laterais e bata na pia para tirar o excesso.', {baseId + 2}),
        (2, 'Numa tigela, coloque a farinha, o sal e o fermento, passando pela peneira.Misture e reserve.', {baseId + 2}),
        (3, 'Lave e descasque as cenouras.Descarte a ponta da rama. Corte cada uma em rodelas e transfira para o liquidificador — a cenoura cortada em rodelas é triturada mais facilmente.', {baseId + 2}),
        (4, 'Junte o óleo às cenouras cortadas.Numa tigela pequena, quebre um ovo de cada vez e transfira para o liquidificador — se algum estiver estragado, você não perde a receita. Acrescente o açúcar e bata bem até ficar liso, por cerca de 5 minutos.', {baseId + 2}),
        (5, 'Transfira a mistura líquida para uma tigela grande e adicione os secos em 3 etapas, passando pela peneira.Misture delicadamente com um batedor de arame a cada adição para incorporar.', {baseId + 2}),
        (6, 'Transfira a massa para a fôrma e leve ao forno para assar por cerca de 45 minutos.Para saber se o bolo está pronto, espete um palito na massa: se sair limpo, pode tirar do forno; caso contrário, deixe assar por mais alguns minutos.', {baseId + 2}),
        (7, 'Retire o bolo do forno e deixe esfriar por 15 minutos antes de preparar a cobertura — o bolo deve estar morno na hora de colocar a cobertura de chocolate.', {baseId + 2});

        SET IDENTITY_INSERT Recipes OFF;
    ");

    // Receita 3 - Bolo de Morango

    Sql($@"
        SET IDENTITY_INSERT Recipes ON;

        INSERT INTO Recipes (ID, Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
        VALUES (
            {baseId + 3},
            'Bolo de Morango', 
            'Bolo fofo e saboroso de morango, perfeito para sobremesas.', 
            '', 
            1,  -- Dificuldade: Fácil
            1,  -- privado
            60,  -- Tempo de preparação
            '10 porções', 
            (SELECT Id FROM Authors WHERE UserId = '{userId1}'), 
            '2024-11-01 08:15:33', 
            '2024-11-01 08:15:33', 
            'ff2a2a'
        );

        INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
        (1, '2 xícaras de morangos frescos picados', {baseId + 3}),
        (2, '2 ovos', {baseId + 3}),
        (3, '1 xícara(chá) de óleo', {baseId + 3}),
        (4, '1½ xícara(chá) de açúcar', {baseId + 3}),
        (5, '2 xícaras(chá) de farinha de trigo', {baseId + 3}),
        (6, '2 colheres(chá) de fermento em pó', {baseId + 3}),
        (7, 'manteiga e farinha de trigo para untar e polvilhar a fôrma', {baseId + 3});

        INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
        (1, 'Para iniciar a preparação do delicioso Bolo de Morango, comece preaquecendo o forno a 180 °C. Essa temperatura é ideal para garantir que o bolo asse de maneira uniforme, resultando em uma textura macia e saborosa. Enquanto isso, prepare uma fôrma de bolo, untando-a generosamente com manteiga e polvilhando farinha de trigo. Isso vai evitar que o bolo grude na forma, facilitando o desenforme posteriormente.', {baseId + 3}),
        (2, 'Lave os morangos sob água corrente, removendo qualquer sujeira. Em seguida, retire as folhas e pique-os em pedaços pequenos. Reserve alguns morangos inteiros ou cortados ao meio para usar na decoração do bolo após o assamento. Essa etapa é fundamental para garantir que o sabor dos morangos esteja presente em cada pedaço do bolo.', {baseId + 3}),
        (3, 'Em uma tigela grande, quebre os dois ovos e adicione o açúcar. Com um batedor ou uma batedeira, bata os ingredientes até que a mistura fique clara e fofa, o que pode levar alguns minutos. Esse processo incorpora ar à mistura, proporcionando leveza ao bolo.', {baseId + 3}),
        (4, 'Após obter a mistura clara, adicione o óleo. É importante misturar bem nesta etapa, garantindo que todos os ingredientes líquidos estejam bem incorporados. O óleo contribui para a umidade do bolo, fazendo com que ele fique macio e saboroso.', {baseId + 3}),
        (5, 'Agora, em outra tigela, misture a farinha de trigo com o fermento em pó. Adicione essa mistura aos ingredientes líquidos aos poucos, mexendo delicadamente com uma espátula ou batedor de arame. Esse movimento deve ser feito com cuidado, para não perder a aeração da massa.', {baseId + 3}),
        (6, 'Finalmente, é hora de adicionar os morangos picados à massa. Misture-os gentilmente, distribuindo-os uniformemente pela massa. Isso garantirá que cada pedaço do bolo tenha uma explosão de sabor de morango.', {baseId + 3}),
        (7, 'Com a massa pronta, despeje-a cuidadosamente na fôrma preparada. Leve ao forno pré-aquecido e asse por cerca de 60 minutos. O tempo de assamento pode variar, então é importante monitorar o bolo. Para saber se está pronto, espete um palito no centro. Se sair limpo, o bolo está perfeito; se não, deixe assar por mais alguns minutos.', {baseId + 3}),
        (8, 'Após o tempo de assamento, retire o bolo do forno e deixe esfriar na fôrma por alguns minutos. Depois, desenforme com cuidado, virando-o sobre um prato. Agora é o momento de caprichar na decoração: utilize os morangos reservados para enfeitar o topo do bolo, tornando-o ainda mais apetitoso e convidativo.', {baseId + 3})

        SET IDENTITY_INSERT Recipes OFF;
    ");

    // Receita 4 - Hambúrguer
    Sql($@"
        SET IDENTITY_INSERT Recipes ON;

        INSERT INTO Recipes (ID, Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
        VALUES (
            {baseId + 4},
            'Hambúrguer', 
            'Hambúrguer caseiro com pão, carne, queijo, alface, tomate e molho especial.', 
            'https://biteswithbri.com/wp-content/uploads/2021/02/HamburgerPattyRecipe04.jpg', 
            1,  -- Dificuldade: Fácil
            0,  -- Público
            30,  -- Tempo de preparação
            '1 porção', 
            (SELECT Id FROM Authors WHERE UserId = '{userId2}'), 
            '2024-11-03 08:15:33', 
            '2024-11-03 10:10:33', 
            '#ffca2a'
        );

        INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
        (1, '200g de carne moída', {baseId + 4}),
        (2, '1 pão de hambúrguer', {baseId + 4}),
        (3, '2 fatias de queijo', {baseId + 4}),
        (4, '2 folhas de alface', {baseId + 4}),
        (5, '2 rodelas de tomate', {baseId + 4}),
        (6, '1 colher(sopa) de maionese', {baseId + 4}),
        (7, '1 colher(sopa) de ketchup', {baseId + 4}),
        (8, '1 colher(sopa) de mostarda', {baseId + 4}),
        (9, 'sal e pimenta a gosto', {baseId + 4});

        INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
        (1, 'Tempere a carne moída com sal e pimenta a gosto e modele em formato de hambúrguer.', {baseId + 4}),
        (2, 'Em uma frigideira quente, grelhe o hambúrguer por 3 minutos de cada lado.', {baseId + 4}),
        (3, 'Monte o hambúrguer com o pão, a carne, o queijo, a alface, o tomate e os molhos.', {baseId + 4});

        SET IDENTITY_INSERT Recipes OFF;
    ");

    // Receita 5 - Pizza de Calabresa
    Sql($@"
        SET IDENTITY_INSERT Recipes ON;

        INSERT INTO Recipes (ID, Title, Description, Image, Difficulty, IsPrivate, PreparationTimeInMinutes, Rendimento, AuthorId, DateAdded, DateUpdated, AccentColor)
        VALUES (
            {baseId + 5},
            'Pizza Margherita', 
            'Uma pizza clássica italiana com molho de tomate, queijo mozzarella, manjericão fresco e azeite de oliva.', 
            'https://s2-receitas.glbimg.com/wb7DIMyCpEyV07sTAtcDWD8HQjw=/0x0:1200x675/984x0/smart/filters:strip_icc()/i.s3.glbimg.com/v1/AUTH_1f540e0b94d8437dbbc39d567a1dee68/internal_photos/bs/2024/h/r/EfCbvqTbeDRAD3Lzc5xA/pizza-margherita.jpg', 
            1,  -- Dificuldade: Fácil
            1,  -- Privado
            30,  -- Tempo de preparação
            '4 porções', 
            (SELECT Id FROM Authors WHERE UserId = '{userId2}'), 
            '2024-11-04 10:00:00', 
            '2024-11-04 10:15:00', 
            '#ad110c'
        );

        INSERT INTO Ingredients ([Order], [Value], [RecipeId]) VALUES
        (1, '1 xícara de molho de tomate', {baseId + 5}),
        (2, '200g de queijo mozzarella fatiado', {baseId + 5}),
        (3, 'Folhas frescas de manjericão', {baseId + 5}),
        (4, '1 massa de pizza pronta', {baseId + 5}),
        (5, '2 colheres de sopa de azeite de oliva', {baseId + 5}),
        (6, 'Sal e pimenta a gosto', {baseId + 5});

        INSERT INTO Steps ([Order], [Value], [RecipeId]) VALUES
        (1, 'Preaqueça o forno a 220°C.', {baseId + 5}),
        (2, 'Espalhe o molho de tomate sobre a massa de pizza.', {baseId + 5}),
        (3, 'Distribua o queijo mozzarella por cima do molho.', {baseId + 5}),
        (4, 'Asse a pizza por 10-15 minutos até o queijo derreter e a massa ficar crocante.', {baseId + 5}),
        (5, 'Retire do forno e adicione as folhas de manjericão fresco.', {baseId + 5}),
        (6, 'Finalize com um fio de azeite de oliva, sal e pimenta a gosto.', {baseId + 5});

        SET IDENTITY_INSERT Recipes OFF;
    ");

    // Cookbook 1 - Bolos
    Sql($@"
        SET IDENTITY_INSERT Cookbooks ON;

        INSERT INTO Cookbooks (ID, Title, Description, Image, IsPrivate, DateAdded, DateUpdated, AccentColor, AuthorId)
        VALUES (
            {baseId + 6},
            'Bolos Deliciosos: Receitas Simples', 
            'Bolos Deliciosos é um guia prático que traz uma seleção de receitas irresistíveis para quem ama a confeitaria. Este livro apresenta bolos fofinhos e saborosos, com um passo a passo claro e fácil de seguir, tornando a preparação acessível a todos. ', 
            'https://img.freepik.com/fotos-premium/padrao-sem-costura-com-bolo-de-morango-e-cereja-ilustracao-vetorial-desenhada-a-mao_508613-1065.jpg?semt=ais_hybrid', 
            0,  -- IsPrivate: publico
            '2024-11-04 10:00:00', 
            '2024-11-04 10:15:00', 
            '#503422',  -- AccentColor
            (SELECT Id FROM Authors WHERE UserId = '{userId1}')
        );

        INSERT INTO RecipeCookbooks([RecipeId], [CookbookId], [Order], [DateAdded])
        VALUES (
            {baseId + 1},
            {baseId + 6},
            1,
            '2024-11-04 10:10:00');

        INSERT INTO RecipeCookbooks([RecipeId], [CookbookId], [Order], [DateAdded])
        VALUES (
            {baseId + 2},
            {baseId + 6},
            1,
            '2024-11-04 10:12:00');

        INSERT INTO RecipeCookbooks([RecipeId], [CookbookId], [Order], [DateAdded])
        VALUES (
            {baseId + 3},
            {baseId + 6},
            1,
            '2024-11-04 10:14:00');

        SET IDENTITY_INSERT Cookbooks OFF;
    ");

    // Cookbook 2 - Lanches
    Sql($@"
        SET IDENTITY_INSERT Cookbooks ON;

        INSERT INTO Cookbooks (ID, Title, Description, Image, IsPrivate, DateAdded, DateUpdated, AccentColor, AuthorId)
        VALUES (
            {baseId + 7},
            'Lanches Gostosos',
            'Uma seleção de receitas de lanches não só deliciosas, mas também práticas e fáceis de fazer. Ideal para quem busca saborosas.',
            '', 
            1,  -- IsPrivate
            '2024-10-10 10:00:00', 
            '2024-10-10 10:00:00', 
            '#455a0f',  -- AccentColor
            (SELECT Id FROM Authors WHERE UserId = '{userId2}')
        );

        INSERT INTO RecipeCookbooks([RecipeId], [CookbookId], [Order], [DateAdded])
        VALUES (
            {baseId + 4},
            {baseId + 7},
            1,
            '2024-10-10 10:05:00');

        INSERT INTO RecipeCookbooks([RecipeId], [CookbookId], [Order], [DateAdded])
        VALUES (
            {baseId + 5},
            {baseId + 7},
            1,
            '2024-10-10 10:07:00');

        SET IDENTITY_INSERT Cookbooks OFF;
    ");
}

```

</details>

<br/>

- Execute o comando `Update-Database` para popular o banco de dados com dados de exemplo

- Os usuários criados são:
  - Usuário 1:
      - E-mail: `maria3@gmail.com`
      - Senha: `#Maria5000`

   - Usuário 2:
      - E-mail: `jose3@gmail.com`
      - Senha: `#Jose5000`



## Funcionalidades

- Cadastro de usuário com e-mail e senha ou autenticação externa com Facebook e Google
- Login de usuário
- Cadastro, edição, exclusão e visualização de receitas
- Adição e remoção de ingredientes e passos de preparo de uma receita
- Visualização de receitas públicas de outros usuários
- Cadastro, edição, exclusão e visualização de livros de receitas
- Adição e remoção de receitas de um livro de receitas
- Visualização de livros de receitas públicos de outros usuários
- Favoritar receitas públicas de outros usuários e visualização de receitas favoritas
- Visualização de receitas favoritas
- Filtragem de receitas por título, autor e dificuldade
- Filtragem de livros de receitas por título e autor

## Entidades

- **AspNetUsers**: Usuários do sistema
- **Author**: Entidade que representa a conta de autor de um usuário, cadastrada posteriormente ao cadastro do usuário
- **Recipe**: Receitas cadastradas pelos usuários
- **Ingredient**: Ingredientes de uma receita
- **Step**: Passos de preparo de uma receita
- **Cookbook**: Livros de receitas cadastrados pelos usuários
- **RecipeCookbook**: Relacionamento entre receitas e livros de receitas
- **FavoriteRecipe**: Receitas favoritas de um usuário

## Relacionamentos

- Um autor está associado a um usuário
- Uma receita está associada a um autor
- Um ingrediente está associado a uma receita
- Um passo de preparo está associado a uma receita
- Um livro de receitas está associado a um autor
- Uma receita pode estar associada a vários livros de receitas
- Uma receita pode ser favoritada por vários usuários


## Algumas validações

1. **Ativação da Conta de Autor**
   - Apenas usuários que ativaram a conta de autor podem adicionar e gerenciar receitas e livros de receitas.

2. **Acesso a Receitas Privadas**
   - Usuários não podem acessar os detalhes de uma receita de outro usuário se ela for privada.

3. **Acesso a Livros de Receitas Privados**
   - Usuários não podem acessar os detalhes de um livro de receitas de outro usuário se ele for privado.

4. **Favoritar Receitas**
   - Não é possível favoritar receitas privadas.

5. **Edição de Receitas**
   - Somente o dono da receita pode editar, adicionar e remover ingredientes e passos, além de deletar a receita.

6. **Edição de Livros de Receitas**
   - Somente o dono de um livro de receitas pode editar, adicionar e remover receitas desse livro, além de deletar o livro de receitas.

7. **Áreas Restritas para Contas de Autor**
   - É necessário ativar a conta de autor para acessar as seguintes áreas: Meu Perfil, Minhas Receitas, Favoritos. Assim como para adicionar e gerenciar receitas e livros de receitas.

8. **Visibilidade de Receitas em Livros de Outros Usuários**
   - Usuários só conseguem ver receitas públicas nos livros de receitas de outros usuários.

9. **Restrição de Exclusão de Receitas**
   - Não é permitido apagar uma receita se ela está sendo referenciada em outro lugar do sistema.

## Acesso das Páginas

### Públicas

- **Home**: Página inicial do sistema
- **Receitas**: Lista de receitas públicas
- **Detalhes da Receita**: Detalhes de uma receita pública
- **Livros de Receitas**: Lista de livros de receitas públicos
- **Detalhes do Livro de Receitas**: Detalhes de um livro de receitas público

### Privadas

- **Meu Perfil**: Perfil do usuário logado
- **Minhas Receitas**: Lista de receitas do usuário logado
- **Adicionar, Editar e Deletar Receitas**: Páginas de adição, edição e exclusão de receitas
- **Adicionar, Editar e Deletar Livros de Receitas**: Páginas de adição, edição e exclusão de livros de receitas
- **Favoritos**: Lista de receitas favoritas do usuário logado
- **Adicionar Receita a Livro de Receitas**: Página de adição de receitas a um livro de receitas
- **Adição de Ingrediente e Passo de Preparo**: Páginas de adição de ingredientes e passos de preparo a uma receita
- **Adicionar uma Receita a Favoritos**: Página de adição de uma receita a favoritos
- **Perfil de Autor**: Perfil de autor de um usuário
- **Ativação de Conta de Autor**: Página de ativação da conta de autor

## Outras Observações

- Para moderação, foi criado um usuário administrador que pode excluir qualquer receita ou livro de receitas, tendo acesso a todas as receitas e livros de receitas, independente de serem públicos ou privados.
  - **Usuário**: defaultuser@example.com
  - **Senha**: #Joao5000
