using System.ComponentModel.DataAnnotations;

namespace Host.Data
{
    public class Product
    {
        [Key]
        public long ID { get; set; }

        [Required(ErrorMessage = "商品名称不得为空")]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
