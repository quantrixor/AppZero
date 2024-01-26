//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppZero.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Peripherals
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Peripherals()
        {
            this.PeripheralShelf = new HashSet<PeripheralShelf>();
            this.SpareParts = new HashSet<SpareParts>();
        }
    
        public int ID { get; set; }
        public int IDRack { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int IDTypeHall { get; set; }
    
        public virtual Rack Rack { get; set; }
        public virtual TypeHall TypeHall { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PeripheralShelf> PeripheralShelf { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpareParts> SpareParts { get; set; }
    }
}
