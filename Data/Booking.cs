//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int BookingNumber { get; set; }
        public string PersonNumber { get; set; }
        public int BoatID { get; set; }
        public System.DateTime DeliveyDateTime { get; set; }
        public Nullable<System.DateTime> ReturnDateTime { get; set; }
    
        public virtual Boat Boat { get; set; }
    }
}
