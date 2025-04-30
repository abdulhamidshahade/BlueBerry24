using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos
{
    class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
