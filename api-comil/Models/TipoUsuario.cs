using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_comil.Models
{
    [Table("Tipo_usuario")]
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            Usuario = new HashSet<Usuario>();
        }

        [Key]
        [Column("Tipo_usuario_id")]
        public int TipoUsuarioId { get; set; }
        [Required]
        [StringLength(30)]
        public string Titulo { get; set; }
        [Column("Deletado_em", TypeName = "datetime")]
        public DateTime? DeletadoEm { get; set; }

        [InverseProperty("TipoUsuario")]
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
