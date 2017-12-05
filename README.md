# BoatRental
Made a testdriven console application with SQLServer database and EntityFramework.
You got four projects in the Solution.
----------------------------------------
BoatRental -PROJECT Console application
----------------------------------------
-Program.cs (classes)
-Admin.cs
-Rental.cs
------------------------------------
BookingBoatSystem -Library PROJECT 
------------------------------------
-Booking.cs
------------------------------------
BookingBoatSystemTests -Test PROJECT 
------------------------------------
-BookingTests.cs
------------------------------------
Data -Entityframework PROJECT 
------------------------------------
1. Download and restore the databasefile:
BoatBookingSystem.bak
2. All projects got an App.config with a connection string.
  <connectionStrings>
    <add name="BoatBookingSystemEntities1" connectionString="metadata=res://*/BoatBookingSystemModel.csdl|res://*/BoatBookingSystemModel.ssdl|res://*/BoatBookingSystemModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=.;initial catalog=BoatBookingSystem;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework&quot;" providerName="System.Data.EntityClient" />
  </connectionStrings>
3. I hope the database will work good and that you don't need to change the connectionString.
