using static cs_project.Core.Entities.Enums;

namespace cs_project.Core.Entities
{
    public class Pump : BaseEntity
    {
        public int TankId { get; set; }

        public decimal CurrentVolume { get; set; }
        public PumpStatus Status { get; set; } = PumpStatus.Idle;

        public Tank Tank { get; set; } = null!;
        public ICollection<CustomerTransaction> CustomerTransactions { get; set; } = [];
    }
}
