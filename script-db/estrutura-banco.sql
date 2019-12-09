CREATE DATABASE communityInLounge;

USE communityInLounge;

CREATE TABLE Tipo_usuario(
 Tipo_usuario_id INT IDENTITY PRIMARY KEY NOT NULL,
 Titulo VARCHAR(30) UNIQUE NOT NULL,
 Deletado_em DATETIME
 );

 select*from Tipo_usuario;

 CREATE TABLE Usuario(
 Usuario_id INT IDENTITY PRIMARY KEY NOT NULL,
 Nome VARCHAR(100) NOT NULL,
 Email VARCHAR(100) UNIQUE NOT NULL,
 Senha VARCHAR(255) NOT NULL, 
 Telefone VARCHAR(20) NOT NULL,
 Foto VARCHAR(255) NOT NULL,
 Genero VARCHAR(30) NOT NULL,
 Deletado_em DATETIME,
 Tipo_usuario_id INT FOREIGN KEY REFERENCES Tipo_usuario(Tipo_usuario_id) NOT NULL
 );

  select*from Usuario;


 CREATE TABLE Comunidade(
 Comunidade_id INT IDENTITY PRIMARY KEY NOT NULL,
 Nome VARCHAR(100) NOT NULL, 
 Descricao VARCHAR(255) NOT NULL,
 Email_contato VARCHAR(100) NOT NULL,
 Telefone_contato VARCHAR(20) NOT NULL,
 Foto VARCHAR(255) NOT NULL,
 Deletado_em DATETIME,
 Responsavel_usuario_id INT FOREIGN KEY REFERENCES Usuario(Usuario_id) NOT NULL
 );

 CREATE TABLE Categoria(
  Categoria_id INT IDENTITY PRIMARY KEY NOT NULL, 
  Nome VARCHAR(100) NOT NULL,
  Deletado_em DATETIME
  );

  CREATE TABLE Sala(
  Sala_id INT IDENTITY PRIMARY KEY NOT NULL,
  Nome VARCHAR(100) NOT NULL,
  Descricao VARCHAR(255) NOT NULL,
  Qntd_pessoas VARCHAR(20) NOT NULL,
  Deletado_em DATETIME
  );

 CREATE TABLE Evento_tw(
  Evento_id INT IDENTITY PRIMARY KEY NOT NULL,
  Nome VARCHAR(100) NOT NULL,
  Evento_data DATE NOT NULL,
  Horario TIME NOT NULL,
  Descricao VARCHAR(255) NOT NULL, 
  Email_contato VARCHAR(255) NOT NULL,
  Publico VARCHAR(3) NOT NULL, 
  Diversidade VARCHAR(3) NOT NULL,
  Coffe VARCHAR(3) NOT NULL,
  Foto VARCHAR(255),
  Url_evento VARCHAR (255) NOT NULL,
  Deletado_em DATETIME,
  Categoria_id INT FOREIGN KEY REFERENCES Categoria(Categoria_id) NOT NULL,
  Sala_id INT FOREIGN KEY REFERENCES Sala(Sala_id) NOT NULL,
  );

  CREATE TABLE Evento(
  Evento_id INT IDENTITY PRIMARY KEY NOT NULL,
  Nome VARCHAR(100) NOT NULL,
  Evento_data DATE NOT NULL,
  Horario TIME NOT NULL,
  Descricao VARCHAR(255) NOT NULL, 
  Email_contato VARCHAR(255) NOT NULL,
  Status_evento VARCHAR(50) NOT NULL, 
  Diversidade VARCHAR(3) NOT NULL,
  Coffe VARCHAR(3) NOT NULL,
  Foto VARCHAR(255),
  Url_evento VARCHAR (255),
  Deletado_em DATETIME,
  Categoria_id INT FOREIGN KEY REFERENCES Categoria(Categoria_id) NOT NULL,
  Sala_id INT FOREIGN KEY REFERENCES Sala(Sala_id) NOT NULL,
  Comunidade_id INT FOREIGN KEY REFERENCES Comunidade(Comunidade_id) NOT NULL
  );

  CREATE TABLE Responsavel_evento_tw(
  Responsavel_evento_tw_id INT IDENTITY PRIMARY KEY NOT NULL,
  Evento INT FOREIGN KEY REFERENCES Evento(Evento_id) NOT NULL,
  Responsavel_evento INT FOREIGN KEY REFERENCES Usuario(Usuario_id) NOT NULL
  );
