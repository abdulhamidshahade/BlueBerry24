using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.StockDtos
{
    public abstract class StockBaseDto
    {


        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
