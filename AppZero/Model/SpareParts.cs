//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppZero.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpareParts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SpareParts()
        {
            this.SparePartsShelves = new HashSet<SparePartsShelves>();
        }
    
        public int ID { get; set; }
        public int IDRack { get; set; }
        public string Description { get; set; }
        public int IDPeripherals { get; set; }
        public int Count { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual Peripherals Peripherals { get; set; }
        public virtual Rack Rack { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SparePartsShelves> SparePartsShelves { get; set; }

       

    }
}