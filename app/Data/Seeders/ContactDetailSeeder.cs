using Bogus;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data.Seeders;

public class ContactDetailSeeder(DbContext _context, List<ApplicationUser> _users)
    : BaseSeeder<ContactDetail>(_context)
{
    private readonly List<ApplicationUser> Users = _users;

    public new List<ContactDetail> Generate()
    {
        var fakeContactDetailGenerator = new Faker<ContactDetail>()
            .RuleFor(cd => cd.NameFirst, f => f.Person.FirstName)
            .RuleFor(cd => cd.NameLast, f => f.Person.LastName)
            .RuleFor(cd => cd.PhoneNumber, f => f.Person.Phone)
            .RuleFor(
                cd => cd.StreetAddress,
                f => f.Person.Address.Street + ", " + f.Person.Address.City
            )
            .RuleFor(cd => cd.PostalCode, f => f.Person.Address.ZipCode)
            .RuleFor(cd => cd.AdministrativeArea, f => f.Person.Address.State)
            .RuleFor(cd => cd.CountryCode, f => "US");

        List<ContactDetail> contactDetails = [];

        foreach (var user in Users)
        {
            var contactDetail = fakeContactDetailGenerator.Generate();

            contactDetail.EmailAddress = user.Email!;
            contactDetail.User = user;

            context.Add(contactDetail);
            contactDetails.Add(contactDetail);
        }

        return contactDetails;
    }
}
