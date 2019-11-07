using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_comil.Models
{
    public partial class Evento
    {
        public Evento()
        {
            ResponsavelEventoTw = new HashSet<ResponsavelEventoTw>();
        }

        [Key]
        [Column("Evento_id")]
        public int EventoId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Column("Evento_data", TypeName = "date")]
        public DateTime EventoData { get; set; }
        public TimeSpan Horario { get; set; }
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; }
        [Required]
        [Column("Email_contato")]
        [StringLength(255)]
        public string EmailContato { get; set; }
        [Required]
        [Column("Status_evento")]
        [StringLength(50)]
        public string StatusEvento { get; set; }
        [Required]
        [StringLength(3)]
        public string Diversidade { get; set; }
        [Required]
        [StringLength(3)]
        public string Coffe { get; set; }
        [StringLength(255)]
        public string Foto { get; set; }
        [Column("Url_evento")]
        [StringLength(255)]
        public string UrlEvento { get; set; }
        [Column("Deletado_em", TypeName = "datetime")]
        public DateTime? DeletadoEm { get; set; }
        [Column("Categoria_id")]
        public int CategoriaId { get; set; }
        [Column("Sala_id")]
        public int SalaId { get; set; }
        [Column("Comunidade_id")]
        public int ComunidadeId { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        [InverseProperty("Evento")]
        public virtual Categoria Categoria { get; set; }
        [ForeignKey(nameof(ComunidadeId))]
        [InverseProperty("Evento")]
        public virtual Comunidade Comunidade { get; set; }
        [ForeignKey(nameof(SalaId))]
        [InverseProperty("Evento")]
        public virtual Sala Sala { get; set; }
        [InverseProperty("EventoNavigation")]
        public virtual ICollection<ResponsavelEventoTw> ResponsavelEventoTw { get; set; }
    }
}
