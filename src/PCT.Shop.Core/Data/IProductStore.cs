using System.Collections.Generic;
using PCT.Shop.Core.Domain;

namespace PCT.Shop.Core.Data
{
    public interface IProductStore
    {
        IReadOnlyList<Product> GetProducts();
    }
}
