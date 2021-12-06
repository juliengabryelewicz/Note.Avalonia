using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Note.Entities
{
    public class NoteEntity
    {
        public long NoteEntityId { get; set; }
        public string? Content { get; set; }

        [Required]
        public string? Title { get; set; }
    
        [DefaultValue(typeof(DateTime), "")]        
        public DateTime Updated { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; } = DateTime.UtcNow;
    }
}