using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class ProductDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public bool Visibility { get; set; }
    }
}
