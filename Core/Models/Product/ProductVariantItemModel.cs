using Core.Models.ProductSize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Product;

public class ProductVariantItemModel
{
    public long Id { get; set; }
    public int Weight { get; set; }
    public decimal Price { get; set; }

    public ProductSizeItemModel? ProductSize { get; set; }
}
