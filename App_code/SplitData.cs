using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for SplitData
/// </summary>
public static class SplitData
{
    public static DataTable SplitDatasetWAPierce(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("OrderNo", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("ParcelNo", typeof(string));
        DataColumn Type = multiownertable.Columns.Add("Type", typeof(string));
        DataColumn Status = multiownertable.Columns.Add("Status", typeof(string));
        DataColumn TaxpayerName = multiownertable.Columns.Add("TaxpayerName", typeof(string));
        DataColumn SiteAddress = multiownertable.Columns.Add("SitusAddress", typeof(string));

        int dtcount = Dt.Rows.Count;
        string[] multiarray;
        for (int i = 0; i < dtcount; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Type, split[0]);
                row.SetField(Status, split[1]);
                row.SetField(TaxpayerName, split[2]);
                row.SetField(SiteAddress, split[3]);
                multiownertable.Rows.Add(row);
            }
        }

        return multiownertable;
    }
    public static DataTable SplitDatasetMultnomaOR(DataTable Dt)
    {

        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = multiownertable.Columns.Add("Owner_Name", typeof(string));
        DataColumn Alternate_Account_Number = multiownertable.Columns.Add("Alternate_Account_Number", typeof(string));
        DataColumn Situs_Address = multiownertable.Columns.Add("Situs_Address", typeof(string));
        DataColumn legal_Description = multiownertable.Columns.Add("legal_Description", typeof(string));

        int dtcount = Dt.Rows.Count;
        string[] multiarray;
        for (int i = 0; i < dtcount; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Alternate_Account_Number, split[1]);
                row.SetField(Situs_Address, split[2]);
                row.SetField(legal_Description, split[3]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetGwinnettGA(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn situs_address = multiownertable.Columns.Add("situs_address", typeof(string));
        DataColumn Ownername = multiownertable.Columns.Add("Ownername", typeof(string));

        int dtcount = Dt.Rows.Count;
        string[] multiarray;
        for (int i = 0; i < dtcount; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(situs_address, split[0]);
                row.SetField(Ownername, split[1]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetKernCA(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn siteaddress = multiownertable.Columns.Add("Site Address", typeof(string));
        DataColumn City = multiownertable.Columns.Add("City", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(siteaddress, split[0]);
                row.SetField(City, split[1]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetMecklenburgCA(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = multiownertable.Columns.Add("Owner_Name", typeof(string));
        DataColumn Property_Address = multiownertable.Columns.Add("Property_Address", typeof(string));
        DataColumn Description = multiownertable.Columns.Add("Description", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Property_Address, split[1]);
                row.SetField(Description, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetDistofColumbiaDC(DataTable Dt)
    {
        DataTable multiaddresstable = new DataTable();
        DataColumn order_no = multiaddresstable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiaddresstable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = multiaddresstable.Columns.Add("Owner_Name", typeof(string));
        DataColumn Address = multiaddresstable.Columns.Add("Address", typeof(string));

        int dtcount = Dt.Rows.Count;
        string[] multiarray;
        for (int i = 0; i < dtcount; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiaddresstable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Address, split[1]);
                multiaddresstable.Rows.Add(row);
            }
        }
        return multiaddresstable;
    }
    public static DataTable SplitDatasetDekalbGA(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = multiownertable.Columns.Add("Owner_Name", typeof(string));
        DataColumn Property_Address = multiownertable.Columns.Add("Property_Address", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Property_Address, split[1]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetWashoeNV(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn type = multiownertable.Columns.Add("type", typeof(string));
        DataColumn situs_address = multiownertable.Columns.Add("situs_address", typeof(string));
        DataColumn taxpayer_name = multiownertable.Columns.Add("taxpayer_name", typeof(string));


        int dtcount = Dt.Rows.Count;
        string[] propertyarray;
        for (int i = 0; i < dtcount; i++)
        {
            propertyarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = propertyarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(type, split[0]);
                row.SetField(situs_address, split[1]);
                row.SetField(taxpayer_name, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetFranklinOH(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Owner_Name = multiownertable.Columns.Add("Owner_Name", typeof(string));
        DataColumn Site_Address = multiownertable.Columns.Add("Site_Address", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Owner_Name, split[0]);
                row.SetField(Site_Address, split[1]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetSaintLouisMO(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn parcel_address = multiownertable.Columns.Add("parcel_address", typeof(string));
        DataColumn owner_name = multiownertable.Columns.Add("owner_name", typeof(string));

        int dtcount = Dt.Rows.Count;
        string[] multiarray;
        for (int i = 0; i < dtcount; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(parcel_address, split[0]);
                row.SetField(owner_name, split[1]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetSanJoaquinCA(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Assess = multiownertable.Columns.Add("Assess", typeof(string));        
        DataColumn Tra = multiownertable.Columns.Add("Tra", typeof(string));
        DataColumn address1 = multiownertable.Columns.Add("address1", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Assess, split[0]);                
                row.SetField(Tra, split[1]);
                row.SetField(address1, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetPlacerCA(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Assess = multiownertable.Columns.Add("Assess", typeof(string));
        DataColumn fee_parcel = multiownertable.Columns.Add("fee_parcel", typeof(string));
        DataColumn Tra = multiownertable.Columns.Add("Tra", typeof(string));
        DataColumn address1 = multiownertable.Columns.Add("address1", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Assess, split[0]);
                row.SetField(fee_parcel, split[1]);
                row.SetField(Tra, split[2]);
                row.SetField(address1, split[3]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasettitleflex(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Addrerss = multiownertable.Columns.Add("Addrerss", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        DataColumn County = multiownertable.Columns.Add("County", typeof(string));
        DataColumn City = multiownertable.Columns.Add("City", typeof(string));
        DataColumn State = multiownertable.Columns.Add("State", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Addrerss, split[0]);
                row.SetField(OwnerName, split[1]);
                row.SetField(County, split[2]);
                row.SetField(City, split[3]);
                row.SetField(State, split[4]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetHarrison(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        DataColumn Addrerss = multiownertable.Columns.Add("Addrerss", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(OwnerName, split[0]);
                row.SetField(Addrerss, split[1]);

                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetowneraddress(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        DataColumn Addrerss = multiownertable.Columns.Add("Addrerss", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(OwnerName, split[0]);
                row.SetField(Addrerss, split[1]);

                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetStcharles(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        DataColumn city = multiownertable.Columns.Add("city", typeof(string));
        DataColumn Addrerss = multiownertable.Columns.Add("Addrerss", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(OwnerName, split[0]);
                row.SetField(city, split[1]);
                row.SetField(Addrerss, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetTulsa(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn AccountNumber = multiownertable.Columns.Add("AccountNumber", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        DataColumn Street = multiownertable.Columns.Add("Street", typeof(string));
        DataColumn StreetDir = multiownertable.Columns.Add("StreetDir", typeof(string));
        DataColumn StreetName = multiownertable.Columns.Add("StreetName", typeof(string));
        DataColumn StreetSuf = multiownertable.Columns.Add("StreetSuf", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(AccountNumber, split[0]);
                row.SetField(OwnerName, split[1]);
                row.SetField(Street, split[2]);
                row.SetField(StreetDir, split[3]);
                row.SetField(StreetName, split[4]);
                row.SetField(StreetSuf, split[5]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetsummit(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Route = multiownertable.Columns.Add("Route", typeof(string));
        DataColumn Address = multiownertable.Columns.Add("Address", typeof(string));
        DataColumn owner = multiownertable.Columns.Add("owner", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Route, split[0]);
                row.SetField(Address, split[1]);
                row.SetField(owner, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetHennepin(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn id = multiownertable.Columns.Add("id", typeof(string));
        DataColumn street_address = multiownertable.Columns.Add("street_address", typeof(string));
        DataColumn city = multiownertable.Columns.Add("city", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(id, split[0]);
                row.SetField(street_address, split[1]);
                row.SetField(city, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetNewcastle(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Address = multiownertable.Columns.Add("Address", typeof(string));
        DataColumn City = multiownertable.Columns.Add("City", typeof(string));
        DataColumn Current_Owner = multiownertable.Columns.Add("Current_Owner", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Address, split[0]);
                row.SetField(City, split[1]);
                row.SetField(Current_Owner, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetpinal(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn parcelNumber = multiownertable.Columns.Add("parcelNumber", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        DataColumn Mailing_City = multiownertable.Columns.Add("Mailing_City", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(parcelNumber, split[0]);
                row.SetField(OwnerName, split[1]);
                row.SetField(Mailing_City, split[2]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetmarion(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn AccountNo = multiownertable.Columns.Add("AccountNo", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        DataColumn SitusAddress = multiownertable.Columns.Add("SitusAddress", typeof(string));
        DataColumn LegalDescription = multiownertable.Columns.Add("LegalDescription", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(AccountNo, split[0]);
                row.SetField(OwnerName, split[1]);
                row.SetField(SitusAddress, split[2]);
                row.SetField(LegalDescription, split[3]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetclackamas(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Location_Address = multiownertable.Columns.Add("Location_Address", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');
                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Location_Address, split[0]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetbaltimore(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Name = multiownertable.Columns.Add("Name", typeof(string));
        DataColumn Account = multiownertable.Columns.Add("Account", typeof(string));
        DataColumn Street = multiownertable.Columns.Add("Street", typeof(string));
        DataColumn Own_Occ = multiownertable.Columns.Add("Own_Occ", typeof(string));
        DataColumn Map = multiownertable.Columns.Add("Map", typeof(string));
        DataColumn Parcel = multiownertable.Columns.Add("Parcel", typeof(string));

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');
                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Name, split[0]);
                row.SetField(Account, split[1]);
                row.SetField(Street, split[2]);
                row.SetField(Own_Occ, split[3]);
                row.SetField(Map, split[4]);
                row.SetField(Parcel, split[5]);
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetpolk(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));
        DataColumn Geoparcel = multiownertable.Columns.Add("Geoparcel", typeof(string));
        DataColumn Class = multiownertable.Columns.Add("Class", typeof(string));
        DataColumn Address = multiownertable.Columns.Add("Address", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));        

        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');
                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Geoparcel, split[0]);
                row.SetField(Class, split[1]);
                row.SetField(Address, split[2]);
                row.SetField(OwnerName, split[3]);                
                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
    public static DataTable SplitDatasetaddressowner(DataTable Dt)
    {
        DataTable multiownertable = new DataTable();
        DataColumn order_no = multiownertable.Columns.Add("order_no", typeof(string));
        DataColumn parcelno = multiownertable.Columns.Add("Parcel_no", typeof(string));        
        DataColumn Addrerss = multiownertable.Columns.Add("Addrerss", typeof(string));
        DataColumn OwnerName = multiownertable.Columns.Add("OwnerName", typeof(string));
        string[] multiarray;
        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            multiarray = new string[] { Dt.Rows[i]["Data_Field_value"].ToString() };
            string source = multiarray[0].ToString();

            string[] lines = source.Split('\n');

            foreach (var line in lines)
            {
                string[] split = line.Split('~');

                DataRow row = multiownertable.NewRow();
                row.SetField(order_no, Dt.Rows[i]["order_no"].ToString());
                row.SetField(parcelno, Dt.Rows[i]["parcel_no"].ToString());
                row.SetField(Addrerss, split[0]);
                row.SetField(OwnerName, split[1]);

                multiownertable.Rows.Add(row);
            }
        }
        return multiownertable;
    }
}
