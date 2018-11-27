using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaisesV2.Models
{
    public class Provincia
    {
        public int Id { get; set; }
        public string  Nombre { get; set; }
        [ForeignKey("pais")]
        public int  PaisId { get; set; }
        [JsonIgnore]
        public Pais Pais { get; set; }
    }
}
