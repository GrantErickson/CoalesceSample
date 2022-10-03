using IntelliTect.Coalesce;
using Microsoft.EntityFrameworkCore;

namespace CoalesceSample.Data.Models;
[Coalesce]
public class UserDetails
{
    public string Id { get; set; }
    public string UserId = null!;
    public string UserName = null!;
    public string UserEmail = null!;
    public List<string> UserRoles = new();
}

//[DefaultDataSource]
//[Coalesce]
//public class UserDetailsDataSource : StandardDataSource<UserDetails, AppDbContext>
//{
//    //public UserDetailsDataSource(CrudContext<AppDbContext> context) : base(context)
//    //{
//    //}

//    //public override IQueryable<UserDetails> GetQuery(IDataSourceParameters parameters)
//    //{
//    //    var Db.Users = ;
        
//    //}
//}