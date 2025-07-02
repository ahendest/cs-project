using cs_project.Core.DTOs;
using cs_project.Validators;
using Xunit;
namespace cs_project.Tests.Validators
{
    public class TransactionsCreateDTOValidatorTests
    {
        private readonly TransactionCreateDTOValidator _validator = new TransactionCreateDTOValidator();

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var dto = new TransactionsCreateDTO
            {
                PumpId = 1,
                Liters = 10,
                PricePerLiter = 5,
                TotalPrice = 50,
                Timestamp = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }


        [Fact]
        public void Should_Pass_When_PumpId_Greater_Than_0()
        {
            var dto = new TransactionsCreateDTO
            {
                PumpId = 0, 
                Liters = 10,
                PricePerLiter = 5,
                TotalPrice = 50,
                Timestamp = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "PumpId");
        }

        [Fact]
        public void Should_Fail_When_Liters_Is_Zero()
        {
            var dto = new TransactionsCreateDTO
            {
                PumpId = 1,
                Liters = 0, // Invalid
                PricePerLiter = 5,
                TotalPrice = 0,
                Timestamp = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "Liters");
        }

        [Fact]
        public void Should_Fail_When_PricePerLiter_Is_Zero()
        {
            var dto = new TransactionsCreateDTO
            {
                PumpId = 1,
                Liters = 10,
                PricePerLiter = 0, // Invalid
                TotalPrice = 0,
                Timestamp = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "PricePerLiter");
        }

        [Fact]
        public void Should_Fail_When_TotalPrice_Is_Negative()
        {
            var dto = new TransactionsCreateDTO
            {
                PumpId = 1,
                Liters = 10,
                PricePerLiter = 5,
                TotalPrice = -10, // Invalid
                Timestamp = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "TotalPrice");
        }

        [Fact]
        public void Should_Fail_When_Timestamp_Is_Empty()
        {
            var dto = new TransactionsCreateDTO
            {
                PumpId = 1,
                Liters = 10,
                PricePerLiter = 5,
                TotalPrice = 50,
                Timestamp = default // Invalid
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "Timestamp");
        }

        [Fact]
        public void Should_Fail_When_Timestamp_Is_In_Future()
        {
            var dto = new TransactionsCreateDTO
            {
                PumpId = 1,
                Liters = 10,
                PricePerLiter = 5,
                TotalPrice = 50,
                Timestamp = DateTime.UtcNow.AddMinutes(15) // Invalid
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "Timestamp");
        }
    }
}
