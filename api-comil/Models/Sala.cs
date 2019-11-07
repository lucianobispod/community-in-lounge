using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_comil.Models
{
    public partial class Sala
    {
        public Sala()
        {
            Evento = new HashSet<Evento>();
            EventoTw = new HashSet<EventoTw>();
        }

        [Key]
        [Column("Sala_id")]
        public int SalaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; }
        [Required]
        [Column("Qntd_pessoas")]
        [StringLength(20)]
        public string QntdPessoas { get; set; }
        [Column("Deletado_em", TypeName = "datetime")]
        public DateTime? DeletadoEm { get; set; }

        [InverseProperty("Sala")]
        public virtual ICollection<Evento> Evento { get; set; }
        [InverseProperty("Sala")]
        public virtual ICollection<EventoTw> EventoTw { get; set; }
    }
}
