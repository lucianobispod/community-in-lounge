using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_comil.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Evento = new HashSet<Evento>();
            EventoTw = new HashSet<EventoTw>();
        }

        [Key]
        [Column("Categoria_id")]
        public int CategoriaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Column("Deletado_em", TypeName = "datetime")]
        public DateTime? DeletadoEm { get; set; }

        [InverseProperty("Categoria")]
        public virtual ICollection<Evento> Evento { get; set; }
        [InverseProperty("Categoria")]
        public virtual ICollection<EventoTw> EventoTw { get; set; }
    }
}
