﻿using System.Linq;
using System.Threading.Tasks;
using EfLocalDb;
using VerifyNUnit;
using NUnit.Framework;
using VerifyTests;

[TestFixture]
public class ClassicTests
{
    static SqlInstance<SampleDbContext> sqlInstance;

    #region AddedClassic
    [Test]
    public async Task Added()
    {
        using var database = await sqlInstance.Build();
        var data = database.Context;
        data.Companies.Add(new Company {Content = "before"});
        await Verifier.Verify(data);
    }
    #endregion

    #region DeletedClassic
    [Test]
    public async Task Deleted()
    {
        using var database = await sqlInstance.Build();
        var data = database.Context;
        data.Companies.Add(new Company {Content = "before"});
        await data.SaveChangesAsync();

        var company = data.Companies.Single();
        data.Companies.Remove(company);
        await Verifier.Verify(data);
    }
    #endregion

    #region ModifiedClassic
    [Test]
    public async Task Modified()
    {
        using var database = await sqlInstance.Build();
        var data = database.Context;
        var company = new Company
        {
            Content = "before"
        };
        data.Companies.Add(company);
        await data.SaveChangesAsync();

        data.Companies.Single().Content = "after";
        await Verifier.Verify(data);
    }
    #endregion

    [Test]
    public async Task WithNavigationProp()
    {
        using var database = await sqlInstance.Build();
        var data = database.Context;
        var company = new Company
        {
            Content = "companyBefore"
        };
        data.Companies.Add(company);
        var employee = new Employee
        {
            Content = "employeeBefore",
            Company = company
        };
        data.Employees.Add(employee);
        await data.SaveChangesAsync();

        data.Companies.Single().Content = "companyAfter";
        data.Employees.Single().Content = "employeeAfter";
        await Verifier.Verify(data);
    }

    [Test, Explicit]
    public async Task SomePropsModified()
    {
        using var database = await sqlInstance.Build();
        var data = database.Context;
        var company = new Company
        {
            Content = "before"
        };
        data.Companies.Add(company);
        await data.SaveChangesAsync();
        var entity = data.Companies.Attach(new Company {Id = company.Id});
        entity.Content = "after";
        data.Entry(entity).Property(_ => _.Content).IsModified = true;
        data.Configuration.ValidateOnSaveEnabled = false;
        await data.SaveChangesAsync();
        await Verifier.Verify(data);
    }

    [Test]
    public async Task UpdateEntity()
    {
        using var database = await sqlInstance.Build();
        var data = database.Context;

        data.Companies.Add(new Company
        {
            Content = "before"
        });
        await data.SaveChangesAsync();

        var company = data.Companies.Single();
        company.Content = "after";
        await Verifier.Verify(data);
    }

    #region QueryableClassic
    [Test]
    public async Task Queryable()
    {
        var database = await DbContextBuilder.GetDatabase("Queryable");
        var data = database.Context;
        var queryable = data.Companies.Where(x => x.Content == "value");
        await Verifier.Verify(queryable);
    }
    #endregion

    static ClassicTests()
    {
        #region EnableClassic
        VerifyEntityFrameworkClassic.Enable();
        #endregion
        sqlInstance = new SqlInstance<SampleDbContext>(
            constructInstance: connection => new SampleDbContext(connection),
            storage: Storage.FromSuffix<SampleDbContext>("Tests"));
    }
}