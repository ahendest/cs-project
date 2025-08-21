using System.ComponentModel.DataAnnotations;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.DTOs
{
    public class SupplierInvoiceLineCreateDTO
    {
        [Required] public int SupplierInvoiceId { get; set; }
        [Required] public int TankId { get; set; }
        [Required] public FuelType FuelType { get; set; }
        [Range(0.0001, double.MaxValue)] public decimal QuantityLiters { get; set; }
        [Range(0, double.MaxValue)] public decimal UnitPrice { get; set; }
    }
}
