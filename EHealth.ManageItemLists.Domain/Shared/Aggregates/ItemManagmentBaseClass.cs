namespace EHealth.ManageItemLists.Domain.Shared.Aggregates
{
    public abstract class ItemManagmentBaseClass : EHealthDomainObject
    {
        public int Id {  get; set; }
        public string Code {  get;  set; }

        public bool Active { get;  set; }
        public object this[string propertyName]
        {
            get => this.GetType().GetProperty(propertyName).GetValue((object)this, (object[])null);
            set => this.GetType().GetProperty(propertyName).SetValue((object)this, value ?? (object)string.Empty, (object[])null);
        }
    }
}
