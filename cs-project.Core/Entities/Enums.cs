
namespace cs_project.Core.Entities
{
    public class Enums
    {
        public enum FuelType { Petrol95, Petrol98, Diesel, LPG }
        public enum PumpStatus { Idle, Dispensing, Offline, Maintenance }
        public enum EmployeeRole { Cashier, Manager, Driver, Admin }
        public enum PaymentMethod { Cash, Card, FleetCard, BankTransfer, Mobile }
        public enum CorrectionType { Adjustment, Reversal, DataFix }
    }
}
