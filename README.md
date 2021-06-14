# Descrição do Teste e Scripts do Docker:
Projeto foi desenvolvido em .Net 5.0, utilizei o banco de dados MySql com docker e os teste unitários foram desenvolvidos com XUnit.

Para executar o projeto primeiro faça o container do Mysql e rode o comando "update-database" para rodar a migration e criar o banco no server do Mysql, após isso a api já está pronta para uso.

Foi usado Identity e JWT para os usuários.

Primeiro use a rota de CadastradoUsuario (está tudo documentado com Swagger) após o cadastro faça o login e ele irá retorna um JWT que deve ser usado como um token de Authorization com o sufixo Bearer no começo, esse token deve ser usado em todas as requisições de transação.

# Script do Docker
docker run --detach --name=test-mysql -p 52000:3306  --env="MYSQL_ROOT_PASSWORD=teste123" mysql:5.7.13
