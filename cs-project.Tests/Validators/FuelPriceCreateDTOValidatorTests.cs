using cs_project.Core.DTOs;
using cs_project.Validators;
using Xunit;

namespace cs_project.Tests.Validators
{
    public class FuelPriceCreateDTOValidatorTests
    {
        private readonly FuelPriceCreateDTOValidator _validator = new FuelPriceCreateDTOValidator();

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var dto = new FuelPriceCreateDTO
            {
                FuelType = "Diesel",
                CurrentPrice = 1.10,
                UpdatedAt = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Should_Fail_When_FuelType_Is_Empty()
        {
            var dto = new FuelPriceCreateDTO
            {
                FuelType = "",
                CurrentPrice = 1.10,
                UpdatedAt = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "FuelType");
        }

        [Fact]
        public void Should_Fail_When_CurrentPrice_Is_Zero()
        {
            var dto = new FuelPriceCreateDTO
            {
                FuelType = "Diesel",
                CurrentPrice = 0,
                UpdatedAt = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "CurrentPrice");
        }

        [Fact]
        public void Should_Fail_When_UpdatedAt_Is_In_Future()
        {
            var dto = new FuelPriceCreateDTO
            {
                FuelType = "Diesel",
                CurrentPrice = 0,
                UpdatedAt = DateTime.UtcNow.AddMinutes(10)
            };

            var result = _validator.Validate(dto);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == "CurrentPrice");

        }
    }
}
