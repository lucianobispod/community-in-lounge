using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_comil.Models
{
    [Table("Responsavel_evento_tw")]
    public partial class ResponsavelEventoTw
    {
        [Key]
        [Column("Responsavel_evento_tw_id")]
        public int ResponsavelEventoTwId { get; set; }
        public int Evento { get; set; }
        [Column("Responsavel_evento")]
        public int ResponsavelEvento { get; set; }

        [ForeignKey(nameof(Evento))]
        [InverseProperty(nameof(ResponsavelEventoTw))]
        public virtual Evento EventoNavigation { get; set; }
        [ForeignKey(nameof(ResponsavelEvento))]
        [InverseProperty(nameof(Usuario.ResponsavelEventoTw))]
        public virtual Usuario ResponsavelEventoNavigation { get; set; }
    }
}
