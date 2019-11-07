using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_comil.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Comunidade = new HashSet<Comunidade>();
            ResponsavelEventoTw = new HashSet<ResponsavelEventoTw>();
        }

        [Key]
        [Column("Usuario_id")]
        public int UsuarioId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Senha { get; set; }
        [Required]
        [StringLength(20)]
        public string Telefone { get; set; }
        [Required]
        [StringLength(255)]
        public string Foto { get; set; }
        [Required]
        [StringLength(30)]
        public string Genero { get; set; }
        [Column("Deletado_em", TypeName = "datetime")]
        public DateTime? DeletadoEm { get; set; }
        [Column("Tipo_usuario_id")]
        public int TipoUsuarioId { get; set; }

        [ForeignKey(nameof(TipoUsuarioId))]
        [InverseProperty("Usuario")]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [InverseProperty("ResponsavelUsuario")]
        public virtual ICollection<Comunidade> Comunidade { get; set; }
        [InverseProperty("ResponsavelEventoNavigation")]
        public virtual ICollection<ResponsavelEventoTw> ResponsavelEventoTw { get; set; }
    }
}
