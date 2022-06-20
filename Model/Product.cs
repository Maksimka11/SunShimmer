//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SunShimmer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProvisionServices = new HashSet<ProvisionService>();
            this.PurchaseProducts = new HashSet<PurchaseProduct>();
        }

        public string GetPhoto
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\Images\Products\" + Photo;
            }
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int Volume { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string ProductType { get; set; }
        public int ManufacturerId { get; set; }
    
        public virtual Manufacturer Manufacturer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProvisionService> ProvisionServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }
    }
}
