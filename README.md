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

### Adicionando o usuario padrão

- Crie uma nova migration com o comando `add-migration defaultuser`
- No arquivo de migração criado, subtitua os métodos `Up` e `Down` pelo seguinte código:

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

- Execute o comando `Update-Database` para adicionar o usuário padrão ao banco de dados

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
