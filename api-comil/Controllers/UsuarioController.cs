using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_comil.Models;
using api_comil.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class UsuarioController : ControllerBase
    {
        UsuarioRepositorio usuariorep = new UsuarioRepositorio();
        UploadRepositorio _uploadRepo = new UploadRepositorio();


        // /// <summary>
        // /// Método de busca de usuário através do Id
        // /// </summary>
        // /// <param name="id">Id do usuário</param>
        // /// <returns>Usuário correspondente ao Id</returns>
        // [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            Usuario userReturn = await usuariorep.Get(id);
            if (userReturn == null)
            {
                return NotFound();
            }
            return userReturn;
        }

        [HttpGet("Adm")]
        public async Task<ActionResult<List<Usuario>>> GetAdm(int id)
        {
            try
            {
                return await usuariorep.GetAdm();
            }
            catch (System.Exception)
            {

                throw;
            }

        }


        // /// <summary>
        // ///  Método de cadastro de usuário
        // /// </summary>
        // /// <param name="usuario">Objeto Usuário</param>
        // /// <returns>Usuario cadastrado</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario)
        {

            var valid = ValidaEnderecoEmail(usuario.Email);
            var exist = await usuariorep.ExistEmail(usuario.Email);


            if (exist != null)
            {
                return BadRequest("Esse email já está cadastrado. ");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (valid)
            {
                return await usuariorep.Post(usuario);
            }
            else
            {
                return BadRequest();
            }

        }
        // /// <summary>
        // /// Método para deletar um usuário
        // /// </summary>
        // /// <param name="id">Id do usuário</param>
        // /// <returns>Retorna usuário deletado</returns>
        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> Delete(int id)
        {
            var user = await usuariorep.Get(id);
            if (user == null) return NotFound();

            // verificar se o usuario tem evento pendente ou aprovado
            EventoRepositorio evento = new EventoRepositorio();
            var eventoComunidade = evento.GetEventsByUser(id).Result;

            if (eventoComunidade.Value == null)
            {
                user.DeletadoEm = DateTime.Now;
                await usuariorep.Delete(user);
                return user;
            }
            else
            {
                return StatusCode(403, "Exclua seus eventos primeiros antes de deletar sua conta");
            }
        }

        // /// <summary>
        // /// Método para atualização de dados do usuário
        // /// </summary>
        // /// <param name="usuario">Objeto Usuário</param>
        // /// <returns>Usuário alterado</returns>
        // [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> Put(int id, Usuario usuario)
        {

            var user = await usuariorep.Get(id);

            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (usuario.Nome != null) user.Nome = usuario.Nome;
            if (usuario.Telefone != null) user.Telefone = usuario.Telefone;
            if (usuario.Senha != null) user.Senha = usuario.Senha;
            if (usuario.Foto != null) user.Foto = usuario.Foto;
            if (usuario.Genero != null) user.Genero = usuario.Genero;

            return await usuariorep.Put(user);
        }







        [HttpPut("{id}/uploadFoto")]
        public async Task<ActionResult<Usuario>> Put(int id)
        {
            var usuario = await usuariorep.Get(id);

            if (usuario == null)
            {
                return NotFound("usuario não encontrado");
            }

            if (usuario.DeletadoEm != null)
            {
                return StatusCode(403, "Não é possivel fazer essa operação");
            }

            try
            {
                var arquivo = Request.Form.Files[0];
                var caminho = _uploadRepo.Upload(arquivo, "Imagens","Usuario");


                usuario.Foto = caminho;

                return await usuariorep.Put(usuario);

            }
            catch (Exception)
            {
                throw;
            }

        }








        //adm
        [HttpPut("ToAd")]
        public async Task<ActionResult<Usuario>> ChangeToAd(ResetSenha email)
        {
            var user = await usuariorep.ExistEmail(email.Email);
            // var user = await usuariorep.Get(id);

            if (user == null) return StatusCode(404, "Esse usuário não existe");

            TipoUsuarioRepositorio tipouser = new TipoUsuarioRepositorio();

            var tipo = await tipouser.Get(user.TipoUsuarioId);

            if (tipo.Titulo == "Comunidade") return StatusCode(403, "Essa operação não é possivel");

            if (tipo.Titulo == "Funcionario")
            {
                var put = await tipouser.Get("Administrador");
                user.TipoUsuarioId = put.TipoUsuarioId;

                return await usuariorep.Put(user);
            }
            else
            {
                return user;
            }
        }

        [HttpPut("{id}/ToFun")]
        public async Task<ActionResult<Usuario>> ChangeToFu(int id)
        {

            var user = await usuariorep.Get(id);

            if (user == null) return StatusCode(404, "Esse usuário não existe");

            TipoUsuarioRepositorio tipouser = new TipoUsuarioRepositorio();

            var tipo = await tipouser.Get(user.TipoUsuarioId);
            if (tipo.Titulo == "Comunidade") return StatusCode(403, "Essa operação não é possivel");

            if (tipo.Titulo == "Administrador")
            {
                var put = await tipouser.Get("Funcionario");
                user.TipoUsuarioId = put.TipoUsuarioId;

                return await usuariorep.Put(user);
            }
            else
            {
                return user;
            }

        }



        [HttpPut("ResetPassword")]
        public async Task<ActionResult<Usuario>> ResetPassword(ResetSenha email)
        {
            var user = await usuariorep.ExistEmail(email.Email);
            if (user == null) return StatusCode(404, "Email não cadastrado");

            user.Senha = user.Nome.Replace(" ", "").ToLower();

            var sucess = await usuariorep.Put(user);
            if (sucess == null) return StatusCode(400);

            try
            {
                usuariorep.Mensagem(user.Email, user.Senha);
                return StatusCode(200, "Senha atualizada. Email enviado com sucesso");
            }
            catch (System.Exception)
            {

                return StatusCode(200, "Senha atualizada. Falha ao enviar o email");
                throw;
            }

        }









        private bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
                return expressaoRegex.IsMatch(enderecoEmail);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}

public class ResetSenha
{
    public string Email { get; set; }
}