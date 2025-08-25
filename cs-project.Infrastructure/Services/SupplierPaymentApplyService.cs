using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Services
{
    public class SupplierPaymentApplyService : ISupplierPaymentApplyService
    {
        private readonly ISupplierPaymentApplyRepository _applyRepository;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public SupplierPaymentApplyService(
            ISupplierPaymentApplyRepository applyRepository,
            AppDbContext db,
            IMapper mapper)
        {
            _applyRepository = applyRepository;
            _db = db;
            _mapper = mapper;
        }

        private async Task<SupplierPayment?> GetPaymentAsync(int id) =>
            await _db.SupplierPayments.Include(p => p.Applies).FirstOrDefaultAsync(p => p.Id == id);

        private async Task<SupplierInvoice?> GetInvoiceAsync(int id) =>
            await _db.SupplierInvoices.Include(i => i.Lines).Include(i => i.Applies).FirstOrDefaultAsync(i => i.Id == id);

        private static decimal GetPaymentBalance(SupplierPayment payment, int? excludeApplyId = null)
        {
            var applied = payment.Applies
                .Where(a => excludeApplyId == null || a.Id != excludeApplyId)
                .Sum(a => a.AppliedAmount);
            return payment.Amount - applied;
        }

        private static decimal GetInvoiceBalance(SupplierInvoice invoice, int? excludeApplyId = null)
        {
            var total = invoice.Lines.Sum(l => l.QuantityLiters * l.UnitPrice);
            var applied = invoice.Applies
                .Where(a => excludeApplyId == null || a.Id != excludeApplyId)
                .Sum(a => a.AppliedAmount);
            return total - applied;
        }

        private static void Validate(SupplierPayment payment, SupplierInvoice invoice, decimal amount, int? applyId = null)
        {
            if (payment.SupplierId != invoice.SupplierId)
                throw new InvalidOperationException("Supplier mismatch between payment and invoice.");

            var paymentBalance = GetPaymentBalance(payment, applyId);
            if (amount > paymentBalance)
                throw new InvalidOperationException("Applied amount exceeds payment balance.");

            var invoiceBalance = GetInvoiceBalance(invoice, applyId);
            if (amount > invoiceBalance)
                throw new InvalidOperationException("Applied amount exceeds invoice balance.");
        }

        public async Task<PagedResult<SupplierPaymentApplyDTO>> GetSupplierPaymentApplyAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _applyRepository.QuerySupplierPaymentAppliesAsync(query);
            var dtoList = _mapper.Map<IEnumerable<SupplierPaymentApplyDTO>>(entities);
            return new PagedResult<SupplierPaymentApplyDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<SupplierPaymentApplyDTO>> GetAllSupplierPaymentAppliesAsync()
        {
            var applies = await _applyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierPaymentApplyDTO>>(applies);
        }

        public async Task<SupplierPaymentApplyDTO?> GetSupplierPaymentApplyByIdAsync(int id)
        {
            var apply = await _applyRepository.GetByIdAsync(id);
            return apply == null ? null : _mapper.Map<SupplierPaymentApplyDTO>(apply);
        }

        public async Task<SupplierPaymentApplyDTO> CreateSupplierPaymentApplyAsync(SupplierPaymentApplyCreateDTO dto)
        {
            var payment = await GetPaymentAsync(dto.SupplierPaymentId) ?? throw new InvalidOperationException("Payment not found");
            var invoice = await GetInvoiceAsync(dto.SupplierInvoiceId) ?? throw new InvalidOperationException("Invoice not found");

            Validate(payment, invoice, dto.AppliedAmount);

            var apply = _mapper.Map<SupplierPaymentApply>(dto);
            await _applyRepository.AddAsync(apply);
            await _applyRepository.SaveChangesAsync();
            return _mapper.Map<SupplierPaymentApplyDTO>(apply);
        }

        public async Task<bool> UpdateSupplierPaymentApplyAsync(int id, SupplierPaymentApplyCreateDTO dto)
        {
            var existing = await _applyRepository.GetByIdAsync(id);
            if (existing == null) return false;

            var payment = await GetPaymentAsync(dto.SupplierPaymentId) ?? throw new InvalidOperationException("Payment not found");
            var invoice = await GetInvoiceAsync(dto.SupplierInvoiceId) ?? throw new InvalidOperationException("Invoice not found");

            Validate(payment, invoice, dto.AppliedAmount, id);

            _mapper.Map(dto, existing);
            _applyRepository.Update(existing);
            return await _applyRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSupplierPaymentApplyAsync(int id)
        {
            var apply = await _applyRepository.GetByIdAsync(id);
            if (apply == null) return false;

            _applyRepository.Delete(apply);
            return await _applyRepository.SaveChangesAsync();
        }
    }
}
