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
    
    public partial class PurchaseProduct
    {
        public int PurchaseProductId { get; set; }
        public int Count { get; set; }
        public int TotalPrice { get; set; }
        public System.DateTime TimeOfPurchase { get; set; }
        public Nullable<int> SertificateId { get; set; }
        public int ProductId { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual PurchaseSertificate PurchaseSertificate { get; set; }
    }
}