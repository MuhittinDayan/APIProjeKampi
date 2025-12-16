using APIProjeKampi_WepApi.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data;

namespace APIProjeKampi_WepApi.ValidationRules
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün adını boş geçmeyin");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("En az 2 karakter girişi yapın");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("En fazla 50 karakter veri girişi yapın");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Ürün fiyatı boş geçilemez").GreaterThan(0).WithMessage("" +
                "Ürün fiyatı negatif olamaz").LessThan(1000).WithMessage("Ürün fiyatı bu kadar fazla olamaz");

            RuleFor(x => x.ProductDescription).NotEmpty().WithMessage("Ürün açıklaması boş geçilemez");

        }
    }
}
