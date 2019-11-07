using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_comil.Models
{
    public partial class Comunidade
    {
        public Comunidade()
        {
            Evento = new HashSet<Evento>();
        }

        [Key]
        [Column("Comunidade_id")]
        public int ComunidadeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; }
        [Required]
        [Column("Email_contato")]
        [StringLength(100)]
        public string EmailContato { get; set; }
        [Required]
        [Column("Telefone_contato")]
        [StringLength(20)]
        public string TelefoneContato { get; set; }
        [Required]
        [StringLength(255)]
        public string Foto { get; set; }
        [Column("Deletado_em", TypeName = "datetime")]
        public DateTime? DeletadoEm { get; set; }
        [Column("Responsavel_usuario_id")]
        public int ResponsavelUsuarioId { get; set; }

        [ForeignKey(nameof(ResponsavelUsuarioId))]
        [InverseProperty(nameof(Usuario.Comunidade))]
        public virtual Usuario ResponsavelUsuario { get; set; }
        [InverseProperty("Comunidade")]
        public virtual ICollection<Evento> Evento { get; set; }
    }
}
