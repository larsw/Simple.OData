﻿using System;
using System.Collections.Generic;

namespace Simple.Data.OData
{
    public class FunctionNameConverter
    {
        private static readonly Dictionary<string, string> _functions = new Dictionary<string, string>
            {
                {"contains", "substringof"},
                {"like", "substringof"},
            };

        public string ConvertToODataName(string simpleFunctionName)
        {
            if (_functions.ContainsKey(simpleFunctionName.ToLowerInvariant()))
            {
                return _functions[simpleFunctionName.ToLowerInvariant()];
            }
            else
            {
                return simpleFunctionName.ToLowerInvariant();
            }
        }
    }

    /* TODO: implement functions

    String functions

    bool substringof(string po, string p1)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=substringof('Alfreds', CompanyName) eq true

    bool endswith(string p0, string p1)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=endswith(CompanyName, 'Futterkiste') eq true

    bool startswith(string p0, string p1)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=startswith(CompanyName, 'Alfr') eq true

    int length(string p0)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=length(CompanyName) eq 19

    int indexof(string p0, string p1)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=indexof(CompanyName, 'lfreds') eq 1

    string replace(string p0, string find, string replace)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=replace(CompanyName, ' ', '') eq 'AlfredsFutterkiste'

    string substring(string p0, int pos)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=substring(CompanyName, 1) eq 'lfreds Futterkiste'

    string substring(string p0, int pos, int length)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=substring(CompanyName, 1, 2) eq 'lf'

    string tolower(string p0)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=tolower(CompanyName) eq 'alfreds futterkiste'

    string toupper(string p0)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=toupper(CompanyName) eq 'ALFREDS FUTTERKISTE'

    string trim(string p0)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=trim(CompanyName) eq 'Alfreds Futterkiste'

    string concat(string p0, string p1)
    http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=concat(concat(City, ', '), Country) eq 'Berlin, Germany'

    Date Functions

    int day(DateTime p0)
    http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=day(BirthDate) eq 8

    int hour(DateTime p0)
    http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=hour(BirthDate) eq 0

    int minute(DateTime p0)
    http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=minute(BirthDate) eq 0

    int month(DateTime p0)
    http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=month(BirthDate) eq 12

    int second(DateTime p0)
    http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=second(BirthDate) eq 0

    int year(DateTime p0)
    http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=year(BirthDate) eq 1948

    Math Functions

    double round(double p0)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=round(Freight) eq 32

    decimal round(decimal p0)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=round(Freight) eq 32

    double floor(double p0)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=filter=round(Freight) eq 32

    decimal floor(decimal p0)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=floor(Freight) eq 32

    double ceiling(double p0)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=ceiling(Freight) eq 33

    decimal ceiling(decimal p0)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=floor(Freight) eq 33

    Type Functions

    bool IsOf(type p0)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=isof('NorthwindModel.Order')

    bool IsOf(expression p0, type p1)
    http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=isof(ShipCountry, 'Edm.String') 
 
     */
}