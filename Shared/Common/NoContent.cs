using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class NoContent
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        public string Errors { get; set; }
    }
}
