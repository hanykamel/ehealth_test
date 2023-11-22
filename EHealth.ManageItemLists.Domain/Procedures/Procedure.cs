using EHealth.ManageItemLists.Domain.Categories;
using EHealth.ManageItemLists.Domain.ItemListPricing;
using EHealth.ManageItemLists.Domain.ItemLists;
using EHealth.ManageItemLists.Domain.LocalSpecialtyDepartments;
using EHealth.ManageItemLists.Domain.Shared.Aggregates;
using EHealth.ManageItemLists.Domain.Shared.Validation;
using EHealth.ManageItemLists.Domain.Sub_Categories;
using FluentValidation;

namespace EHealth.ManageItemLists.Domain.Procedures
{
    public class Procedure : EHealthDomainObject, IEntity<Guid>, IValidationModel<Procedure>
    {
        private Procedure()
        {

        }
        public Guid Id { get; private set; }
        public string? FoundationURI { get; private set; }
        public string? LinearizationURI { get; private set; }
        public string Code { get; private set; }
        public string? Target { get; private set; }
        public string? Action { get; private set; }
        public string? Means { get; private set; }
        public string? TitleAr { get; private set; }
        public string TitleEn { get; private set; }
        public string? Definition { get; private set; }
        public string? IndexTerms { get; private set; }
        public string? IncludesNotes { get; private set; }
        public string? CodeAlso { get; private set; }
        public string? ExcludesNotes { get; private set; }
        public int ServiceCategoryId { get; private set; }
        public Category ServiceCategory { get; private set; }
        public int SubCategoryId { get; private set; }
        public SubCategory Category { get; private set; }
        public int ItemListId { get; private set; }
        public ItemList ItemList { get; private set; }
        public int LocalSpecialtyDepartmentId { get; private set; }
        public LocalSpecialtyDepartment LocalSpecialtyDepartment { get; private set; }
        public bool? CapitationFlagPCU { get;private set; }
        public bool? PriorApproval  { get;private set; }
        public bool? PCUScope  { get;private set; }
        public DateTime DataEffectiveDateFrom { get; private set; }
        public DateTime? DataEffectiveDateTo { get; private set; }
        public IList<ItemListPrice> ItemListPrices { get; private set; } = new List<ItemListPrice>();

        public AbstractValidator<Procedure> Validator => throw new NotImplementedException();
    }
}
